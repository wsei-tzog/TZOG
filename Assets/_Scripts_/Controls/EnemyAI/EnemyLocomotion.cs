using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLocomotion : MonoBehaviour
{

    public bool detectedPlayer, Alive;
    public bool isMoving;


    public Animator animator;
    public NavMeshAgent agent;
    public Transform playerTransform;
    Transform mTransform;


    public float detectDistance;
    public float attackDistance;
    public float movementSpeed;
    public int Health;
    public int Damage;

    private void Start()
    {

        Alive = true;
        mTransform = this.transform;
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(mTransform.position, playerTransform.position);

        if (Alive)
        {
            if (detectedPlayer)
            {
                MoveToPlayer(distanceToPlayer);

            }
            else
            {
                PlayerDetecion(distanceToPlayer);
            }
        }

    }

    public void PlayerDetecion(float distanceToPlayer)
    {
        if (distanceToPlayer < detectDistance)
        {
            detectedPlayer = true;
        }
        else
        {
            detectedPlayer = false;
            animator.SetFloat("locomotion", 0, 0.4f, Time.deltaTime);
        }

    }

    public void MoveToPlayer(float distanceToPlayer)
    {
        // lost player
        if (distanceToPlayer > detectDistance)
        {
            detectedPlayer = false;
            animator.SetFloat("locomotion", 0f, 0.4f, Time.deltaTime);
            animator.SetBool("Attack", false);

        }
        else
        {
            if (distanceToPlayer < attackDistance)
            {
                agent.isStopped = true;
                animator.SetFloat("locomotion", 0f);

                animator.SetBool("Attack", true);
                Debug.Log("attack dist " + attackDistance);
            }
            else
            {
                agent.isStopped = false;
                agent.SetDestination(playerTransform.position);
                animator.SetBool("Attack", false);
                animator.SetFloat("locomotion", 1f, 0.4f, Time.deltaTime);
            }
        }


    }

    public void Die()
    {
        Alive = false;
        agent.isStopped = true;
        animator.SetBool("Die", true);
        Destroy(this.gameObject, 1.8f);
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health < 0)
        {
            Debug.Log("Health lower than 0");
            Die();
        }
    }
}