using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Thumbnail7Controller : MonoBehaviour
{
    public AnimationClip _boardShakeAnimationClip;
    public Transform startPoint, displayPoint, endPoint;
    public GameObject _imageDisplayPanelPrefab;
    public Transform spawnParent;
    public string[] activityTexts;
    public Sprite[] textRelatedSprites;
    public string[] answerTexts;
    public GameObject counterObj;
    public GameObject activityCompletedScreen;
    public AudioClip[] questionAudioClips;
    public AudioClip[] answerAudioClips;
    public AudioClip wrongAudioClip;
    GameObject _spawnedObject;
    GameObject prevSpawnedObject;
    int currentIndex = 0;

    void Start()
    {
        currentIndex = 0;
        _spawnedObject = null;
        prevSpawnedObject = null;
        SpawnObject();
    }

    void SpawnCounter()
    {
        Utilities.Instance.ANIM_ShrinkObject(counterObj.transform.GetChild(0), 0f);
        Utilities.Instance.ANIM_Move(counterObj.transform, counterObj.transform.position + (Vector3.down * 2), callBack: PopCounterText);
    }

    void PopCounterText()
    {
        Utilities.Instance.ANIM_CorrectScaleEffect(counterObj.transform.GetChild(0));
    }

    void AssignTextValuesToSpawnedObj(string questionText)
    {
        _spawnedObject.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = questionText;
    }

    void SpawnObject()
    {
        if(currentIndex == activityTexts.Length) {activityCompletedScreen.SetActive(true); return;}
        UpdateCounter();

        prevSpawnedObject = _spawnedObject;
        if(prevSpawnedObject != null)
        {
            MoveObjectWithScalDown(prevSpawnedObject.transform, endPoint);
        }

        _spawnedObject = Instantiate(_imageDisplayPanelPrefab, spawnParent);
        
        _spawnedObject.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Button>().onClick.AddListener(OnTextClicked);
        _spawnedObject.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = textRelatedSprites[currentIndex];
        AssignTextValuesToSpawnedObj(activityTexts[currentIndex]);
        _spawnedObject.GetComponent<AudioSource>().clip = questionAudioClips[currentIndex];

        StartCoroutine(WaitAndCallFun(_boardShakeAnimationClip.length, ()=>{
            _spawnedObject.GetComponent<AudioSource>().Play();
        }));

        Utilities.Instance.ANIM_ShrinkObject(_spawnedObject.transform, 0f);
        _spawnedObject.transform.position = startPoint.position;
        MoveObjectWithScalUp(_spawnedObject.transform, displayPoint);
    }

    void MoveObjectWithScalUp(Transform objectToMove, Transform movePoint)
    {
        if(prevSpawnedObject == null)
            Utilities.Instance.ANIM_MoveWithScaleUp(objectToMove, movePoint.position, onCompleteCallBack: SpawnCounter);
        else
            Utilities.Instance.ANIM_MoveWithScaleUp(objectToMove, movePoint.position);
        StartCoroutine(WaitAndCallFun(0.25f, ()=>{
            _spawnedObject.GetComponent<Animator>().Play("shake_board");
        }));
    }

    void MoveObjectWithScalDown(Transform objectToMove, Transform movePoint)
    {
        var _obj = objectToMove.gameObject;
        Utilities.Instance.ANIM_MoveWithScaleDown(objectToMove, movePoint.position, ()=>{Destroy(_obj);});
    }

    IEnumerator WaitAndCallFun(float waitTime, Action _func)
    {
        yield return new WaitForSeconds(waitTime);
        _func();
    }

    void OnTextClicked()
    {
        TextMeshProUGUI _text = EventSystem.current.currentSelectedGameObject.GetComponent<TextMeshProUGUI>();
        var wordIndex = TMP_TextUtilities.FindIntersectingWord(_text, Input.mousePosition, Camera.main);

        if (wordIndex != -1)
        {
            var clickedText = _text.textInfo.wordInfo[wordIndex].GetWord();
            if(clickedText.Trim() == answerTexts[currentIndex].Trim())
            {
                string[] textArr = _text.text.Split(' ');
                textArr[wordIndex] = $"<color=green><b>{textArr[wordIndex]}</b></color>";
                _spawnedObject.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<HighlightTextOnHover>().enabled = false;

                AssignTextValuesToSpawnedObj(string.Join(" ", textArr));

                AudioManager.PlayAudio(answerAudioClips[currentIndex]);
                StartCoroutine(WaitAndCallFun(answerAudioClips[currentIndex].length + 1f, () => {
                    currentIndex++;
                    SpawnObject();
                }));

            }else{
                AudioManager.PlayAudio(wrongAudioClip);
            }
        }
    }

    void UpdateCounter()
    {
        counterObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{currentIndex + 1} / {answerTexts.Length}";
    }

}
