using System;
using System.Collections;
using System.Collections.Generic;
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
    public GameObject counterObj;
    int attendedAnswer;

#region QA
    private int qIndex;
    public GameObject questionGO;
    public GameObject[] optionsGO;
    public bool isActivityCompleted = false;
    public Dictionary<string, Component> additionalFields;
    Component[] questions;
    Component[] options;
    Component[] answers;
#endregion

    void Start()
    {
        attendedAnswer = 0;
        _questionPanelIntialPosition = questionPanel.transform;
        MoveQuestionBoardUp(7f);
        Utilities.Instance.ANIM_Move(counterObj.transform, counterObj.transform.position + (Vector3.up * 3), 0f);
        UpdatedCounter();
#region DataSetter
        // Main_Blended.OBJ_main_blended.levelno = 10;
        QAManager.instance.UpdateActivityQuestion();
        qIndex = 0;
        GetData(qIndex);
        GetAdditionalData();
#endregion
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
            ScoreManager.instance.WrongAnswer(attendedAnswer, questionID: questions[qIndex].id, answerID: GetOptionID(droppedObjText.Trim()));
            AudioManager.PlayAudio(wrongOptionSFX);
            return;
        }

        ScoreManager.instance.RightAnswer(attendedAnswer, questionID: questions[qIndex].id, answerID: GetOptionID(droppedObjText.Trim()));

        attendedAnswer++;

        UpdatedCounter();

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
        BlendedOperations.instance.NotifyActivityCompleted();
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
        Utilities.Instance.ANIM_Move(counterObj.transform, counterObj.transform.position + (Vector3.down * 3));
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

    void UpdatedCounter()
    {
        counterObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{attendedAnswer}/{optionText.Length}";
    }

#region QA
    int GetOptionID(string selectedOption)
    {
        for (int i = 0; i < options.Length; i++)
        {
            if (options[i].text.Contains(selectedOption))
            {
                return options[i].id;
            }
        }
        return -1;
    }

    void GetData(int questionIndex)
    {
        // question = QAManager.instance.GetQuestionAt(0, questionIndex);
        questions = QAManager.instance.GetAllQuestions(0);
        options = QAManager.instance.GetOption(0, questionIndex);
        answers = QAManager.instance.GetAnswer(0, questionIndex);
    }

    void GetAdditionalData()
    {
        additionalFields = QAManager.instance.GetAdditionalField(0);
    }
#endregion
}
