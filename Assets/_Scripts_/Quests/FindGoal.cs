using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindGoal : Goal
{
    public int itemId;

    public FindGoal(int amountNeeded, int itemId, Quest quest)
    {
        countCurrent = 0;
        countNeeded = amountNeeded;
        completed = false;

        this.quest = quest;
        this.itemId = itemId;
        EventController.OnItemFound += itemFound;
    }

    void itemFound(int itemId)
    {
        if (this.itemId == itemId)
        {
            Increment(1);
        }
    }
}
