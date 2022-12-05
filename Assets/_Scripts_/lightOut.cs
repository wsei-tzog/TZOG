using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightOut : MonoBehaviour
{

    public bool lightOff;

    public void onAllLightOff()
    {
        gameObject.SetActive(false);
    }

}
