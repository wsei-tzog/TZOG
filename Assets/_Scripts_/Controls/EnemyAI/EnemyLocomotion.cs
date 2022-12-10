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
    public float rotationSpeed;

    private void Start()
    {
        Alive = true;
        mTransform = this.transform;
        agent.speed = movementSpeed;
        agent.angularSpeed = rotationSpeed;
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
        else
        {
            Die();
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
            animator.SetFloat("locomotion", 0, 0.4f, Time.deltaTime);
            Debug.Log("player lost");
        }

        if (distanceToPlayer < attackDistance)
        {
            // attack player
            animator.SetFloat("locomotion", 1, 0.4f, Time.deltaTime);
            Debug.Log("attacking");
        }
        else
        {
            // go to player
            agent.SetDestination(playerTransform.position);
            animator.SetFloat("locomotion", 0.5f, 0.4f, Time.deltaTime);
            Debug.Log("going to");
        }


    }

    public void Die()
    {
        Debug.Log("Die function");
        animator.SetBool("Die", true);
        Destroy(this.gameObject, 2.5f);
    }

    public void enemyAlive(bool _Alive)
    {
        Debug.Log("Alive call");
        Alive = _Alive;
    }




}
