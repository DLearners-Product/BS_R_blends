using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class phonological : MonoBehaviour
{
    public int I_count, I_Qcount;
    public AudioSource AS_Wrong;
    public GameObject G_Final;
    public PhonologicalContent[] blendWordsData;
    public TextMeshProUGUI questionText, answerText;
    public TextMeshProUGUI optText1, optText2, optText3;
    public Transform position1, position2, position3;
    public AudioClip[] textAudioClips;
    int currentIndex = 0;
    Transform[] _optionsObjs;
    string displayText;
    GameObject[] displayObjs;

    void Start()
    {
        I_count = 0;
        G_Final.SetActive(false);
        _optionsObjs = new Transform[3];
        _optionsObjs[0] = optText1.transform;
        _optionsObjs[1] = optText2.transform;
        _optionsObjs[2] = optText3.transform;
        displayText = "";

        displayObjs = new GameObject[5];
        displayObjs[0] = questionText.transform.parent.gameObject;
        displayObjs[1] = answerText.transform.parent.gameObject;
        displayObjs[2] = optText1.transform.parent.gameObject;
        displayObjs[3] = optText2.transform.parent.gameObject;
        displayObjs[4] = optText3.transform.parent.gameObject;

        SpawnQuestion();
    }

    void PopDown()
    {
        int i = 0;
        foreach (var item in displayObjs)
        {
            if(++i == (displayObjs.Length - 1))
                Utilities.Instance.ANIM_ShrinkObject(item.transform, callback: PopUp);
            else
                Utilities.Instance.ANIM_ShrinkObject(item.transform);
        }
    }

    void PopUp()
    {
        // foreach (var item in displayObjs)
        // {
        //     Utilities.Instance.ANIM_ShowBounceNormal(item.transform);
        // }
        int i = 0;
        for (; i < displayObjs.Length - 1; i++)
        {
            Utilities.Instance.ANIM_ShowBounceNormal(displayObjs[i].transform);
        }
        Utilities.Instance.ANIM_ShowBounceNormal(displayObjs[i].transform, callback : OnSpeakerBTNClick);
    }

    void SpawnQuestion()
    {
        PopDown();
        displayText = blendWordsData[currentIndex].content;
        questionText.text = blendWordsData[currentIndex].content.Substring(2);
        answerText.text = displayText;
        optText1.text = blendWordsData[currentIndex].options[0];
        optText2.text = blendWordsData[currentIndex].options[1];
        optText3.text = blendWordsData[currentIndex].content.Substring(0, 2);
    }

    void ShiftOptionObjectRight()
    {
        Utilities.Instance.ANIM_Move(_optionsObjs[0].parent, position2.position);
        Utilities.Instance.ANIM_Move(_optionsObjs[1].parent, position3.position);
        Utilities.Instance.ANIM_Move(_optionsObjs[2].parent, position1.position, callBack: RefreshDisplayText);
        ShiftOptionRight();
    }

    void ShiftOptionObjectLeft()
    {
        Utilities.Instance.ANIM_Move(_optionsObjs[2].parent, position2.position);
        Utilities.Instance.ANIM_Move(_optionsObjs[1].parent, position1.position);
        Utilities.Instance.ANIM_Move(_optionsObjs[0].parent, position3.position, callBack: RefreshDisplayText);
        ShiftOptionLeft();
    }

    void ShiftOptionRight()
    {
        var _temp = _optionsObjs[0];
        _optionsObjs[0] = _optionsObjs[2];
        _optionsObjs[2] = _optionsObjs[1];
        _optionsObjs[1] = _temp;
    }

    void ShiftOptionLeft()
    {
        var _temp = _optionsObjs[0];
        _optionsObjs[0] = _optionsObjs[1];
        _optionsObjs[1] = _optionsObjs[2];
        _optionsObjs[2] = _temp;
    }

    int GetOptionPositionIndex(Transform selectedObj)
    {
        var selectedObjName = selectedObj.name;

        for (int i = 0; i < _optionsObjs.Length; i++)
        {
            // Debug.Log($"{_optionsObjs[i].parent.name} == {selectedObjName}");
            if(_optionsObjs[i].parent.name.Contains(selectedObjName)) return i;
        }
        return -1;
    }

    public void OnOptionBtnClick()
    {
        GameObject selectedObj = EventSystem.current.currentSelectedGameObject;
        int optionIndex = GetOptionPositionIndex(selectedObj.transform);
        // Debug.Log($"selectedObj : {selectedObj.name}");
        // Debug.Log($"Selected Object Index : {optionIndex}");
        displayText = $"{selectedObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text}{displayText.Substring(2)}";

        if(optionIndex == 0)
        {
            ShiftOptionObjectLeft();
        } else if(optionIndex == 1) {
            ShiftOptionObjectRight();
        }
    }

    public void OnSpeakerBTNClick()
    {
        foreach (var audioClip in textAudioClips)
        {
            if(audioClip.name.Contains(displayText))
            {
                AudioManager.PlayAudio(audioClip);
            }
        }
    }

    public void OnNextBtnClick()
    {
        currentIndex++;
        if(blendWordsData.Length == currentIndex)
            G_Final.SetActive(true);
        else
            SpawnQuestion();
    }

    void RefreshDisplayText()
    {
        Utilities.Instance.ANIM_ScaleUpDelayScaleDown(answerText.transform.parent);
        answerText.text = displayText;
    }
}

[System.Serializable]
public class PhonologicalContent
{
    public string content;
    public string[] options;
}