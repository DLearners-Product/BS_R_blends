using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class phonological : MonoBehaviour
{
    public GameObject[] GA_Question,GA_words;
    public int I_count,I_Qcount;
    public AudioSource AS_Wrong;
    public GameObject G_Final;
    // Start is called before the first frame update
    void Start()
    {
        I_count = 0;
        THI_ShowQuestion();
        G_Final.SetActive(false);
        for (int i=0;i<GA_words.Length;i++)
        {
            GA_words[i].SetActive(false);
        }
    }
    public void THI_ShowQuestion()
    {
        for(int i=0;i<GA_Question.Length;i++)
        {
            GA_Question[i].SetActive(false);
        }
        GA_Question[I_Qcount].SetActive(true);
    }
    public void BUT_Deletion()
    {
        GameObject G_Selected = EventSystem.current.currentSelectedGameObject;
        I_count++;
        if (I_count%2 != 0)
        {
            G_Selected.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            G_Selected.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void BUT_Next()
    {
        if (I_Qcount < GA_Question.Length - 1)
        {
            I_Qcount++;
            THI_ShowQuestion();
        }
        else
        {
            G_Final.SetActive(true);
        }
    }
    public void BUT_Selecting()
    {
        GameObject G_Selected = EventSystem.current.currentSelectedGameObject;
        if(G_Selected.name== "dr")
        {
            for (int i = 0; i < GA_words.Length; i++)
            {
                GA_words[i].SetActive(false);
            }
            GA_words[1].SetActive(true);
        }
        else
        if (G_Selected.name == "br")
        {
            for (int i = 0; i < GA_words.Length; i++)
            {
                GA_words[i].SetActive(false);
            }
            GA_words[0].SetActive(true);
        }
        else
        {
            for (int i = 0; i < GA_words.Length; i++)
            {
                GA_words[i].SetActive(false);
            }
            AS_Wrong.Play();
        }
    }
}
