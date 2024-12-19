using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class ImageDragandDrop : MonoBehaviour, IBeginDragHandler,IEndDragHandler,IDragHandler
{
    Vector2 mousePos;
    Camera mainCam;
    public CanvasGroup canvasGroup;
    public Vector3 currentPos;
    public Vector2 min, max;
    public Transform originalParent = null;
    public bool resetPositionOnDrop;

    public delegate void OnDragStartDelegate(GameObject dragObject);
    public delegate void OnDragDelegate(GameObject dragObject);
    public delegate void OnDragEndDelegate(GameObject dragObject);

    public static OnDragStartDelegate onDragStart;
    public static OnDragDelegate onDrag;
    public static OnDragEndDelegate onDragEnd;

    private void Awake()
    {
        mainCam = Camera.main;
        canvasGroup = GetComponent<CanvasGroup>();
        originalParent = this.transform.parent;
        min = GetComponent<RectTransform>().offsetMin;
        max = GetComponent<RectTransform>().offsetMax;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        onDragStart?.Invoke(eventData.pointerDrag);

        // this.transform.SetParent(originalParent.parent);
        canvasGroup.alpha = .5f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        onDrag?.Invoke(eventData.pointerDrag);

        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = mousePos;
        currentPos = mousePos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Debug.Log($"OnEndDragCalled");
        onDragEnd?.Invoke(eventData.pointerDrag);

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        // Debug.Log(gameObject.transform.parent.name, gameObject.transform.parent);
        if(originalParent.Equals(gameObject.transform.parent) && resetPositionOnDrop){
            ReturnToOriginalPos();
        }
    }

    public void ReturnToOriginalPos()
    {
        transform.SetParent(originalParent);
        GetComponent<RectTransform>().offsetMin = min;
        GetComponent<RectTransform>().offsetMax = max;
    }
}
