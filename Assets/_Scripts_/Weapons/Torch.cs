using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public static bool torchOn, torchEquipped;

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
        torchLight.SetActive(true);
    }
    public void turnOff()
    {
        torchLight.SetActive(false);
    }
}