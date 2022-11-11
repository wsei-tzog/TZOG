using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class muzzle_audio : MonoBehaviour
{
    // public AudioSource audioSource;
    public AudioClip clip;
    float volume;
    float pitch;
    void Start()
    {
        volume = Random.Range(0, 1);
        pitch = Random.Range(-3, 3);
        if (TryGetComponent(out AudioSource audioSource))
        {
            audioSource.pitch = pitch;
            audioSource.volume = volume;
            audioSource.clip = clip;
            audioSource.Play();

        }
        Destroy(gameObject, 4f);
    }

}
