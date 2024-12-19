using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thumbnail2Controller : MonoBehaviour
{
    public GameObject[] rainbowPoints;
    bool canLog = true;

    void Start()
    {
        
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
        if(!canLog) return;

        List<float> distance = new List<float>();
        for (int i = 0; i < rainbowPoints.Length; i++)
        {
            distance.Add(Vector3.Distance(rainbowPoints[i].transform.position, dragObj.transform.position));
        }
        Debug.Log($"OLD first object {distance[0]}");
        List<float> _distance = new List<float>(distance);
        // List<float> _distance = distance;
        _distance.Sort();
        Debug.Log($"NEW first object {_distance[0]}");
        // distance.FindIndex(0, 1, _distance[0])
        // foreach (var item in distance)
        // {
        //     Debug.Log(item);
        // }
        // canLog = false;

        Debug.Log("-----------------------");
    }
}
