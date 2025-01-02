using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Thumbnail6Controller : MonoBehaviour
{
    public GameObject IMG_questionImagePanel;
    public GameObject G_optionsParent;
    public Transform T_startPoint, T_endPoint;
    public Sprite[] SPR_questionSprite;
    public GameObject G_questionImagePrefab;
    GameObject _currentQuestion = null;
    GameObject _prevQuestion = null;
    GameObject _currentSelectedOption = null;
    int _currentIndex = 0;
    bool B_canInteract = true;

    void Start()
    {
        OpenOptions();
        Invoke(nameof(CloseOptions), 3f);
        Invoke(nameof(SpawnQuestion), 3.5f);
    }

    void Update()
    {

    }

    void OpenOptions()
    {
        int childCount = G_optionsParent.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform _obj = G_optionsParent.transform.GetChild(i);
            OpenOption(_obj);
        }
    }

    void CloseOptions()
    {
        int childCount = G_optionsParent.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform _obj = G_optionsParent.transform.GetChild(i);
            CloseOption(_obj);
        }
    }

    void OpenOption(Transform _obj)
    {
        Utilities.Instance.ANIM_RotateObjWithCallback(_obj, 
            () => {
                Thumbnail6Controller.EnableOptionText(_obj.gameObject);
            }
        );
    }

    void CloseOption(Transform _obj)
    {
        Utilities.Instance.ANIM_RotateObjWithCallback(_obj, 
            () => {
                Thumbnail6Controller.EnableOptionBG(_obj.gameObject);
            }
        );
    }

    public static void EnableOptionText(GameObject optionObj)
    {
        optionObj.transform.GetChild(1).gameObject.SetActive(false);
        optionObj.transform.GetChild(0).gameObject.SetActive(true);
    }

    public static void EnableOptionBG(GameObject optionObj)
    {
        optionObj.transform.GetChild(0).gameObject.SetActive(false);
        optionObj.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void OnBGPanelClicked()
    {
        if(!B_canInteract) return;
        B_canInteract = false;
        var selectedObj = EventSystem.current.currentSelectedGameObject;

        if(selectedObj.CompareTag("answer"))
            return;

        if(_currentSelectedOption != null)
            if(_currentSelectedOption.name != selectedObj.name)
                CloseOption(_currentSelectedOption.transform);
            else if(_currentSelectedOption.name == selectedObj.name)
                return;

        OpenOption(selectedObj.transform);
        _currentSelectedOption = selectedObj;

        if(EvaluateAnswer(selectedObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text))
        {
            selectedObj.tag = "answer";
            _currentSelectedOption = null;
            _currentIndex++;
            Invoke(nameof(SpawnQuestion), 3.5f);
        }
    }

    bool EvaluateAnswer(string clickedOptString)
    {
        return SPR_questionSprite[_currentIndex].name.Contains(clickedOptString);
    }

    void SpawnQuestion()
    {
        _prevQuestion = _currentQuestion;
        MovePrevQuestion();
        var spawnedQuestion = Instantiate(G_questionImagePrefab, IMG_questionImagePanel.transform);
        spawnedQuestion.GetComponent<Image>().sprite = SPR_questionSprite[_currentIndex];
        spawnedQuestion.GetComponent<Image>().preserveAspect = true;
        spawnedQuestion.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        spawnedQuestion.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        _currentQuestion = spawnedQuestion;
        Utilities.Instance.ANIM_Move(spawnedQuestion.transform, T_startPoint.position, 0f, MoveQuestion);
        B_canInteract = false;
    }

    void EnableClicking() {B_canInteract = true;}
    void DisableClicking() {B_canInteract = false;}

    void MoveQuestion()
    {
        Utilities.Instance.ANIM_MoveWithScaleUp(_currentQuestion.transform, IMG_questionImagePanel.transform.position);
    }

    void MovePrevQuestion()
    {
        if(_prevQuestion == null) return;

        Utilities.Instance.ANIM_Move(_prevQuestion.transform, T_endPoint.position);
    }
}
