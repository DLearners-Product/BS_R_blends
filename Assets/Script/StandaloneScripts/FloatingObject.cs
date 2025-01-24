using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{

    [Header("Floating Settings")]
    public float floatSpeed = 2f;
    public float floatAmplitude = 10f;

    private RectTransform rectTransform;
    private Vector3 initialPosition;
    float randomOffset;

    void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();

        initialPosition = rectTransform.anchoredPosition;
        randomOffset = Random.Range(0f, 2f * Mathf.PI);
    }

    void Update()
    {
        float newY = initialPosition.y + Mathf.Sin(Time.time * floatSpeed * randomOffset) * floatAmplitude;
        rectTransform.anchoredPosition = new Vector3(initialPosition.x, newY, initialPosition.z);
    }
}