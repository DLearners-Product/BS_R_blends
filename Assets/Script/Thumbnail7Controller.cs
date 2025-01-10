using System.Collections;
using System.Collections.Generic;
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
        _spawnedObject.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = activityTexts[currentIndex];

        Utilities.Instance.ANIM_ShrinkObject(_spawnedObject.transform, 0f);
        _spawnedObject.transform.position = startPoint.position;
        MoveObjectWithScalUp(_spawnedObject.transform, displayPoint);
    }

    void MoveObjectWithScalUp(Transform objectToMove, Transform movePoint)
    {
        if(prevSpawnedObject == null)
            Utilities.Instance.ANIM_MoveWithScaleUp(objectToMove, movePoint.position, SpawnCounter);
        else
            Utilities.Instance.ANIM_MoveWithScaleUp(objectToMove, movePoint.position);
        StartCoroutine(WaitAndPlayAnimation(0.25f));
    }

    void MoveObjectWithScalDown(Transform objectToMove, Transform movePoint)
    {
        var _obj = objectToMove.gameObject;
        Utilities.Instance.ANIM_MoveWithScaleDown(objectToMove, movePoint.position, ()=>{Destroy(_obj);});
        // StartCoroutine(WaitAndPlayAnimation(0.25f));
    }

    IEnumerator WaitAndPlayAnimation(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _spawnedObject.GetComponent<Animator>().Play("shake_board");
    }

    void OnTextClicked()
    {
        TextMeshProUGUI _text = EventSystem.current.currentSelectedGameObject.GetComponent<TextMeshProUGUI>();
        var wordIndex = TMP_TextUtilities.FindIntersectingWord(_text, Input.mousePosition, Camera.main);
        Debug.Log(_text.text);

        if (wordIndex != -1)
        {
            var clickedText = _text.textInfo.wordInfo[wordIndex].GetWord();
            if(clickedText.Trim() == answerTexts[currentIndex].Trim())
            {
                currentIndex++;
                SpawnObject();
            }
        }
    }

    void UpdateCounter()
    {
        counterObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{currentIndex} / {answerTexts.Length}";
    }

}
