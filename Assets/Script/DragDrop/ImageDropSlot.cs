using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class ImageDropSlot: MonoBehaviour, IDropHandler
{
    ImageDragandDrop dragitem;
    public delegate void OnDropInSlotDelegate(GameObject droppedObject, GameObject dropSlotObject);
    public static OnDropInSlotDelegate onDropInSlot;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDropCalled...."+eventData.pointerDrag);
        dragitem = eventData.pointerDrag.GetComponent<ImageDragandDrop>();
        onDropInSlot.Invoke(eventData.pointerDrag, gameObject);
    }

    public void SetDropedObject(){
        dragitem.transform.SetParent(this.transform);
        dragitem.canvasGroup.alpha = 1f;
        dragitem.transform.localPosition = dragitem.currentPos;
        dragitem.enabled = false;
    }

    public void ResetDropedObjectPosition(){
        dragitem.ReturnToOriginalPos();
    }
}

