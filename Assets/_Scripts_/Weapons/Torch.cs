using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public bool torchOn, torchEquipped;

    #region polish
    public AudioSource audioSource;
    public AudioClip clip;
    public GameObject torchLight;
    #endregion

    #region input
    public void OnTorchPressed()
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
    #endregion

    public void turnOn()
    {
        gameObject.SetActive(true);
        torchLight.SetActive(true);
        torchOn = true;
    }
    public void turnOff()
    {
        gameObject.SetActive(false);
        //torchLight.SetActive(false);
        torchOn = false;
    }
}