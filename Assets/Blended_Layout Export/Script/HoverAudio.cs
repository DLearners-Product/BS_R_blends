using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class HoverAudio : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    public AudioSource audioSource;
    public AudioClip clip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.transform.localScale = new Vector3(0.99343f, 0.99343f, 0);
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.transform.localScale = new Vector3(0.69343f, 0.69343f, 0);
        audioSource.Stop();
    }
}
