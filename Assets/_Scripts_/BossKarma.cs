using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKarma : MonoBehaviour
{

    public GameObject gameObject1;
    public GameObject gameObject2;
    public GameObject mainBoss;

    public int bossBaseHealth = 100;
    public int bossBaseDamage = 10;



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
        float baseMoveSpeed = NewEnemyAI.moveSpeed;
        float baseRunningSpeed = NewEnemyAI.runningSpeed;
        float baseAttackDamage = NewEnemyAI.attackDamage;
        float enemyKilled = NewEnemyAI.enemyKilled;
        float karmaFactor = (enemyKilled / 10);

        NewEnemyAI.moveSpeed += karmaFactor;
        NewEnemyAI.runningSpeed += karmaFactor;
        NewEnemyAI.attackDamage += karmaFactor;

    }
}