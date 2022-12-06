using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscController : MonoBehaviour
{
    public GameObject inGameMenu;
    public void OnEscPressed()
    {
        if (inGameMenu.activeInHierarchy == true)
        {
            inGameMenu.SetActive(false);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            inGameMenu.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
