using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class letterFound : MonoBehaviour
{
    public QuestController questController;
    public bool letterFounded;
    bool presentMission;
    public Image letterUI;
    public GameObject enemies_q1;
    public GameObject q2;
    private void Awake()
    {
        letterFounded = false;
        this.transform.gameObject.GetComponent<Renderer>().material.SetFloat("_startClue", 1f);
        presentMission = true;
    }
    public void hideLetter()
    {
        letterUI.enabled = false;
    }
    public void showLetter()
    {

        if (presentMission)
        {
            questController.actuallMission = letterUI;
            q2.SetActive(true);
            presentMission = false;
        }


        if ((!letterUI.enabled))
        {
            letterUI.enabled = true;
            letterFounded = true;
            // add to letters
        }
        else
            letterUI.enabled = false;

        // if (letterFounded)
        // {
        //     enemies_q1.SetActive(true);
        // }

    }
}
