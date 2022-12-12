using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyEnv : MonoBehaviour
{
    public float healt;
    public float duration;
    public Quaternion targetRotation;

    public void destroyObject(int damange)
    {
        healt -= damange;

        if (healt <= 0)
        {
            // RotateOverTime(this.transform, Quaternion.Euler(270, 0, 0), 3);
            StartCoroutine(RotateOverTime(this.transform, targetRotation, duration));
        }
    }

    private IEnumerator RotateOverTime(Transform transformToRotate, Quaternion targetRotation, float duration)
    {
        this.GetComponent<Collider>().enabled = false;

        var startRotation = transformToRotate.localRotation;

        var timePassed = 0f;
        while (timePassed < duration)
        {
            var factor = timePassed / duration;
            // optional add ease-in and -out
            // factor = Mathf.SmoothStep(0, 1, factor);
            factor = 1f - Mathf.Cos(factor * Mathf.PI * 0.5f);

            transformToRotate.localRotation = Quaternion.Lerp(startRotation, targetRotation, factor);
            // or
            //transformToRotate.rotation = Quaternion.Slerp(startRotation, targetRotation, factor);

            // increae by the time passed since last frame
            timePassed += Time.deltaTime;

            // important! This tells Unity to interrupt here, render this frame
            // and continue from here in the next frame
            yield return null;
        }

        // to be sure to end with exact values set the target rotation fix when done
        transformToRotate.localRotation = targetRotation;
    }
}