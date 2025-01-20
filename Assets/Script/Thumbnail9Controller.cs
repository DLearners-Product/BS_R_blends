using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Thumbnail9Controller : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public GameObject answerObject;
    public string[] questionSTR;
    public string[] answerSTR;
    public GameObject[] optionObjects;
    int index = 0;
    Color intialColor;

    void Start()
    {
        questionText.text = questionSTR[index];
        intialColor = answerObject.GetComponent<Image>().color;
        ShrinkOptionObjects();
        SpawnOptions();
    }

    void OnEnable() {
        ImageDragandDrop.onDragStart += OnObjectDragStart;
        ImageDragandDrop.onDragEnd += OnObjectDragEnd;
        ImageDropSlot.onDropInSlot += OnObjectDrop;
    }

    void OnObjectDrop(GameObject dropObj)
    {
        string selectedOptionSTR = dropObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        if(answerSTR[index] == selectedOptionSTR)
        {
            answerObject.GetComponent<Image>().color = Color.white;
            answerObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = selectedOptionSTR;
            Destroy(dropObj);
            RightAnswer();
        }
    }

    void OnObjectDragStart(GameObject dragObject)
    {
        Utilities.Instance.ScaleObject(answerObject.transform, scaleSize: 1.25f, duration: 0.5f);
    }

    void OnObjectDragEnd(GameObject dragObject)
    {
        Utilities.Instance.ScaleObject(answerObject.transform, scaleSize: 1f, duration: 0.5f);
    }

    void RightAnswer()
    {
        MoveAnswerPanel(Vector3.up * 1.25f);
    }

    void ChangeQuestion()
    {
        index++;
        questionText.text = questionSTR[index];
    }

    void MoveAnswerPanel(Vector3 _position)
    {
        Vector3 endPos = answerObject.transform.position + _position;
        Utilities.Instance.ANIM_Move(answerObject.transform, endPos, callBack: () => { MoveQuestionPanel(Vector3.up * 1.25f); });
    }

    void MoveQuestionPanel(Vector3 _position)
    {
        Vector3 endPos = questionText.transform.parent.position + _position;
        Utilities.Instance.ANIM_Move(questionText.transform.parent, endPos, callBack: ChangeQuestion);
    }

    void ShrinkOptionObjects()
    {
        foreach (var opt in optionObjects)
        {
            Utilities.Instance.ANIM_ShrinkObject(opt.transform, 0f);
        }
    }

    void SpawnOptions(int index = 0)
    {
        if(index >= optionObjects.Length) return;

        Utilities.Instance.ANIM_ShowBounceNormal(optionObjects[index].transform, shrinkUpTime: 0.25f, callback: () => {
            SpawnOptions(++index);
        });
    }

}
