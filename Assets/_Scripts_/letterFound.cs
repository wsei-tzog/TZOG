using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class letterFound : MonoBehaviour
{
    public lightOut lOut;
    public bool letterFounded;
    public Image letterUI;
    private void Awake()
    {
        letterFounded = false;
    }
    public void hideLetter()
    {
        letterUI.enabled = false;
    }
    public void showLetter()
    {

        if ((!letterUI.enabled))
        {
            letterUI.enabled = true;
            letterFounded = true;
            // add to letters
        }
        else
            letterUI.enabled = false;

        if (letterFounded)
            lOut.onAllLightOff();
    }
}
