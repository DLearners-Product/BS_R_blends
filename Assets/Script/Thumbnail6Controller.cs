using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    int _currentIndex = 0;

    void Start()
    {
        OpenOptions();
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
            Utilities.Instance.ANIM_RotateObjWithCallback(_obj, 
                () => {
                    Thumbnail6Controller.EnableOptionText(_obj.gameObject);
                }
            );
        }
        SpawnQuestion();
    }

    void CloseOptions()
    {
        int childCount = G_optionsParent.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform _obj = G_optionsParent.transform.GetChild(i);
            Utilities.Instance.ANIM_RotateObjWithCallback(_obj, 
                () => {
                    Thumbnail6Controller.EnableOptionBG(_obj.gameObject);
                }
            );
        }
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
    }

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
