using System;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class Thumbnail10Controller : MonoBehaviour
{
    public GameObject questionPanel;
    Transform _questionPanelIntialPosition;
    public GameObject optionObject;
    public Transform optionInitialisePosition;
    public Transform optionsParent;
    public GameObject activityCompleted;
    public string[] optionText;
    public Transform[] placementPositions;
    int attendedAnswer;

    void Start()
    {
        attendedAnswer = 0;
        _questionPanelIntialPosition = questionPanel.transform;
        MoveQuestionBoardUp(7f);
    }

    void OnEnable() {
        ImageDropSlot.onDropInSlot += OnObjectDrop;
    }

    private void OnDisable() {
        ImageDropSlot.onDropInSlot -= OnObjectDrop;
    }

    void OnObjectDrop(GameObject dropedObj, GameObject dropSlotObject)
    {
        string droppedObjText = dropedObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        string dropSlotText = dropSlotObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        dropSlotText = Regex.Replace(dropSlotText, "<.*?>", string.Empty); 

        if(droppedObjText.ToLower().Trim().Equals(dropSlotText.ToLower().Trim()))
        {
            attendedAnswer++;
            Destroy(dropedObj);
            dropSlotObject.transform.GetChild(0).gameObject.SetActive(true);
            Utilities.Instance.ANIM_CorrectScaleEffect(dropSlotObject.transform.GetChild(0));
        }

        if(attendedAnswer == optionText.Length)
        {
            StartCoroutine(WaitAndExecute(1f, () => {
                activityCompleted.SetActive(true);
            }));
        }

        Debug.Log($"Dropped object Text :: {droppedObjText} Drop Slot Text :: {dropSlotText}");
    }

    IEnumerator WaitAndExecute(float waitTime, Action _func)
    {
        yield return new WaitForSeconds(waitTime);
        _func();
    }

    void MoveQuestionBoardUp(float moveDistance)
    {
        Vector3 endPosition = _questionPanelIntialPosition.position + (Vector3.up * moveDistance);

        Utilities.Instance.ANIM_MoveAndReturnToOriginalPos(questionPanel.transform, endPosition, destReachTime: 0f, _callbackOnEnd: SpawnOptions);
    }

    void SpawnOptions()
    {
        for (int i = 0; i < optionText.Length; i++)
        {
            var spawnedObj = Instantiate(optionObject, optionsParent);
            spawnedObj.SetActive(true);
            spawnedObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = optionText[i];
            Utilities.Instance.ANIM_Move(spawnedObj.transform, placementPositions[i].position, callBack: ()=>{
                spawnedObj.GetComponent<ImageDragandDrop>().ResetCurrentPositionAsDefault();
            });
        }
        optionsParent.SetSiblingIndex(3);
    }

}
