using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thumbnail6Controller : MonoBehaviour
{
    public Image IMG_questionImage;
    public GameObject G_optionsParent;
    public static Thumbnail6Controller instance;
    delegate void DummyDelegate();
    DummyDelegate dummyDelegate;

    void Start()
    {
        if(instance != null) instance = this;
        OpenOptions();
        // AppendToDummyDelegate();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            dummyDelegate?.Invoke();
        }
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
    }

    public static void EnableOptionText(GameObject optionObj)
    {
        optionObj.transform.GetChild(1).gameObject.SetActive(false);
        optionObj.transform.GetChild(0).gameObject.SetActive(true);
    }

    void AppendToDummyDelegate()
    {
        int dummyVar = -1;
        for (int i = 0; i < 5; i++)
        {
            dummyDelegate += () => { Logging((dummyVar + i).ToString()); };
        }
        dummyVar = 80;
    }

    void Logging(string logMessage)
    {
        Debug.Log($"Log Message :: {logMessage}");
    }
}
