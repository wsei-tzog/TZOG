using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public bool torchOn, torchEquipped;

    public AudioSource audioSource;
    public AudioClip clip;
    public GameObject torchLight;

    public float startingPower = 100f;
    public float powerDrainRate = 5f;
    public float rechargeAmount = 20f;

    private float currentPower;
    private bool isActive;

    private void Awake()
    {
        // turnOn();
        Activate();
    }
    private void Start()
    {
        currentPower = startingPower;
    }

    private void Update()
    {
        if (isActive)
        {
            float lossFactor = Mathf.Clamp01(currentPower / startingPower);
            currentPower -= powerDrainRate * lossFactor * Time.deltaTime;

            if (currentPower <= 0f)
            {
                Deactivate();
            }

            // Adjust the light's intensity based on the torch's current power level
            float lightIntensity = currentPower / startingPower;
            torchLight.GetComponent<Light>().intensity = lightIntensity;
        }
    }

    public void Recharge(float amount)
    {
        currentPower = Mathf.Min(currentPower + amount, startingPower);
    }

    public float GetPowerLevel()
    {
        return currentPower;
    }

    public void OnTorchSwitchPressed()
    {
        if (torchEquipped && !gameObject.activeInHierarchy)
        {
            Activate();
            // turnOn();
        }
        else if (torchEquipped && gameObject.activeInHierarchy)
        {
            // turnOff();
            Deactivate();
        }
    }
    public void Activate()
    {
        isActive = true;
        gameObject.SetActive(true);
        audioSource.Play();
    }

    public void Deactivate()
    {
        isActive = false;
        gameObject.SetActive(false);
        audioSource.Play();
    }


    // public void turnOn()
    // {
    //     gameObject.SetActive(true);
    //     audioSource.Play();
    //     // torchLight.SetActive(true);
    //     torchOn = true;
    // }
    // public void turnOff()
    // {
    //     gameObject.SetActive(false);
    //     //torchLight.SetActive(false);
    //     torchOn = false;
    // }
}