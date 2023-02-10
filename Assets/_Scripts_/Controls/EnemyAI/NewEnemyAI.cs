using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewEnemyAI : MonoBehaviour
{
    // The target transform (i.e. the player)
    public Transform target;

    // The range at which the enemy can see the player
    public float detectionRange = 10f;

    // The range at which the enemy will stop chasing the player
    public float loseSightRange = 15f;

    // The speed at which the enemy moves towards the player
    public float moveSpeed = 3f;
    public float runningSpeed = 5f;

    // The distance at which the enemy will attack the player
    public float attackRange = 1.5f;

    // The amount of damage the enemy does when it attacks
    public int attackDamage = 10;

    // The rate at which the enemy can attack
    public float attackRate = 1f;

    // The enemy's field of view angle (in degrees)
    public float fieldOfViewAngle = 200f;

    // A list of transforms representing the enemy's patrol points
    public List<Transform> patrolPoints;

    // A reference to the enemy's Animator component
    private Animator animator;

    // A reference to the enemy's NavMeshAgent component
    private NavMeshAgent navMeshAgent;

    // A timer for keeping track of the enemy's attack rate
    private float attackTimer;

    // The current patrol point index
    private int currentPatrolPointIndex = 0;

    public bool Alive;
    public bool Alerted;
    public int Health;

    private void Start()
    {
        // Get reference to Animator and NavMeshAgent components
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        Alive = true;
        Alerted = false;
    }

    private void Update()
    {
        if (Alive)
        {
            // Calculate distance to target
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            // If the player is within the detection range and within the enemy's field of view
            if (distanceToTarget < detectionRange && IsInFieldOfView())
            {
                Alerted = true;
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
                    animator.SetBool("Attack", false);
                    // Set the enemy's animation state to "running"
                    navMeshAgent.speed = runningSpeed;
                    animator.SetFloat("locomotion", 2f);
                }
            }
            // If the player is outside the lose sight range
            else if (distanceToTarget > loseSightRange)
            {
                Alerted = false;
                navMeshAgent.speed = moveSpeed;

                if (patrolPoints.Count != 0)
                {

                    animator.SetFloat("locomotion", 1f, 0.4f, Time.deltaTime);
                    // Check if the enemy has reached its current patrol point
                    if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                    {
                        // Increment the current patrol point index
                        currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Count;

                        // Set the enemy's destination to the next patrol point
                        navMeshAgent.SetDestination(patrolPoints[currentPatrolPointIndex].position);
                    }
                }
                else
                {
                    animator.SetFloat("locomotion", 0f, 0.4f, Time.deltaTime);
                }

            }
        }
    }

    private bool IsInFieldOfView()
    {
        // Calculate the angle between the enemy's forward direction and the direction to the player
        float angleToTarget = Vector3.Angle(transform.forward, target.position - transform.position);
        // Check if there is an obstacle blocking the line of sight
        RaycastHit hit;
        if (Physics.Linecast(transform.position, target.position, out hit))
        {
            // Return false if the line of sight is blocked by an object
            if (hit.collider.CompareTag("Player"))
            {
                // Return true if the angle is within the enemy's field of view
                return angleToTarget < fieldOfViewAngle * 0.5f;
            }
            else
                return false;
        }

        return angleToTarget < fieldOfViewAngle * 0.5f;
    }

    public void Stun()
    {
        if (!animator.GetBool("Attack") && !Alerted)
        {
            Alive = false;
            navMeshAgent.isStopped = true;
            animator.SetBool("Die", true);
        }
    }
    private void Attack()
    {
        // If the attack timer has reached the attack rate
        if (attackTimer >= attackRate)
        {
            // Apply damage to the player
            // target.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
            animator.SetFloat("locomotion", 0f);
            animator.SetBool("Attack", true);
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

    public void Die()
    {
        Alive = false;
        navMeshAgent.isStopped = true;
        animator.SetBool("Die", true);
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health < 0)
        {
            Die();
        }
    }


}
//The `Update` method checks whether the player is within the enemy's detection range and field of view, and if so, sets the enemy's destination to the player's position and sets the enemy's animation state to "walking". If the player is within the attack range, the enemy will attack the player. If the player is outside the lose sight range, the enemy will reset its destination and set its animation state to "idle". The `IsInFieldOfView` method calculates the angle between the enemy's forward direction and the direction to the player, and returns true if the angle is within the enemy's field of view. The `Attack` method applies damage to the player and has a timer to ensure that the enemy can only attack at the specified attack rate.
