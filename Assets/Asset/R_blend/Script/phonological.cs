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
    int currentIndex = 0;
    Transform[] _optionsObjs;

    void Start()
    {
        I_count = 0;
        G_Final.SetActive(false);
        _optionsObjs = new Transform[3];
        _optionsObjs[0] = optText1.transform;
        _optionsObjs[1] = optText2.transform;
        _optionsObjs[2] = optText3.transform;
        SpawnQuestion();
    }

    void SpawnQuestion()
    {
        questionText.text = blendWordsData[currentIndex].content.Substring(2);
        answerText.text = blendWordsData[currentIndex].content;
        optText1.text = blendWordsData[currentIndex].options[0];
        optText2.text = blendWordsData[currentIndex].options[1];
        optText3.text = blendWordsData[currentIndex].content.Substring(0, 2);
    }

    void ShiftOptionObjectRight()
    {
        Utilities.Instance.ANIM_Move(_optionsObjs[0].parent, position2.position);
        Utilities.Instance.ANIM_Move(_optionsObjs[1].parent, position3.position);
        Utilities.Instance.ANIM_Move(_optionsObjs[2].parent, position1.position);
        ShiftOptionRight();
    }

    void ShiftOptionObjectLeft()
    {
        Utilities.Instance.ANIM_Move(_optionsObjs[2].parent, position1.position);
        Utilities.Instance.ANIM_Move(_optionsObjs[1].parent, position2.position);
        Utilities.Instance.ANIM_Move(_optionsObjs[0].parent, position3.position);
        ShiftOptionLeft();
    }

    void ShiftOptionRight()
    {
        var _temp = _optionsObjs[0];
        _optionsObjs[0] = _optionsObjs[1];
        _optionsObjs[1] = _optionsObjs[2];
        _optionsObjs[2] = _temp;
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

        return -1;
    }

    public void OnOptionBtnClick()
    {
        GameObject selectedObj = EventSystem.current.currentSelectedGameObject;
    }
}

[System.Serializable]
public class PhonologicalContent
{
    public string content;
    public string[] options;
}