using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public bool torchOn, torchEquipped;

    public AudioSource audioSource;
    public AudioClip clip;
    public GameObject torchLight;

    private void Awake()
    {
        turnOn();
    }

    public void OnTorchSwitchPressed()
    {
        if (torchEquipped && !torchOn)
        {
            turnOn();
        }
        else if (torchEquipped && torchOn)
        {
            turnOff();
        }
    }



    public void turnOn()
    {
        gameObject.SetActive(true);
        audioSource.Play();
        // torchLight.SetActive(true);
        torchOn = true;
    }
    public void turnOff()
    {
        gameObject.SetActive(false);
        //torchLight.SetActive(false);
        torchOn = false;
    }
}