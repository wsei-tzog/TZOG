using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Health;
    public int Damage;
    EnemyLocomotion enemyLocomotion;
    void Start()
    {
        enemyLocomotion = this.GetComponent<EnemyLocomotion>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Health < 0)
        {
            Debug.Log("Health lower than 0");
            enemyLocomotion.enemyAlive(false);
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
    }
}
