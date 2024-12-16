using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class introduction : MonoBehaviour
{
    public GameObject[] GA_Question;
    int I_Qcount,I_count;
    public GameObject G_Final;

    // Start is called before the first frame update
    void Start()
    {
        G_Final.SetActive(false);
        THI_ShowQuestion();
    }
    public void THI_ShowQuestion()
    {
        for(int i=0;i<GA_Question.Length;i++)
        {
            GA_Question[i].SetActive(false);
        }
        GA_Question[I_Qcount].SetActive(true);
        I_count = 0;
        for(int i=0;i<GA_Question[I_Qcount].transform.childCount;i++)
        {
            GA_Question[I_Qcount].transform.GetChild(i).gameObject.SetActive(false);
        }
        GA_Question[I_Qcount].transform.GetChild(I_count).gameObject.SetActive(true);
    }
    
    public void BUT_Next()
    {
       // if(I_Qcount<GA_Question.Length-1)
       // {
            if(I_count< GA_Question[I_Qcount].transform.childCount-1)
            {
                I_count++;
                GA_Question[I_Qcount].transform.GetChild(I_count).gameObject.SetActive(true);
                if (I_count == 2)
                {
                    for (int i = 0; i < GA_Question[I_Qcount].transform.childCount; i++)
                    {
                        GA_Question[I_Qcount].transform.GetChild(i).gameObject.SetActive(false);
                    }
                    GA_Question[I_Qcount].transform.GetChild(I_count).gameObject.SetActive(true);
                }
            }
            else
            if (I_Qcount < GA_Question.Length - 1)
            {
                I_Qcount++;
                THI_ShowQuestion();
            } else
            {
                G_Final.SetActive(true);
            }
        
    }
}
