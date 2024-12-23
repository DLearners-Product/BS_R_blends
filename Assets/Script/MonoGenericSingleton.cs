using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoGenericSingleton<T> : MonoBehaviour where T : MonoGenericSingleton<T>
{
    private static T instance;

    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Destroy duplicates
        }
        else
        {
            instance = GetComponent<T>(); // Get component of type T attached to the same GameObject
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
    }
}