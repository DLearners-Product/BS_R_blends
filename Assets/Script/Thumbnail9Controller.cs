using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Thumbnail9Controller : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public GameObject answerObject;
    public GameObject questionParentObject;
    public string[] questionSTR;
    public AudioClip[] questionAudioClips;
    public string[] answerSTR;
    public GameObject[] optionObjects;
    public AudioClip[] optionAudioClips;
    public AudioClip wrongAudioClip;
    public TextMeshProUGUI questionCounter;
    public GameObject activityCompleted;
    int index = 0;
    Color initialColor;

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
        questionText.text = questionSTR[index];
        initialColor = answerObject.GetComponent<Image>().color;
        activityCompleted.SetActive(false);
        ShrinkOptionObjects();
        SpawnOptions();
        UpdateCounter();
#region DataSetter
        // Main_Blended.OBJ_main_blended.levelno = 9;
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

    string RemoveTag(string text) { return System.Text.RegularExpressions.Regex.Replace(text, "<.*?>", string.Empty); }

    void OnObjectDrop(GameObject dropObj, GameObject dropSlotObject)
    {
        string selectedOptionSTR = RemoveTag(dropObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
        if(answerSTR[index] == selectedOptionSTR)
        {
            ScoreManager.instance.RightAnswer(qIndex, questionID: questions[qIndex].id, answerID: GetOptionID(selectedOptionSTR));
            qIndex++;
            answerObject.GetComponent<Image>().color = Color.white;
            answerObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = selectedOptionSTR;
            Destroy(dropObj);
            AudioManager.PlayAudio(optionAudioClips[index]);
            Invoke(nameof(RightAnswer), optionAudioClips[index].length);
        }else{
            ScoreManager.instance.WrongAnswer(qIndex, questionID: questions[qIndex].id, answerID: GetOptionID(selectedOptionSTR));
            AudioManager.PlayAudio(wrongAudioClip);
        }
    }

    void RightAnswer()
    {
        StartCoroutine(StartMovingPanelAfter(0.5f));
    }

    IEnumerator StartMovingPanelAfter(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        MoveAnswerPanel(Vector3.up * 3f);
    }

    void ChangeQuestion()
    {
        index++;

        if(index == questionSTR.Length) {
            BlendedOperations.instance.NotifyActivityCompleted();
            activityCompleted.SetActive(true);
            return;
        }

        questionText.text = questionSTR[index];
        answerObject.GetComponent<Image>().color = initialColor;
        answerObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
    }

    public void OnSpeakerBTNClicked()
    {
        AudioManager.PlayAudio(questionAudioClips[index]);
    }

    void MoveAnswerPanel(Vector3 _position, bool canMove=true)
    {
        Vector3 endPos = answerObject.transform.position + _position;
        if(canMove)
            Utilities.Instance.ANIM_Move(answerObject.transform, endPos, callBack: () => { MoveQuestionPanel(Vector3.up * 6f); });
        else
            Utilities.Instance.ANIM_Move(answerObject.transform, endPos, callBack: OnSpeakerBTNClicked);
    }

    void MoveQuestionPanel(Vector3 _position)
    {
        Vector3 endPos = questionParentObject.transform.position + _position;
        Utilities.Instance.ANIM_MoveAndReturnToOriginalPos(questionParentObject.transform, endPos, ChangeQuestion, () => {
            MoveAnswerPanel(Vector3.down * 3f, false);
            UpdateCounter();
        });
    }

    void UpdateCounter()
    {
        questionCounter.text = $"{index + 1}/{questionSTR.Length}";
    }

    void ShrinkOptionObjects()
    {
        foreach (var opt in optionObjects)
        {
            Utilities.Instance.ANIM_ShrinkObject(opt.transform, 0f);
        }
    }

    void SpawnOptions(int index = 0)
    {
        if(index >= optionObjects.Length) { OnSpeakerBTNClicked(); return;}

        Utilities.Instance.ANIM_ShowBounceNormal(optionObjects[index].transform, shrinkUpTime: 0.25f, callback: () => {
            SpawnOptions(++index);
        });
    }

#region QA
    int GetOptionID(string selectedOption)
    {
        for (int i = 0; i < options.Length; i++)
        {
            if (options[i].text == selectedOption)
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
