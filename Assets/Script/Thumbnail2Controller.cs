using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thumbnail2Controller : MonoBehaviour
{
    public GameObject[] rainbowPoints;
    public float rotateSpeed = 0f;
    public float minAngle, maxAngle;
    public  GameObject cardObj;
    bool canLog = true;

    void Start()
    {
        Debug.Log($"UP :: {Vector3.up}");
        Debug.Log($"RIGHT :: {Vector3.right}");
        Debug.Log($"Forward :: {Vector3.forward}");
    }

    private void OnEnable() {
        ImageDragandDrop.onDrag += OnCardDragged;
    }

    private void OnDisable() {
        ImageDragandDrop.onDrag -= OnCardDragged;
    }

    void Update()
    {
        
    }

    void OnCardDragged(GameObject dragObj)
    {

        List<float> distance = new List<float>();
        for (int i = 0; i < rainbowPoints.Length; i++)
        {
            distance.Add(Vector3.Distance(rainbowPoints[i].transform.position, dragObj.transform.position));
        }

        List<float> _distance = new List<float>(distance);
        _distance.Sort();
        int nearObjectIndex = distance.IndexOf(_distance[0]);

        // dragObj.transform.LookAt(rainbowPoints[nearObjectIndex].transform, Vector3.right);

        Vector3 lookDir = (rainbowPoints[nearObjectIndex].transform.position - dragObj.transform.position).normalized;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        angle = Mathf.Clamp(angle, minAngle, maxAngle);

        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        Debug.Log(angle);

        // lookDir.y = 0f;
        // var __rotation = Quaternion.LookRotation(lookDir);
        dragObj.transform.rotation = Quaternion.Slerp(dragObj.transform.rotation, targetRotation, rotateSpeed);

        // float angle = Mathf.Atan2(rainbowPoints[nearObjectIndex].transform.position.y - dragObj.transform.position.y, rainbowPoints[nearObjectIndex].transform.position.x - dragObj.transform.position.x ) * Mathf.Rad2Deg;
        // Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        // dragObj.transform.rotation = Quaternion.RotateTowards(dragObj.transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        // var cardfaceDirection = Vector3.RotateTowards(dragObj.transform.position, rainbowPoints[nearObjectIndex].transform.position, rotateSpeed, 0.0f);
        // dragObj.transform.rotation = Quaternion.LookRotation(cardfaceDirection);
    }
}
