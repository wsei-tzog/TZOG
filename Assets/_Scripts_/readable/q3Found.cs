using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class q3Found : MonoBehaviour
{
    public QuestController questController;
    public bool q3Founded;
    bool presentMission;
    public Image q3UI;
    public GameObject enemies_q3;
    public GameObject q4;
    private void Awake()
    {
        q3Founded = false;
        this.transform.gameObject.GetComponent<Renderer>().material.SetFloat("_startClue", 1f);
        presentMission = true;
        enemies_q3.SetActive(false);
    }
    public void hideq3()
    {
        q3UI.enabled = false;
    }
    public void showq3()
    {
        presentMission = true;

        if (presentMission)
        {
            questController.actuallMission = q3UI;
            q4.SetActive(true);
            presentMission = false;
        }


        if ((!q3UI.enabled))
        {
            q3UI.enabled = true;
            q3Founded = true;
            turnOnEnemies();
            // add to letters
        }
        else
            q3UI.enabled = false;

    }

    public void turnOnEnemies()
    {
        enemies_q3.SetActive(true);
    }
}