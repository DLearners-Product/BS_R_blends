using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Thumbnail2Controller : MonoBehaviour
{
    public GameObject[] rainbowPoints;
    public GameObject[] clouds;
    public AudioClip[] alphabetClips;
    public AudioClip alphabetPopUpClip;
    public AudioClip consonantActivityVO;
    public string[] consonantActivityChars;
    public float rotateSpeed = 0f;
    public float minAngle, maxAngle;
    public GameObject environmentObj;
    public Transform envStopMovPoint;
    public Transform[] cloudHorizontalStartPoints;
    public List<Transform> spawnPoints;
    public Transform cloudHorizontalEndPoint;
    public Transform cloudSpawnPoint;
    public GameObject rainbowObject;
    public AudioClip rainbowShineSFX;
    public Color changeColor;
    public Material normalMaterial,
                    glowMaterial;
    public Image bgPanel;
    public Button consonantActivityReplayBTN;
    public GameObject activityCompleted;
    int cloudSpawnIndexMin = 0, cloudSpawnIndexMax = 10;
    int consonantCounterIndex = -1;
    int counter = 0;
    int spawnPointIndex = 0;
    float mvoeStartTime = -1f;
    int selectedVowelsCount = 0;
    bool playConsonant = false;
    bool inConsonantEffect = false;
    Vector3 replayBTNOriginalPos;

    [Header("Card Settings")]
    public GameObject cardParent;
    public GameObject cardPrefab;
    public Transform cardSpawnPoint1, cardSpawnPoint2;
    public Transform cardSpawn1StartPosition, cardSpawn2StartPosition;
    public Sprite rearCardSprite,
                    frontCardSprite;
    public GameObject cardBG;
    public Sprite[] _vowelSprites;
    public AudioClip[] vowelsWordClips;
    public Transform mainCardObject;
    List<string> vowelsChar = new List<string>(){"a", "e", "i", "o", "u"};
    int A_ASCIIVal = 97;
    Sprite displaySprite;
    AudioClip currentVowelAudioClip;

    void Start()
    {

        int spawnPointCount = spawnPoints.Count;

        mvoeStartTime = 0f;
        for (int i = 0; i < spawnPointCount; i++)
        {
            InstantiateCloud();
        }
        counter = rainbowPoints.Length - 1;
        mvoeStartTime = -1f;
        InvokeRepeating(nameof(InstantiateCloud), 0f, 2f);
        Invoke(nameof(MoveEnvironment), 2f);
        cardBG.GetComponent<Image>().color = new Color(Color.white.r, Color.white.g, Color.white.b, 0);

        // MoveHighlightSpeakerBTN();
        // consonantCounterIndex = 0;
        // playConsonant = true;

        Utilities.Instance.ANIM_ShrinkObject(consonantActivityReplayBTN.transform, 0);
        Utilities.Instance.ANIM_ImageFade(bgPanel, 0f, 0f);
        replayBTNOriginalPos = consonantActivityReplayBTN.transform.position;

        ShrinkAlphabetObjects();
    }

    private void OnEnable() {
        // ImageDragandDrop.onDrag += OnCardDragged;
        // ImageDropSlot.onDropInSlot += OnCardDrop;
    }

    private void OnDisable() {
        // ImageDragandDrop.onDrag -= OnCardDragged;
        // ImageDropSlot.onDropInSlot -= OnCardDrop;
    }

    void InstantiateCloud()
    {
        var instantiatedCloud = Instantiate(clouds[UnityEngine.Random.Range(0, clouds.Length)].transform, cloudSpawnPoint);
        int spawnPoint = UnityEngine.Random.Range(cloudSpawnIndexMin, cloudSpawnIndexMax);

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
        var rainbowColor = rainbowObject.GetComponent<Image>().color;
        rainbowObject.SetActive(true);
        AudioManager.PlayAudio(rainbowShineSFX);
        Utilities.Instance.ANIM_ImageFill(rainbowObject.GetComponent<Image>(), rainbowShineSFX.length, PopAlphabetObjects);
        rainbowObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    void ResetRainbowLetterMaterial()
    {
        for (int i = 0; i < rainbowPoints.Length; i++)
        {
            rainbowPoints[i].GetComponent<TextMeshProUGUI>().fontMaterial = normalMaterial;
        }
    }

    void PopAlphabetObjects()
    {
        // var alphabetIndex = counter;
        // if(counter < (rainbowPoints.Length - 1))
        AudioManager.PlayAudio(alphabetPopUpClip, 0.5f);
        if(counter >= 1)
            Utilities.Instance.ScaleObject(rainbowPoints[counter--].transform, 1f, 0.1f, PopAlphabetObjects);
            // Utilities.Instance.ANIM_ShowBounceNormal(rainbowPoints[counter--].transform, 0.10f, 0.10f, PopAlphabetObjects);
        else
            Utilities.Instance.ScaleObject(rainbowPoints[counter--].transform, 1f, 0.1f, EnableCard);
            // Utilities.Instance.ANIM_ShowBounceNormal(rainbowPoints[counter--].transform, 0.10f, 0.10f, EnableCard);
    }

    void EnableCard()
    {
        cardParent.SetActive(true);
        Utilities.Instance.ANIM_ShowBounceNormal(cardParent.transform.GetChild(0));
    }

    // void SpawnCard(int cardSpawnIndex = 0)
    // {
    //     var _cardSpawnIndex = cardSpawnIndex;
    //     var spawnedCard = Instantiate(cardPrefab, cardSpawnPoint1);

    //     spawnedCard.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ((char)(A_ASCIIVal + cardSpawnIndex)).ToString();
    //     spawnedCard.transform.GetChild(0).gameObject.SetActive(true);
    //     spawnedCard.transform.GetComponent<Image>().sprite = frontCardSprite;

    //     spawnedCard.transform.position = cardParent.transform.GetChild(0).position;
    //     Vector3 endMovePosition;

    //     if(_cardSpawnIndex < 14)
    //     {
    //         endMovePosition = new Vector3(
    //                                     cardSpawn1StartPosition.position.x + (cardSpawnIndex * 1.2f), 
    //                                     cardSpawn1StartPosition.position.y,
    //                                     cardSpawn1StartPosition.position.z);
    //     }else{
    //         endMovePosition = new Vector3(
    //                                     cardSpawn2StartPosition.position.x + ((cardSpawnIndex - 14) * 1.2f), 
    //                                     cardSpawn2StartPosition.position.y,
    //                                     cardSpawn2StartPosition.position.z);
    //     }

    //     if(++_cardSpawnIndex < 26)
    //         Utilities.Instance.ANIM_Move(spawnedCard.transform, endMovePosition, 0.15f, callBack: () => { SpawnCard(_cardSpawnIndex); });
    //     else
    //         Utilities.Instance.ANIM_Move(spawnedCard.transform, endMovePosition, 0.15f);
    // }

    // void OnCardDragged(GameObject dragObj)
    // {
    //     List<float> distance = new List<float>();
    //     ResetRainbowLetterMaterial();
    //     for (int i = 0; i < rainbowPoints.Length; i++)
    //     {
    //         distance.Add(Vector3.Distance(rainbowPoints[i].transform.position, dragObj.transform.position));
    //     }

    //     List<float> _distance = new List<float>(distance);
    //     _distance.Sort();
    //     int nearObjectIndex = distance.IndexOf(_distance[0]);

    //     Vector3 lookDir = (rainbowPoints[nearObjectIndex].transform.position - dragObj.transform.position).normalized;

    //     rainbowPoints[nearObjectIndex].GetComponent<TextMeshProUGUI>().fontMaterial = glowMaterial;

    //     float angle = (Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg) - 90f;

    //     angle = Mathf.Clamp(angle, minAngle, maxAngle);

    //     Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

    //     dragObj.transform.rotation = Quaternion.Slerp(dragObj.transform.rotation, targetRotation, rotateSpeed);
    // }

    float PlayAlphabetSound(string alphabetChar)
    {
        foreach (var clip in alphabetClips)
        {
            if(clip.name.ToLower().Contains(alphabetChar.ToLower())) { AudioManager.PlayAudio(clip); return clip.length; }
        }

        return 0f;
    }

    void ReturnToOriginalState()
    {
        mainCardObject.GetChild(0).gameObject.SetActive(false);
        mainCardObject.GetChild(3).gameObject.SetActive(false);
        Utilities.Instance.ANIM_ImageFade(cardBG.GetComponent<Image>(), 0f, 1f);

        Utilities.Instance.ScaleObject(mainCardObject, 1f, 3f, () => { 
            cardBG.SetActive(false); mainCardObject.SetAsFirstSibling();
            if(selectedVowelsCount == vowelsChar.Count)
            {
                Utilities.Instance.ANIM_ShowBounceNormal(consonantActivityReplayBTN.transform, callback: () => {
                    MoveHighlightSpeakerBTN();
                    consonantCounterIndex = 0;
                    playConsonant = true;
                });
            }
        });

        Utilities.Instance.ANIM_RotateAndReveal(mainCardObject, 
                            () => { 
                                ChangeSprite(mainCardObject.GetChild(1).GetComponent<Image>(), rearCardSprite); 
                                DisplayRearCard(mainCardObject);
                            });
    }

    IEnumerator WaitAndCall(float waitTime, Action func)
    {
        yield return new WaitForSeconds(waitTime);
        func();
    }

    void StartConsonantActivity()
    {
        AudioManager.PlayAudio(consonantActivityVO);
        StartCoroutine(WaitAndCall(consonantActivityVO.length + 1f, () => {PlayConsonantVO(
            () => {
                Utilities.Instance.ANIM_MoveWithScaleUp(consonantActivityReplayBTN.transform, replayBTNOriginalPos);
                Utilities.Instance.ANIM_ImageFade(bgPanel, 0f, callback : () => {
                    bgPanel.gameObject.SetActive(false);
                    consonantActivityReplayBTN.transform.SetSiblingIndex(1);
                });
            }
            );
        }));
    }

    void MoveHighlightSpeakerBTN()
    {
        // consonantCounterIndex = 0;
        // playConsonant = true;
        consonantActivityReplayBTN.transform.SetSiblingIndex(4);
        Utilities.Instance.ANIM_MoveWithScaleUp(
                consonantActivityReplayBTN.transform, 
                Vector3.zero, 
                scaleSize: 2.5f, 
                effectSpeed: 3f, 
                onCompleteCallBack: () => { StartConsonantActivity(); }
        );
        bgPanel.gameObject.SetActive(true);
        Utilities.Instance.ANIM_ImageFade(bgPanel, 0.5f);
    }

    private void PlayConsonantVO(Action callback = null)
    {
        StartCoroutine(WaitAndCall(PlayAlphabetSound(
            consonantActivityChars[consonantCounterIndex]) + 1f, 
            () => {if(callback != null) callback();}
        ));
    }

    void DisplayPicInMainCard(Sprite displaySprite)
    {
        mainCardObject.SetSiblingIndex(4);
        cardBG.SetActive(true);
        Debug.Log($"selected Sprite name :: {displaySprite.name}");

        Utilities.Instance.ANIM_ImageFade(cardBG.GetComponent<Image>(), 0.5f, 1f);
        Utilities.Instance.ScaleObject(mainCardObject, 2f, 3f, DisplayInteractiveBtns);
        Utilities.Instance.ANIM_RotateAndReveal(mainCardObject, 
                            () => {
                                ChangeSprite(mainCardObject.GetChild(1).GetComponent<Image>(), frontCardSprite); 
                                AssignImage(mainCardObject, displaySprite); 
                            });
    }

    void ChangeSprite(Image obj, Sprite spriteToChange)
    {
        obj.sprite = spriteToChange;
    }

    void DisplayRearCard(Transform mainCardObj)
    {
        mainCardObj.transform.GetChild(0).gameObject.SetActive(false);
        mainCardObj.transform.GetChild(2).gameObject.SetActive(false);
        mainCardObj.transform.GetChild(3).gameObject.SetActive(false);
        mainCardObj.transform.GetChild(4).gameObject.SetActive(false);
    }

    void AssignImage(Transform mainCardObj, Sprite displaySprite)
    {
        mainCardObj.transform.GetChild(2).gameObject.SetActive(true);
        mainCardObject.GetChild(4).gameObject.SetActive(true);

        mainCardObj.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = displaySprite;
        mainCardObj.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = displaySprite.name;
    }

    void DisplayInteractiveBtns()
    {
        Utilities.Instance.ANIM_ShrinkObject(mainCardObject.GetChild(3), 0f);
        mainCardObject.GetChild(0).gameObject.SetActive(true);
        mainCardObject.GetChild(3).gameObject.SetActive(true);

        Utilities.Instance.ANIM_ShowBounceNormal(mainCardObject.GetChild(3));
        Utilities.Instance.ANIM_MoveWithScaleUp(mainCardObject.GetChild(0), mainCardObject.GetChild(0).position + (Vector3.down * 1.5f), onCompleteCallBack: OnSpeakeBTNClick);
    }

    void GetSelectedVowelAssets(string selectedSTR)
    {
        for (int i = 0; i < _vowelSprites.Length; i++)
        {
            Debug.Log($"-- {selectedSTR.ToLower()} :: {_vowelSprites[i].name.Substring(0, 1)}");
            if(_vowelSprites[i].name.Substring(0, 1).ToLower().Equals(selectedSTR.ToLower()))
            {
                displaySprite = _vowelSprites[i];
                currentVowelAudioClip = vowelsWordClips[i];
            }
        }
    }

#region CLICK LISTENER
    public void OnCancelBTNClick()
    {
        Utilities.Instance.ANIM_HideBounce(mainCardObject.GetChild(3));
        Utilities.Instance.ANIM_MoveWithScaleDown(mainCardObject.GetChild(0), mainCardObject.GetChild(0).position + (Vector3.up * 1.5f), ReturnToOriginalState);
    }

    public void OnSpeakeBTNClick()
    {
        AudioManager.PlayAudio(currentVowelAudioClip);
    }

    public void OnConsonantSpeakerBTNClick()
    {
        // inConsonantEffect = false;
        PlayConsonantVO();
    }

    public void OnAlphabetsClicked()
    {
        var clickedObj = EventSystem.current.currentSelectedGameObject;
        string selectedLetter = clickedObj.GetComponent<TextMeshProUGUI>().text;

        // ResetRainbowLetterMaterial();

        if(playConsonant && consonantActivityChars[consonantCounterIndex].Equals(selectedLetter))
        {
            var waitTime = PlayAlphabetSound(consonantActivityChars[consonantCounterIndex++]);
            clickedObj.GetComponent<TextMeshProUGUI>().color = changeColor;

            if(consonantCounterIndex == consonantActivityChars.Length) { activityCompleted.SetActive(true); return; }

            Invoke(nameof(MoveHighlightSpeakerBTN), waitTime + 1.5f);

            return;
        }

        foreach (var alphabet in rainbowPoints)
        {
            if(alphabet.GetComponent<TextMeshProUGUI>().text.ToLower().Equals(selectedLetter.ToLower()))
            {
                if(vowelsChar.Contains(selectedLetter.ToLower()))
                {
                    selectedVowelsCount++;
                    GetSelectedVowelAssets(selectedLetter.ToLower());
                    DisplayPicInMainCard(displaySprite);
                    return;
                }
                // alphabet.GetComponent<TextMeshProUGUI>().color = fontChangeColor;
            }
        }
        PlayAlphabetSound(selectedLetter);
    }


#endregion
}
