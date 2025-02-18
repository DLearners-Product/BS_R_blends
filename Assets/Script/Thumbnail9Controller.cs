using System.Collections;
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

    void Start()
    {
        questionText.text = questionSTR[index];
        initialColor = answerObject.GetComponent<Image>().color;
        activityCompleted.SetActive(false);
        ShrinkOptionObjects();
        SpawnOptions();
        UpdateCounter();
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
        string selectedOptionSTR = dropObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        if(answerSTR[index] == RemoveTag(selectedOptionSTR))
        {
            answerObject.GetComponent<Image>().color = Color.white;
            answerObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = selectedOptionSTR;
            Destroy(dropObj);
            AudioManager.PlayAudio(optionAudioClips[index]);
            Invoke(nameof(RightAnswer), optionAudioClips[index].length);
        }else{
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

        if(index == questionSTR.Length) { Debug.Log("came to if statement"); activityCompleted.SetActive(true); return; }

        Debug.Log("In changeQuestion func....");

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

}
