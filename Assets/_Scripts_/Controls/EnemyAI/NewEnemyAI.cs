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

    // The distance at which the enemy will attack the player
    public float attackRange = 1.5f;

    // The amount of damage the enemy does when it attacks
    public int attackDamage = 10;

    // The rate at which the enemy can attack
    public float attackRate = 1f;

    // The enemy's field of view angle (in degrees)
    public float fieldOfViewAngle = 110f;

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

    private void Start()
    {
        // Get reference to Animator and NavMeshAgent components
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // Calculate distance to target
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // If the player is within the detection range and within the enemy's field of view
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
                animator.SetFloat("locomotion", 1f, 0.4f, Time.deltaTime);
            }
        }
        // If the player is outside the lose sight range
        else if (distanceToTarget > loseSightRange)
        {
            if (patrolPoints.Count != 0)
            {

                // Check if the enemy has reached its current patrol point
                if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                {
                    Debug.Log("Somee patrol points, walking " + this.name);
                    animator.SetFloat("locomotion", 1f, 0.4f, Time.deltaTime);
                    // Increment the current patrol point index
                    currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Count;

                    // Set the enemy's destination to the next patrol point
                    navMeshAgent.SetDestination(patrolPoints[currentPatrolPointIndex].position);
                }
            }
            else
            {
                Debug.Log("No patrol points, idle " + this.name);
                animator.SetFloat("locomotion", 0f, 0.4f, Time.deltaTime);
            }

        }
    }

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
            animator.SetFloat("locomotion", 0f);
            animator.SetBool("Attack", true);
            Debug.Log("Attacking");
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
}
//The `Update` method checks whether the player is within the enemy's detection range and field of view, and if so, sets the enemy's destination to the player's position and sets the enemy's animation state to "walking". If the player is within the attack range, the enemy will attack the player. If the player is outside the lose sight range, the enemy will reset its destination and set its animation state to "idle". The `IsInFieldOfView` method calculates the angle between the enemy's forward direction and the direction to the player, and returns true if the angle is within the enemy's field of view. The `Attack` method applies damage to the player and has a timer to ensure that the enemy can only attack at the specified attack rate.
