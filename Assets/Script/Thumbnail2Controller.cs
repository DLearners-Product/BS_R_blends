using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thumbnail2Controller : MonoBehaviour
{
    public GameObject[] rainbowPoints;
    public GameObject[] clouds;
    public float rotateSpeed = 0f;
    public float minAngle, maxAngle;
    public GameObject environmentObj;
    public Transform envStopMovPoint;
    public Transform[] cloudHorizontalStartPoints;
    public List<Transform> spawnPoints;
    public Transform cloudHorizontalEndPoint;
    public Transform cloudSpawnPoint;
    public GameObject rainbowObject;
    int cloudSpawnIndexMin = 0, cloudSpawnIndexMax = 10;
    int counter = 0;
    int spawnPointIndex = 0;
    float mvoeStartTime = -1f;

    [Header("Card Settings")]
    public GameObject cardParent;
    public GameObject cardPrefab;
    public Transform cardSpawnPoint1, cardSpawnPoint2;
    public Transform cardSpawn1StartPosition, cardSpawn2StartPosition;

    void Start()
    {

        int spawnPointCount = spawnPoints.Count;

        mvoeStartTime = 0f;
        for (int i = 0; i < spawnPointCount; i++)
        {
            InstantiateCloud();
        }

        mvoeStartTime = -1f;
        InvokeRepeating(nameof(InstantiateCloud), 0f, 2f);
        Invoke(nameof(MoveEnvironment), 2f);
        // InvokeRepeating(nameof(MoveEnvironment), 2f, 2f);
        ShrinkAlphabetObjects();
    }

    private void OnEnable() {
        ImageDragandDrop.onDrag += OnCardDragged;
    }

    private void OnDisable() {
        ImageDragandDrop.onDrag -= OnCardDragged;
    }

    void InstantiateCloud()
    {
        var instantiatedCloud = Instantiate(clouds[Random.Range(0, clouds.Length)].transform, cloudSpawnPoint);
        int spawnPoint = Random.Range(cloudSpawnIndexMin, cloudSpawnIndexMax);

        Transform spawnPosition = (spawnPoints.Count != spawnPointIndex) ? spawnPoints[spawnPointIndex++] : cloudHorizontalStartPoints[spawnPoint];

        instantiatedCloud.transform.position = spawnPosition.position;
        instantiatedCloud.transform.SetParent(cloudSpawnPoint);

        instantiatedCloud.GetComponent<RunningClouds>().SetEndPoint(cloudHorizontalEndPoint, mvoeStartTime);
    }

    void ShrinkAlphabetObjects()
    {
        foreach (var item in rainbowPoints)
        {
            Utilities.Instance.ANIM_ShrinkObject(item.transform, 0f);
        }
    }

    void MoveEnvironment()
    {
        Utilities.Instance.ANIM_Move(environmentObj.transform, envStopMovPoint.position, 4f, EnableRainBow);
    }

    void EnableRainBow()
    {
        // rainbowObject.GetComponent<Image>().enabled = false;
        var rainbowColor = rainbowObject.GetComponent<Image>().color;
        rainbowObject.GetComponent<Image>().color = new Color(rainbowColor.r, rainbowColor.g, rainbowColor.b, 0);
        // rainbowObject.GetComponent<Image>().enabled = true;
        rainbowObject.SetActive(true);
        StartCoroutine(MakeRainBowVisible());
    }

    IEnumerator MakeRainBowVisible()
    {
        var rainbowColor = rainbowObject.GetComponent<Image>().color;
        float alphaValue = 0;
        while (true)
        {
            if(alphaValue >= 1f) break;
            yield return new WaitForSeconds(0.15f);
            alphaValue += 0.05f;
            rainbowObject.GetComponent<Image>().color = new Color(rainbowColor.r, rainbowColor.g, rainbowColor.b, Mathf.Clamp01(alphaValue));
        }
        rainbowObject.transform.GetChild(0).gameObject.SetActive(true);
        PopAlphabetObjects();
    }

    void PopAlphabetObjects()
    {
        // var alphabetIndex = counter;
        if(counter < (rainbowPoints.Length - 1))
            Utilities.Instance.ANIM_ShowBounceNormal(rainbowPoints[counter++].transform, 0.15f, 0.15f, PopAlphabetObjects);
        else
            Utilities.Instance.ANIM_ShowBounceNormal(rainbowPoints[counter++].transform, 0.15f, 0.15f, EnableCard);
    }

    void EnableCard()
    {
        cardParent.SetActive(true);
        Utilities.Instance.ANIM_ShowBounceNormal(cardParent.transform.GetChild(0), callback: () => { SpawnCard(); });
    }

    void SpawnCard(int cardSpawnIndex = 0)
    {
        var _cardSpawnIndex = cardSpawnIndex;
        var spawnedCard = Instantiate(cardPrefab, cardSpawnPoint1);
        // spawnedCard.transform. = new Vector3(
        //                                     cardSpawn1StartPosition.position.x + (cardSpawnIndex * 130), 
        //                                     cardSpawn1StartPosition.position.y,
        //                                     cardSpawn1StartPosition.position.z);
        spawnedCard.transform.position = cardParent.transform.GetChild(0).position;
        Vector3 endMovePosition = new Vector3(
                                            cardSpawn1StartPosition.position.x + (cardSpawnIndex * 1.2f), 
                                            cardSpawn1StartPosition.position.y,
                                            cardSpawn1StartPosition.position.z);
        Debug.Log($"Vector Position :: {endMovePosition} --> Index :: {cardSpawnIndex}");
        if(_cardSpawnIndex != 14)
            Utilities.Instance.ANIM_Move(spawnedCard.transform, endMovePosition, callBack: () => { SpawnCard(++_cardSpawnIndex); });
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
