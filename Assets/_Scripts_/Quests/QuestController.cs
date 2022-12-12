using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuestController : MonoBehaviour
{
    // public string actuallMission;
    public EscController escController;
    public GameObject UI;
    public Image controls;
    public Image actuallMission;
    public Image missionZero;
    void Awake()
    {
        controls.enabled = true;
        missionZero.enabled = true;
        actuallMission = missionZero;

        escController.OnTabPressed();
    }

    // public void takeNewMission(string actuallMission)
    // {
    //     Image newMission = GetComponentInChildren(typeof(Image)) as Image;

    //     if (newMission.name == actuallMission)
    //     {
    //         mission = newMission;
    //     }
    // }
    public void OnMissionPressed()
    {
        if (actuallMission.enabled)
        {
            actuallMission.enabled = false;
            controls.enabled = false;
        }
        else
        {
            actuallMission.enabled = true;
            controls.enabled = true;
        }
    }
}
