using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Thumbnail3Controller : MonoBehaviour
{
    public GameObject astroid1, astroid2;
    public TextMeshProUGUI astroid1Text,
                            astroid2Text;
    public Image answerDisplayIMG;
    public TextMeshProUGUI answerDisplayText;
    public TextMeshProUGUI textObj1, textObj2;
    public string[] contents;
    public Sprite[] rBlendedImages;
    public AudioClip[] _blendWordClips;
    public AudioClip[] _contentClips;
    public GameObject revealObject;
    public Button nextBtn, revealBTN;
    public GameObject activityCompleted;
    int currentContentIndex = 0;

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

            Utilities.Instance.ANIM_ShowNormal(textObj1.transform);
            Utilities.Instance.ANIM_ShowNormal(textObj2.transform);
        });
        Utilities.Instance.ANIM_Move(astroid2.transform, astrd2EndPos, callBack: () => {
            astroid2.GetComponent<FloatingObject>().enabled = true;
        });

        var currentContentText = contents[currentContentIndex];
        string contentText = Regex.Replace(currentContentText, "<.*?>", string.Empty);
        astroid1Text.text = contentText.Substring(0, 1);
        astroid2Text.text = contentText.Substring(1, 1);
        answerDisplayText.text = currentContentText;
        answerDisplayIMG.sprite = rBlendedImages[currentContentIndex];
    }

    public void OnRevealBTNClick()
    {
        revealBTN.interactable = false;
        Vector3 endPos = revealObject.transform.position + (Vector3.up * 10f);
        revealObject.GetComponent<FloatingObject>().enabled = false;
        Utilities.Instance.ANIM_Move(revealObject.transform, endPos, callBack: () => {
            revealObject.GetComponent<FloatingObject>().enabled = true;
            nextBtn.interactable = true;
            AudioManager.PlayOnQueue( new AudioClip[]{_blendWordClips[currentContentIndex], _contentClips[currentContentIndex]});
        });
    }

    public void OnNextBTNClick()
    {
        ChangeContent();
        // SpawnContent();
        nextBtn.interactable = false;
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

        Vector3 astrd1EndPos = astroid1.transform.position + (Vector3.down * 10f);
        Vector3 astrd2EndPos = astroid2.transform.position + (Vector3.down * 10f);

        Utilities.Instance.ANIM_Move(astroid1.transform, astrd1EndPos);
        Utilities.Instance.ANIM_Move(astroid2.transform, astrd2EndPos, callBack: () => {
            Utilities.Instance.ANIM_ShrinkObject(textObj1.transform);
            Utilities.Instance.ANIM_ShrinkObject(textObj2.transform);
        });

        Invoke(nameof(SpawnContent), 2f);
    }

}