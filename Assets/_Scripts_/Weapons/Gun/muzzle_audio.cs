using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class muzzle_audio : MonoBehaviour
{
    public AudioSource audioSource;
    void Start()
    {
        float volume = Random.Range(0, 1);
        audioSource.PlayOneShot(audioSource.clip, volume);
        Destroy(gameObject, 2f);
    }


}
