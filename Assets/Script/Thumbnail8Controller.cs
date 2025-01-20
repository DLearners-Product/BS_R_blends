using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thumbnail8Controller : MonoBehaviour
{
    public Transform textContainPanel;
    public GameObject nextBtn, prevBtn;

    void Start()
    {
        
    }

    public void OnNextBtnClick()
    {
        if(textContainPanel.GetChild(1).gameObject.activeSelf) return;

        Vector3 endPosition = textContainPanel.position + (Vector3.up * 10);
        Utilities.Instance.ANIM_Move(textContainPanel, endPosition, callBack: () => { Enable2ndPanel(1); });
    }

    public void OnPrevBtnClick()
    {
        if(textContainPanel.GetChild(0).gameObject.activeSelf) return;

        Vector3 endPosition = textContainPanel.position + (Vector3.up * 10);
        Utilities.Instance.ANIM_Move(textContainPanel, endPosition, callBack: () => { Enable2ndPanel(2); });
    }

    void Enable2ndPanel(int enableIndex)
    {
        if(enableIndex == 1)
        {
            textContainPanel.GetChild(0).gameObject.SetActive(false);
            textContainPanel.GetChild(1).gameObject.SetActive(true);
            nextBtn.SetActive(false);
            prevBtn.SetActive(true);
        }else{
            textContainPanel.GetChild(0).gameObject.SetActive(true);
            textContainPanel.GetChild(1).gameObject.SetActive(false);
            nextBtn.SetActive(true);
            prevBtn.SetActive(false);
        }

        Vector3 endPosition = textContainPanel.position + (Vector3.down * 10);
        Utilities.Instance.ANIM_Move(textContainPanel, endPosition);
    }
}
