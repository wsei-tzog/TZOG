using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class muzzle_audio : MonoBehaviour
{
    public AudioSource audioSource;
    float volume;
    void Start()
    {
        audioSource.PlayOneShot(audioSource.clip, volume);
        Destroy(gameObject, 4f);
    }

    private void Update()
    {
        volume = Random.Range(0, 1);
    }


}
