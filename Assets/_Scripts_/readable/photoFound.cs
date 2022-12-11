using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class photoFound : MonoBehaviour
{
    public QuestController questController;
    public bool photoFounded;
    bool presentMission;
    public GameObject enemies;
    public Image photoUI;
    private void Awake()
    {
        photoFounded = false;
        enemies.SetActive(false);
        this.transform.gameObject.GetComponent<Renderer>().material.SetFloat("_startClue", 1f);
        presentMission = true;

    }
    public void hidepQ3()
    {
        photoUI.enabled = false;
    }
    public void showpQ3()
    {
        if (presentMission)
        {
            questController.actuallMission = photoUI;
            presentMission = false;
        }

        if ((!photoUI.enabled))
        {
            photoUI.enabled = true;
            photoFounded = true;
            turnOnEnemies();
            // add to letters
        }
        else
            photoUI.enabled = false;

    }

    public void turnOnEnemies()
    {
        Debug.Log("Turning on enemies");
        enemies.SetActive(true);
    }
}
