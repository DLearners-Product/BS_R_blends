using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HighlightTextOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    Camera mainCam;
    TextMeshProUGUI _text;
    bool pointerEntered;
    string displayText;

    private void Start() {
        pointerEntered = false;
        _text = GetComponent<TextMeshProUGUI>();
        displayText = _text.text;
        mainCam = (mainCam == null) ? Camera.main : mainCam;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerEntered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerEntered = false;
        _text.text = displayText;
    }

    private void Update() {
        if(!pointerEntered) return;

        var wordIndex = TMP_TextUtilities.FindIntersectingWord(_text, Input.mousePosition, mainCam);
        Debug.Log($"{wordIndex} :: {mainCam}");

        if (wordIndex != -1)
        {
            var clickedText = _text.textInfo.wordInfo[wordIndex].GetWord();
            string[] splitTexts = displayText.Split(' ');
            splitTexts[wordIndex] = $"<b>{splitTexts[wordIndex]}</b>";
            _text.text = string.Join(" ", splitTexts);
        }
    }
}
