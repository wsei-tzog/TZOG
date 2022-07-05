using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSlayer : Quest
{
    void Awake()
    {
        questName = "Ghost Slayer";
        description = "Slay some ghosts";
        itemRewards = new List<string>() { "Some new skill", "Maybe some karma points" };
        goal = new KillGoal(5, 0, this);
    }

    public override void Complete()
    {
        base.Complete();
        // can add or override something else when completed
    }
}
