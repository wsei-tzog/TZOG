using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLocomotion : MonoBehaviour
{

    public bool detectedPlayer;
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
        mTransform = this.transform;
        agent.speed = movementSpeed;
        agent.angularSpeed = rotationSpeed;
    }


    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(mTransform.position, playerTransform.position);

        if (detectedPlayer)
        {
            MoveToPlayer(distanceToPlayer);
        }
        else
        {
            PlayerDetecion(distanceToPlayer);
        }
    }


    public void PlayerDetecion(float distanceToPlayer)
    {
        if (distanceToPlayer < detectDistance)
        {
            detectedPlayer = true;
        }


    }

    public void MoveToPlayer(float distanceToPlayer)
    {
        // lost player
        if (distanceToPlayer > detectDistance)
        {
            detectedPlayer = false;
            animator.SetFloat("walk", 0, 0.4f, Time.deltaTime);
        }

        // go to player
        agent.SetDestination(playerTransform.position);
        animator.SetFloat("walk", 1, 0.4f, Time.deltaTime);

        // attack player
        if (distanceToPlayer > attackDistance)
        {
            animator.SetFloat("attack", 1, 0.4f, Time.deltaTime);
        }

    }






}
