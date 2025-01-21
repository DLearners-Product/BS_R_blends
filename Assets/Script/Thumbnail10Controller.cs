using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Thumbnail10Controller : MonoBehaviour
{
    public GameObject questionPanel;
    Transform _questionPanelIntialPosition;
    public GameObject optionObject;
    public Transform optionInitialisePosition;
    public string[] optionText;
    public Transform[] placementPositions;

    void Start()
    {
        _questionPanelIntialPosition = questionPanel.transform;
        MoveQuestionBoardUp(7f);
    }

    void OnEnable() {
        ImageDropSlot.onDropInSlot += OnObjectDrop;
    }

    private void OnDisable() {
        ImageDropSlot.onDropInSlot -= OnObjectDrop;
    }

    void OnObjectDrop(GameObject dropedObj)
    {
        
    }

    void MoveQuestionBoardUp(float moveDistance)
    {
        Vector3 endPosition = _questionPanelIntialPosition.position + (Vector3.up * moveDistance);

        Utilities.Instance.ANIM_MoveAndReturnToOriginalPos(questionPanel.transform, endPosition, destReachTime: 0f, _callbackOnEnd: SpawnOptions);
    }

    void SpawnOptions()
    {
        for (int i = 0; i < optionText.Length; i++)
        {
            var spawnedObj = Instantiate(optionObject, optionInitialisePosition);
            spawnedObj.SetActive(true);
            spawnedObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = optionText[i];
            Utilities.Instance.ANIM_Move(spawnedObj.transform, placementPositions[i].position);
        }
    }

}
