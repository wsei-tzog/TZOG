using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class muzzle_audio : MonoBehaviour
{
    public AudioSource audioSource;
    void Start()
    {
        float volume = Random.Range(0.5f, 1);
        float pitch = Random.Range(0.8f, 1.5f);
        float dopplerLevel = Random.Range(0, 5);
        audioSource.pitch = pitch;
        audioSource.dopplerLevel = dopplerLevel;
        audioSource.PlayOneShot(audioSource.clip, volume);
        Destroy(gameObject, 4f);
    }


}
