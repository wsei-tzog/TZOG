using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLocomotion : MonoBehaviour
{

    /// <summary>
    /// NEW AI
    /// </summary>

    // The target transform (i.e. the player)
    public Transform target;

    // The range at which the enemy can see the player
    public float detectionRange = 10f;

    // The range at which the enemy will stop chasing the player
    public float loseSightRange = 15f;

    // The speed at which the enemy moves towards the player
    public float moveSpeed = 3f;

    // The distance at which the enemy will attack the player
    public float attackRange = 1.5f;

    // The amount of damage the enemy does when it attacks
    public int attackDamage = 10;

    // The rate at which the enemy can attack
    public float attackRate = 1f;

    // The enemy's field of view angle (in degrees)
    public float fieldOfViewAngle = 110f;

    // A reference to the enemy's Animator component
    private Animator animator;

    // A reference to the enemy's NavMeshAgent component
    private NavMeshAgent navMeshAgent;

    // A timer for keeping track of the enemy's attack rate
    private float attackTimer;


    /// <summary>
    /// Patrol
    /// </summary>
    public Transform[] patrolPoints;
    private int currentPoint;
    public float rotationSpeed;
    // public float fieldOfViewAngle;
    public bool playerInSight;
    public Vector3 personalLastSighting;
    private SphereCollider col;















    public bool detectedPlayer, Alive;
    public bool isMoving;


    // public Animator animator;
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
        currentPoint = 0;
        transform.position = patrolPoints[currentPoint].position;
        col = GetComponent<SphereCollider>();


        // Get reference to Animator and NavMeshAgent components
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();


        Alive = true;
        mTransform = this.transform;
    }

    private void Update()
    {
        // Calculate distance to target
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        float distanceToPlayer = Vector3.Distance(mTransform.position, playerTransform.position);



        if (Alive)
        {
            if (distanceToTarget < detectionRange && IsInFieldOfView())
            {
                // Set the enemy's destination to the player's position
                navMeshAgent.SetDestination(target.position);

                // If the player is within the attack range
                if (distanceToTarget < attackRange)
                {
                    // Attack the player
                    Attack();
                }
                // If the player is outside the attack range
                else
                {
                    // Set the enemy's animation state to "walking"
                    animator.SetBool("IsWalking", true);
                }
            }
            // If the player is outside the lose sight range
            else if (distanceToTarget > loseSightRange)
            {
                // Reset the enemy's destination
                navMeshAgent.SetDestination(transform.position);

                // Set the enemy's animation state to "idle"
                animator.SetBool("IsWalking", false);
            }


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

    // method for patroling
    // method when player spotted

    private bool IsInFieldOfView()
    {
        // Calculate the angle between the enemy's forward direction and the direction to the player
        float angleToTarget = Vector3.Angle(transform.forward, target.position - transform.position);

        // Return true if the angle is within the enemy's field of view
        return angleToTarget < fieldOfViewAngle * 0.5f;
    }


    private void Attack()
    {
        // If the attack timer has reached the attack rate
        if (attackTimer >= attackRate)
        {
            // Apply damage to the player
            // target.GetComponent<PlayerHealth>().TakeDamage(attackDamage);

            // Reset the attack timer
            attackTimer = 0f;
        }
        // If the attack timer has not yet reached the attack rate
        else
        {
            // Increment the attack timer
            attackTimer += Time.deltaTime;
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