using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class images_intro : MonoBehaviour
{
    public GameObject[] GA_Question;
    int I_Qcount;
    public GameObject G_Final;
    // Start is called before the first frame update
    void Start()
    {
        G_Final.SetActive(false);
        THI_ShowQuestion();
    }
    public void THI_ShowQuestion()
    {
        for (int i = 0; i < GA_Question.Length; i++)
        {
            GA_Question[i].SetActive(false);
        }
        GA_Question[I_Qcount].SetActive(true);
    }

    public void BUT_Next()
    {
        if(I_Qcount<GA_Question.Length-1)
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
}
