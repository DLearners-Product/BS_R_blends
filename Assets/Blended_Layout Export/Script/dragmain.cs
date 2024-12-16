using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class dragmain : MonoBehaviour
{
    public static dragmain OBJ_dragmain;
   // public GameObject[] GA_Questions;
    public int I_Anscount;
    public GameObject G_Final,G_Words;
    public GameObject[] GA_Options;
    int I_count;
   //public GameObject G_oldnext, G_final;
   // public AudioSource AS_crt, AS_wrg;

   
    public void Start()
    {
        OBJ_dragmain = this;
        // I_Qcount = -1;
        // G_oldnext.SetActive(false);
        G_Final.SetActive(false);
        
        showquestion();
    }
    public void showquestion()
    {

       
        if(this.name== "10story_time(Clone)")
        {
            for(int i=0;i< G_Words.transform.childCount;i++)
            {
                G_Words.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(false);
            }
            THIShowOption();
        }
        else
        {
            for (int i = 0; i < this.transform.GetChild(0).transform.childCount - 1; i++)
            {
                this.transform.GetChild(0).transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        
    }

    public void BUT_next()
    {
       if(I_count<GA_Options.Length-1)
        {
            I_count++;
            THIShowOption();
        }
    }
    public void BUT_Back()
    {
        if (I_count>0)
        {
            I_count--;
            THIShowOption();
        }
    }
    public void THIShowOption()
    {
        for (int i = 0; i < GA_Options.Length; i++)
        {
            GA_Options[i].SetActive(false);
        }
        GA_Options[I_count].SetActive(true);
    }
    public void THI_correct()
    {
        I_Anscount++;
       // AS_crt.Play();
        if (I_Anscount == 4)
        {
            Invoke("THI_wrg", 2f);
           
        }
    }
    public void THI_wrg()
    {
        G_Final.SetActive(true);
    }
    
   
   

    
}
