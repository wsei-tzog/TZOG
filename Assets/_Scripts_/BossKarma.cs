using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKarma : MonoBehaviour
{

    public GameObject gameObject1;
    public GameObject gameObject2;
    public GameObject mainBoss;


    public void useNPCKarma()
    {
        int npcHelped = NPCScript.npcHelped;
        // Turn on gameObjects based on npcHelped value
        if (npcHelped == 0)
        {
            gameObject1.SetActive(true);
            gameObject2.SetActive(true);
        }
        else if (npcHelped == 1)
        {
            gameObject1.SetActive(true);
            gameObject2.SetActive(false);
        }
        else if (npcHelped == 2)
        {
            gameObject1.SetActive(false);
            gameObject2.SetActive(false);
        }
    }


    public void useEnemyKarma()
    {
        float enemyKilled = NewEnemyAI.enemyKilled;
        float FieldOfView = NewEnemyAI.enemyKilled;
        float karmaFactor = (enemyKilled / 10);

        float baseAttackDamage = NewEnemyAI.attackDamage;
        float baseHealth = NewEnemyAI.Health;
        // float playerSpeed = Movement.speed;

        NewEnemyAI.attackDamage += karmaFactor;
        NewEnemyAI.Health += karmaFactor;

        // Movement.speed += karmaFactor;

    }
}