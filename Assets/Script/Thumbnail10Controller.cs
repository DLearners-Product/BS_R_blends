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
    public AudioClip[] optionClips;
    public Transform[] placementPositions;
    public AudioClip passageAudioClip, wrongOptionSFX;
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

        if(!droppedObjText.ToLower().Trim().Equals(dropSlotText.ToLower().Trim())) {
            AudioManager.PlayAudio(wrongOptionSFX);
            return;
        }

        attendedAnswer++;
        Destroy(dropedObj);
        dropSlotObject.transform.GetChild(0).gameObject.SetActive(true);
        Utilities.Instance.ANIM_CorrectScaleEffect(dropSlotObject.transform.GetChild(0));
        var optionClip = GetOptionClip(dropSlotText);
        AudioManager.PlayAudio(optionClip);

        if(attendedAnswer == optionText.Length)
        {
            StartCoroutine(WaitAndExecute(optionClip.length + 1f, ()=>{
                AudioManager.PlayAudio(passageAudioClip);
                StartCoroutine(WaitAndExecute(passageAudioClip.length + 1f, EnableActivityCompleted));
            }));
        }
    }

    void EnableActivityCompleted() {
        activityCompleted.SetActive(true);
    }

    AudioClip GetOptionClip(string optionSTR)
    {
        foreach (var optionClip in optionClips)
        {
            if(optionClip.name.ToLower().Contains(optionSTR.ToLower())) {
                return optionClip;
            }
        }
        return null;
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
