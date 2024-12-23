using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Thumbnail5Controller : MonoBehaviour
{
    public TextMeshProUGUI blendWordTextPanel;
    public Image frame1, frame2;
    public string[] blendWords;
    public Sprite[] blendSprites;
    public AudioClip[] blendImageVOs;
    int currentIndex =0;

    void Start()
    {
        Invoke(nameof(NextFrame), 2f);
    }

    void Update()
    {
        
    }

    void ChangeContent()
    {
        blendWordTextPanel.text = blendWords[currentIndex];
        frame1.GetComponent<Image>().sprite = blendSprites[currentIndex * 2];
        frame1.GetComponent<Image>().preserveAspect = true;
        frame2.GetComponent<Image>().sprite = blendSprites[(currentIndex * 2) + 1];
        frame2.GetComponent<Image>().preserveAspect = true;
    }

    void NextFrame()
    {
        Utilities.Instance.ANIM_RotateObj(blendWordTextPanel.transform);
        Utilities.Instance.ANIM_RotateObj(frame1.transform.parent);
        Utilities.Instance.ANIM_RotateObj(frame2.transform.parent);
    }
}
