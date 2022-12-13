using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscController : MonoBehaviour
{
    public GameObject inGameMenu;
    public List<MonoBehaviour> Scripts;


    private void Awake()
    {
        inGameMenu.SetActive(false);
        foreach (var s in Scripts)
        {
            s.enabled = true;
        }
    }

    // public void OnTabPressed()
    // {
    //     foreach (var s in Scripts)
    //     {
    //         s.enabled = true;
    //     }
    // }

    public void OnEscPressed()
    {


        if (inGameMenu.activeInHierarchy == true)
        {
            foreach (var s in Scripts)
            {
                s.enabled = true;
            }
            inGameMenu.SetActive(false);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            foreach (var s in Scripts)
            {
                s.enabled = false;
            }
            inGameMenu.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }


}
