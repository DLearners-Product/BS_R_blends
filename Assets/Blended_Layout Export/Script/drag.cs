using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class drag : MonoBehaviour, IDragHandler, IEndDragHandler
{
    Vector2 mousePos;
    public Vector2 initalPos;

    bool isdrag;
    GameObject otherGameObject;

    Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Start()
    {
        //initalPos = this.GetComponent<RectTransform>().position;
        initalPos = this.transform.position;
    }


    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = mousePos;

        Debug.Log("Drag");
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag");
        if(otherGameObject!=null)
        {
            if (this.gameObject.name == otherGameObject.name)
            {

                dragmain.OBJ_dragmain.THI_correct();
                otherGameObject.gameObject.transform.GetChild(1).gameObject.SetActive(true);
                this.gameObject.SetActive(false);
                this.GetComponent<drag>().enabled = false;
            }
            else
            {
               // lb_Activity.OBJ_lb_Activity.Wrg();
                this.transform.position = initalPos;
            }
        }else
        {
            this.transform.position = initalPos;
        }
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.parent.name == "Q1")
            otherGameObject = other.gameObject;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.parent.name == "Q1")
            otherGameObject = null;

    }

}
