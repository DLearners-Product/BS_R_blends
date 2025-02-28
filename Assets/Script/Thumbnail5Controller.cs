using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Thumbnail5Controller : MonoBehaviour
{
    public TextMeshProUGUI blendWordTextPanel;
    public TextMeshProUGUI frame1Text, frame2Text;
    public Image frame1, frame2;
    public Button nxtBtn, backBtn;
    public GameObject activityCompleted;
    public string[] blendWords;
    public Sprite[] blendSprites;
    public AudioClip[] blendImageVOs;
    int currentIndex = 0;
    List<int> clickedFrame = new List<int>();
    int clickTime = 0;

    void Start()
    {
        ShowNextFrame();
        backBtn.interactable = false;
        // Invoke(nameof(ShowNextFrame), 2f);
    }

    string GetColorCodedText(int assetIndex)
    {
        string assetName = blendSprites[assetIndex].name;
        string colorCode = GetColorCode();

        string colorCodedStr = $"<color={colorCode}>{assetName.Substring(0,2)}</color>";
        assetName = colorCodedStr + assetName.Substring(2);
        // Debug.Log(assetName);
        return assetName;
    }

    string GetColorCode()
    {
        int colorCodeStartIndex = blendWords[currentIndex].IndexOf("color=") + "color=".Length;
        return blendWords[currentIndex].Substring(colorCodeStartIndex, colorCodeStartIndex);
    }

    void ChangeContent()
    {
        blendWordTextPanel.text = blendWords[currentIndex];
        frame1.GetComponent<Image>().sprite = blendSprites[currentIndex * 2];
        frame1.GetComponent<Image>().preserveAspect = true;
        frame1.GetComponent<AudioSource>().clip = blendImageVOs[currentIndex * 2];
        frame1Text.text = GetColorCodedText(currentIndex * 2);

        frame2.GetComponent<Image>().sprite = blendSprites[(currentIndex * 2) + 1];
        frame2.GetComponent<Image>().preserveAspect = true;
        frame2.GetComponent<AudioSource>().clip = blendImageVOs[(currentIndex * 2) + 1];
        frame2Text.text = GetColorCodedText((currentIndex * 2) + 1);
    }

    void ShowNextFrame()
    {
        nxtBtn.interactable = false;
        Utilities.Instance.ANIM_RotateObjWithCallback(blendWordTextPanel.transform, ChangeContent);
        Utilities.Instance.ANIM_RotateObjWithCallback(frame1.transform.parent);
        Utilities.Instance.ANIM_RotateObjWithCallback(frame2.transform.parent);
    }

    public void OnSpeakerClick(int frameIndex)
    {
        clickTime++;
        Transform frameObj = (frameIndex == 1) ? frame1.transform : frame2.transform;
        float clipLen = frameObj.GetComponent<AudioSource>().clip.length;
        Utilities.Instance.ANIM_ScaleUpDelayScaleDown(frameObj, clipLen);

        if(!clickedFrame.Contains(frameIndex)) clickedFrame.Add(frameIndex);

        if(clickedFrame.Count == 2)
        {
            clickedFrame.Clear();
            nxtBtn.interactable = true;
        }
    }

    public void OnBackBTNClick()
    {
        currentIndex--;

        backBtn.interactable = (currentIndex != 0);

        ShowNextFrame();
    }

    public void OnNextBtnClick()
    {
        currentIndex++;

        if(currentIndex > 0) backBtn.interactable = true;

        if (currentIndex == blendWords.Length)
        {
            activityCompleted.SetActive(true);
            return;
        }

        ShowNextFrame();
    }
}
