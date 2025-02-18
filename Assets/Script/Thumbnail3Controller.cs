using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

public class Thumbnail3Controller : MonoBehaviour
{
    public GameObject astroid1, astroid2;
    public TextMeshProUGUI astroid1Text,
                            astroid2Text;
    public Image answerDisplayIMG;
    public TextMeshProUGUI answerDisplayText;
    public TextMeshProUGUI textObj2;
    public string[] contents;
    public Sprite[] rBlendedImages;
    public AudioClip[] _blendWordClips;
    public AudioClip[] _contentClips;
    public AudioClip[] wordClips;
    public GameObject revealObject;
    public Button nextBtn, revealBTN;
    public GameObject activityCompleted;
    int currentContentIndex = 0;
    int playIndex = 0;

    void Start()
    {
        astroid1.GetComponent<FloatingObject>().enabled = false;
        astroid2.GetComponent<FloatingObject>().enabled = false;
        revealBTN.interactable = false;

        SpawnContent();
    }

    void SpawnContent()
    {
        Vector3 astrd1EndPos = astroid1.transform.position + (Vector3.up * 10f);
        Vector3 astrd2EndPos = astroid2.transform.position + (Vector3.up * 10f);

        Utilities.Instance.ANIM_Move(astroid1.transform, astrd1EndPos, callBack: () => {
            astroid1.GetComponent<FloatingObject>().enabled = true;
            revealBTN.interactable = true;

            // Utilities.Instance.ANIM_ShowNormal(textObj1.transform);
            Utilities.Instance.ANIM_ShowNormal(textObj2.transform);
        });
        Utilities.Instance.ANIM_Move(astroid2.transform, astrd2EndPos, callBack: () => {
            astroid2.GetComponent<FloatingObject>().enabled = true;
        });

        var currentContentText = contents[currentContentIndex];
        string contentText = RemoveTag(currentContentText);

        int colorCodeStartIndex = currentContentText.IndexOf("color=") + "color=".Length;
        string colorCode = currentContentText.Substring(colorCodeStartIndex, colorCodeStartIndex);

        astroid1Text.text = $"<color={colorCode}>{contentText.Substring(0, 1)}</color>";
        astroid2Text.text = $"<color={colorCode}>{contentText.Substring(1, 1)}</color>";
        answerDisplayText.text = currentContentText;
        answerDisplayIMG.sprite = rBlendedImages[currentContentIndex];
    }

    string RemoveTag(string text) { return Regex.Replace(text, "<.*?>", string.Empty); }

    public void OnRevealBTNClick()
    {
        playIndex = 0;
        revealBTN.interactable = false;
        Vector3 endPos = revealObject.transform.position + (Vector3.up * 10f);
        revealObject.GetComponent<FloatingObject>().enabled = false;
        PlayBlendWords();
    }

    void PlayBlendWords()
    {
        if(playIndex == 0){
            Utilities.Instance.ScaleObject(astroid1.transform.GetChild(0), duration: 0.5f, callback : () => {
                AudioClip voClip = GetWordAC(RemoveTag(astroid1Text.text));
                AudioManager.PlayAudio(voClip);
                playIndex++;
                Invoke(nameof(PlayBlendWords), voClip.length);
            });
        }else{
            Utilities.Instance.ScaleObject(astroid1.transform.GetChild(0), scaleSize: 1f, duration: 0.5f);
            Utilities.Instance.ScaleObject(astroid2.transform.GetChild(0), duration: 0.5f, callback : () => {
                AudioClip voClip = GetWordAC(RemoveTag(astroid2Text.text));
                AudioManager.PlayAudio(voClip);
                Invoke(nameof(ShrinkToNormal), voClip.length);
            });
        }
    }

    void ShrinkToNormal()
    {
        Utilities.Instance.ScaleObject(astroid2.transform.GetChild(0), scaleSize: 1f, duration: 0.5f, MoveAstroidTowards);
    }

    void MoveAstroidTowards()
    {
        astroid1.GetComponent<FloatingObject>().enabled = false;
        astroid2.GetComponent<FloatingObject>().enabled = false;

        Utilities.Instance.ANIM_Move(astroid1.transform, astroid1.transform.position + (Vector3.right * 1.5f));

        Utilities.Instance.ANIM_Move(astroid2.transform, astroid2.transform.position + (Vector3.left * 1.5f), callBack: PlayBlendedClip);
    }

    void PlayBlendedClip()
    {
        AudioManager.PlayAudio(_blendWordClips[currentContentIndex]);
        Invoke(nameof(RevealAnswer), _blendWordClips[currentContentIndex].length);
    }

    void RevealAnswer()
    {
        Vector3 endPos = revealObject.transform.position + (Vector3.up * 10f);

        Utilities.Instance.ANIM_Move(revealObject.transform, endPos, callBack: () => {
            revealObject.GetComponent<FloatingObject>().enabled = true;
            nextBtn.interactable = true;
            AudioManager.PlayAudio(_contentClips[currentContentIndex]);
        });
    }

    AudioClip GetWordAC(string strText)
    {
        foreach (var clip in wordClips)
        {
            if(clip.name.Contains(strText))
            {
                return clip;
            }
        }
        return null;
    }

    public void OnNextBTNClick()
    {
        ChangeContent();
        // SpawnContent();
        nextBtn.interactable = false;
    }

    public void OnAnswerClick()
    {
        AudioManager.PlayAudio(_contentClips[currentContentIndex]);
    }

    void ChangeContent()
    {
        currentContentIndex++;
        
        if(currentContentIndex == contents.Length) { activityCompleted.SetActive(true); return; }
        
        revealObject.GetComponent<FloatingObject>().enabled = false;
        Vector3 enPosition = revealObject.transform.position + (Vector3.down * 10f);
        Utilities.Instance.ANIM_Move(revealObject.transform, enPosition, callBack:MoveDownAstroids);
    }

    void MoveDownAstroids()
    {
        astroid1.GetComponent<FloatingObject>().enabled = false;
        astroid2.GetComponent<FloatingObject>().enabled = false;

        Vector3 astrd1EndPos = astroid1.transform.position + (Vector3.down * 10f) + (Vector3.left * 1.5f);
        Vector3 astrd2EndPos = astroid2.transform.position + (Vector3.down * 10f) + (Vector3.right * 1.5f);

        Utilities.Instance.ANIM_Move(astroid1.transform, astrd1EndPos);
        Utilities.Instance.ANIM_Move(astroid2.transform, astrd2EndPos, callBack: () => {
            // Utilities.Instance.ANIM_ShrinkObject(textObj1.transform);
            Utilities.Instance.ANIM_ShrinkObject(textObj2.transform);
        });

        Invoke(nameof(SpawnContent), 2f);
    }

}