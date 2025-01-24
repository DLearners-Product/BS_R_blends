using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    Queue<AudioClip> queueClips;
    AudioSource audioSource;

    public static void PlayAudio(AudioClip audioClip)
    {
        GameObject audioManager = new GameObject();
        audioManager.AddComponent<AudioSource>().clip = audioClip;
        audioManager.AddComponent<AudioManager>();
        audioManager.GetComponent<AudioManager>().PlayAssignedAudio();
    }

    public static void PlayOnQueue(IEnumerable<AudioClip> clips)
    {
        GameObject audioManager = new GameObject();
        audioManager.AddComponent<AudioSource>();
        audioManager.AddComponent<AudioManager>().PlayClipOnQueue(clips);
    }

    public void PlayClipOnQueue(IEnumerable<AudioClip> clips)
    {
        audioSource = GetComponent<AudioSource>();
        queueClips = new Queue<AudioClip>(clips);
        PlayAllClips();
    }

    void PlayAllClips()
    {
        audioSource.clip = queueClips.Dequeue();
        audioSource.Play();
        if(queueClips.Count > 0)
        {
            Invoke(nameof(PlayAllClips), audioSource.clip.length);
        }else{
            Invoke(nameof(DestroyObj), gameObject.GetComponent<AudioSource>().clip.length);
        }
    }

    public void PlayAssignedAudio()
    {
        audioSource = GetComponent<AudioSource>();
        gameObject.GetComponent<AudioSource>().Play();
        Invoke(nameof(DestroyObj), gameObject.GetComponent<AudioSource>().clip.length);
    }

    void DestroyObj() {
        Destroy(gameObject);
    }
}
