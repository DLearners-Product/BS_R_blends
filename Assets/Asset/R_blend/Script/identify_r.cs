using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class identify_r : MonoBehaviour
{
    public GameObject[] GA_Questions;
    public int I_Qcount;
    public GameObject G_Final;
   /* public TextMeshProUGUI TMP_Text;
    public string[] STRA_Before, STRA_After;
    public string STR_Clicked;*/
    // Start is called before the first frame update
    void Start()
    {
        I_Qcount = 0;
        THI_ShowQuestion();
        G_Final.SetActive(false);
    }
    public void THI_ShowQuestion()
    {
        for (int i = 0; i < GA_Questions.Length; i++)
        {
            GA_Questions[i].SetActive(false);
        }
        GA_Questions[I_Qcount].SetActive(true);
        GA_Questions[I_Qcount].transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
    }
    public void BUT_Next()
    {
        if(I_Qcount<GA_Questions.Length-1)
        {
            I_Qcount++;
            THI_ShowQuestion();
        }
        else
        {
            G_Final.SetActive(true);
        }
        
    }
    public void BUT_Back()
    {
        if (I_Qcount >0)
        {
            I_Qcount--;
            THI_ShowQuestion();
        }
        else
        {
            G_Final.SetActive(true);
        }

    }
    public void BUT_Clicking()
    {
        GameObject G_Selected = EventSystem.current.currentSelectedGameObject;
        G_Selected.transform.GetChild(0).gameObject.SetActive(true);
    }

   /* public void THI_Seperate_TMP()
    {
        STRA_Before = new string[0];
        STRA_Before = TMP_Text.text.ToString().Split(' ');
        STRA_After = new string[0];
        STRA_After = TMP_Text.text.ToString().Split(' ');

        for (int i = 0; i < STRA_After.Length; i++)
        {
            STRA_After[i] = "<link =" + STRA_After[i] + ">" + STRA_After[i] + "</link>";
        }
        TMP_Text.text = string.Join(" ", STRA_After);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       
        STR_Clicked = TMP_Text.textInfo.linkInfo[TMP_TextUtilities.FindIntersectingLink(TMP_Text, Input.mousePosition, Camera.main)].GetLinkText();
        Debug.Log("Selected =" + STR_Clicked);
        for(int i=0;i<STRA_After.Length;i++)
        {
            if(STRA_Before[i]==STR_Clicked)
            {
                STRA_After[i] = "<link =" + STRA_Before[i] + "><color=red>" + STRA_Before[i] + "</color>";
            }
        }

        for (int i = 0; i < STRA_After.Length; i++)
        {
            if (STRA_Before[i] == STR_Clicked)
            {
                if (STR_Clicked.IndexOf(1) == r)
                {
                    STRA_After[i] = "<link =" + STRA_wordsBefore[i] + "><color=" + red + ">"STRA_Before[i].IndexOf(1))+"</color> ";
                 }
            }
        }
              
    }*/
}
