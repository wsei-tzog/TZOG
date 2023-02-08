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
    public GameObject q3;
    public Image photoUI;
    private void Awake()
    {
        photoFounded = false;
        enemies.SetActive(false);
        this.transform.gameObject.GetComponent<Renderer>().material.SetFloat("_startClue", 1f);
        presentMission = true;

    }
    public void hidepQ2()
    {
        photoUI.enabled = false;
    }
    public void showpQ2()
    {
        if (presentMission)
        {
            questController.actuallMission = photoUI;
            q3.SetActive(true);
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
        enemies.SetActive(true);
    }
}
