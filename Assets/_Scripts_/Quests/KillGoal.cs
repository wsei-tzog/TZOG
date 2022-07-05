using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillGoal : Goal
{
    public int enemyId;

    public KillGoal(int amountNeeded, int enemyId, Quest quest)
    {
        countCurrent = 0;
        countNeeded = amountNeeded;
        completed = false;

        this.quest = quest;
        this.enemyId = enemyId;
        EventController.OnEnemyDied += enemyKilled;
    }

    void enemyKilled(int enemyId)
    {
        if (this.enemyId == enemyId)
        {
            Increment(1);
        }
    }
}
