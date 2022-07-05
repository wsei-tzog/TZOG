using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public string questName;
    public string description;
    public Goal goal;
    public bool completed;
    public List<string> itemRewards;

    public virtual void Complete()
    {
        Debug.Log("Quest completed");
        EventController.QuestCompleted(this);
        GrantReward();
    }

    private void GrantReward()
    {
        Debug.Log("Granting reward for quest!");
        foreach (string item in itemRewards)
        {
            Debug.Log("Your reward: " + item);
        }

        Destroy(this);
    }
}
