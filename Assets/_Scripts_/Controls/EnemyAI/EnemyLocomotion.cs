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
        if (!detectedPlayer)
        {
            PlayerDetecion();
        }
        else
        {
            MoveToPlayer();
        }
    }


    public void PlayerDetecion()
    {
        float distanceToPlayer = Vector3.Distance(mTransform.position, playerTransform.position);
        if (distanceToPlayer < detectDistance)
        {
            detectedPlayer = true;
        }
    }

    public void MoveToPlayer()
    {
        agent.SetDestination(playerTransform.position);
    }






}
