using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewEnemyAI : MonoBehaviour
{

    [Header("Karma")]
    public BossKarma bossKarma;
    public static float moveSpeed = 3f;
    [SerializeField]
    public static float runningSpeed = 5f;
    [SerializeField]
    public static float attackDamage = 10;
    [SerializeField]
    public static int enemyKilled = 0;

    [Header("Sound")]
    #region setup
    public AudioSource audioSource;
    public SoundManager soundManager;
    public List<GameObject> enemyBlood = new List<GameObject>();
    private float waitTimer = 3f;
    public bool checkingNoise;
    // The target transform (i.e. the player)
    public Transform target;

    // The range at which the enemy can see the player
    public float detectionRange = 10f;

    // The range at which the enemy will stop chasing the player
    public float loseSightRange = 15f;

    public float attackRange = 1.5f;
    private float attackTimer;
    public float attackRate = 1f;
    public float fieldOfViewAngle = 200f;
    public List<Transform> patrolPoints;
    private int currentPatrolPointIndex = 0;
    private Animator animator;
    private NavMeshAgent navMeshAgent;

    public bool Alive;
    public bool Alerted;
    public int Health;
    #endregion
    private void Awake()
    {
        gameObject.tag = "Enemy";
        soundManager = FindObjectOfType<SoundManager>();
        bossKarma = FindObjectOfType<BossKarma>();

        // Try to get the existing AudioSource component
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            // If an AudioSource component does not exist, add one and assign it to the variable
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        // Set the playOnAwake property to false
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1;
    }

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

            // If the player is within the detection range and the enemy's field of view
            if (distanceToTarget < detectionRange && IsInFieldOfView() && Alive)
            {
                Alerted = true;

                // Set the enemy's destination to the player's position
                navMeshAgent.SetDestination(target.position);
                animator.SetBool("Attack", false);
                navMeshAgent.speed = runningSpeed;
                animator.SetFloat("locomotion", 2f);
                PlaySound(SoundType.Run);

                // If the player is within the attack range
                if (distanceToTarget < attackRange && Alive)
                {
                    Attack();
                }
                // else
                // {
                //     animator.SetBool("Attack", false);
                //     navMeshAgent.speed = runningSpeed;
                //     animator.SetFloat("locomotion", 2f);
                //     PlaySound(SoundType.Run);
                // }
            }
            else
            {
                // If the player is outside the lose sight range
                // else if (distanceToTarget > loseSightRange)
                Alerted = false;
                navMeshAgent.speed = moveSpeed;
            }

            if (!Alerted)
            {
                if (checkingNoise)
                {
                    if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && Alive)
                    {
                        StartCoroutine(WaitAtDestination());
                    }
                    else
                    {
                        PlaySound(SoundType.Walk);
                    }
                }
                else if (patrolPoints.Count != 0 && !checkingNoise && Alive)
                {
                    animator.SetFloat("locomotion", 1f, 0.4f, Time.deltaTime);
                    PlaySound(SoundType.Walk);
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
            if (hit.collider.CompareTag("Player") && Alive)
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
        if (!animator.GetBool("Attack") && !Alerted && Alive)
        {
            Alive = false;
            animator.SetBool("Die", true);
            PlaySound(SoundType.Stun);
            navMeshAgent.SetDestination(this.transform.position);

            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<NewEnemyAI>().enabled = false;

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
            PlaySound(SoundType.MeeleAttack);
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

    public void CheckNoise(Vector3 noiseSourcePosition)
    {
        if (Alive)
        {
            Debug.Log("Ide sobie sprawdzic " + gameObject.name);
            animator.SetFloat("locomotion", 1f);
            navMeshAgent.SetDestination(noiseSourcePosition);
            checkingNoise = true;
        }
    }

    private IEnumerator WaitAtDestination()
    {
        var timePassed = 0f;
        animator.SetFloat("locomotion", 0f);

        while (timePassed < waitTimer)
        {
            var factor = timePassed / waitTimer;
            navMeshAgent.isStopped = true;
            // increae by the time passed since last frame
            timePassed += Time.deltaTime;

            yield return null;
        }

        navMeshAgent.isStopped = false;
        checkingNoise = false;
    }

    // public Vector3 bloodOffset;
    public Quaternion bloodRotation;
    public void Die()
    {
        if (Alive)
        {
            Alive = false;
            animator.SetBool("Die", true);
            navMeshAgent.SetDestination(this.transform.position);
            navMeshAgent.isStopped = true;
            bloodRotation.x = 0;
            bloodRotation.y = 90;
            bloodRotation.z = 0;
            int enemyBloodType = Random.Range(0, enemyBlood.Count);
            Instantiate(enemyBlood[enemyBloodType], transform.position, bloodRotation);
            enemyKilled++;
            PlaySound(SoundType.Die);
            bossKarma.useEnemyKarma();

            GetComponent<Collider>().enabled = false;
            GetComponentInChildren<Collider>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<NewEnemyAI>().enabled = false;
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
        PlaySound(SoundType.TookDamage);
    }

    private void PlaySound(SoundType soundType)
    {
        soundManager.PlaySound(audioSource, soundType);
    }
}
