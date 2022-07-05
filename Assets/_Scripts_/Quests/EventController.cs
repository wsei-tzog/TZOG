using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public static event System.Action<int> OnEnemyDied = delegate { };
    public static event System.Action<Quest> OnQuestProgressChanged = delegate { };
    public static event System.Action<Quest> OnQuestCompleted = delegate { };


    public static void EnemyDied(int enemyId)
    {
        OnEnemyDied(enemyId);
    }
    public static void QuestProgressChanged(Quest quest)
    {

        OnQuestProgressChanged(quest);
    }
    public static void QuestCompleted(Quest quest)
    {
        OnQuestCompleted(quest);

    }

}
