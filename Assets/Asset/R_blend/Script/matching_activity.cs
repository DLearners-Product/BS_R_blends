using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class matching_activity : MonoBehaviour
{
    public GameObject[] GA_Questions;
    int I_Qcount;
    GameObject G_Selected;
    bool B_CanClick;
    public GameObject G_Final;
    public Text TXT_Max, TXT_Current;
    public AudioSource AS_crt, AS_wrg;
    // Start is called before the first frame update
    void Start()
    {
        I_Qcount = 0;
        THI_ShowQuestion();
        TXT_Max.text = GA_Questions.Length.ToString();
        G_Final.SetActive(false);
    }
    public void THI_ShowQuestion()
    {
        for(int i=0;i<GA_Questions.Length;i++)
        {
            GA_Questions[i].SetActive(false);
        }
        GA_Questions[I_Qcount].SetActive(true);
        GA_Questions[I_Qcount].transform.GetChild(1).gameObject.SetActive(true);
        GA_Questions[I_Qcount].transform.GetChild(2).gameObject.SetActive(false);
         
        int count = I_Qcount + 1;
        TXT_Current.text = count.ToString();
        B_CanClick = true;
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
        if (I_Qcount > 0)
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
        if(B_CanClick)
        {
            B_CanClick = false;
            G_Selected = EventSystem.current.currentSelectedGameObject;
            if (G_Selected.tag == "answer")
            {
                AS_crt.Play();
                G_Selected.transform.GetChild(0).GetComponent<Image>().color = Color.green;
                GA_Questions[I_Qcount].transform.GetChild(1).gameObject.SetActive(false);
                GA_Questions[I_Qcount].transform.GetChild(2).gameObject.SetActive(true);
            }
            else
            {
                AS_wrg.Play();
                G_Selected.transform.GetChild(0).GetComponent<Image>().color = Color.red;
                Invoke("Offred", 1f);
            }
        }
        
    }
    public void Offred()
    {
        B_CanClick = true;
        G_Selected.transform.GetChild(0).GetComponent<Image>().color = Color.grey;
    }
}
