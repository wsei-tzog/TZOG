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
    private void Awake()
    {
        q3Founded = false;
        this.transform.gameObject.GetComponent<Renderer>().material.SetFloat("_startClue", 1f);
        presentMission = true;
    }
    public void hideq3()
    {
        q3UI.enabled = false;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            presentMission = true;

            if (presentMission)
            {
                questController.actuallMission = q3UI;
                presentMission = false;
            }


            // if ((!q3UI.enabled))
            // {
            //     q3UI.enabled = true;
            //     q3Founded = true;
            // }
            // else
            //     q3UI.enabled = false;
        }

    }

}