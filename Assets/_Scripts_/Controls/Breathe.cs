using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breathe : MonoBehaviour
{
    Vector3 startPos;
    public float amplitude = 10f;
    public float peroid = 5f;
    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float theta = Time.timeSinceLevelLoad / peroid;
        float distance = amplitude * Mathf.Sin(theta);
        transform.position = startPos + Vector3.up * distance;
    }
}
