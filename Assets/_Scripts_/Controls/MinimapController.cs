using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    bool displayMap;
    public GameObject minimap;
    public void OnMapPressed()
    {
        if (minimap.activeInHierarchy)
            minimap.SetActive(false);
        else
            minimap.SetActive(true);
    }
    void Start()
    {
        minimap.SetActive(false);
    }



}
