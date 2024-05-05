using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System;
using static UnityEngine.GraphicsBuffer;

public class EnemyStates : MonoBehaviour
{
    // Public changeable variables
    public Transform target;
    public NavMeshAgent agent;
    public float patrolDistance;
    public float chaseDistance;
    public float maxDistance;
    public float sphereSize;

    // Private unchangeable variables
    private EnemyState currentState;
    private int damageEnemyOne = 15;
    private bool isPlayerInRange = false;
    private float cooldownAttack = 0f;

    /*public float attackRange = 2f;
    public float attackDamage = 10f;
    public float attackCooldown = 1f;
    public float knockbackForce = 10f;
    private float lastAttackTime;
    private bool isAttacking;*/

    // States of the enemy AI
    private enum EnemyState
    {
        Patrol,
        Chase,
        Attack
    }

    // Start is called before the first frame update
    void Start()
    {
        if (target != null)
        {
            target = target.transform;
            currentState = EnemyState.Patrol;

            // Check player distance and update state every second
            InvokeRepeating("UpdateEnemyState", 0f, 1f);
        }
        else if (target == null)
        {
            Debug.LogError("Target not assigned to EnemyStates.");

            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Checking to see if the target is null
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
        }

        // Decreasing the attack cooldown
        cooldownAttack -= Time.deltaTime;

        // Cooldown for following player and attacking
        if (cooldownAttack <= 0)
        {
            agent.isStopped = false;
        }

        // Depending on the state of the enemy it'll patrol, chase, or attack
        if (currentState == EnemyState.Patrol)
        {
            Patrolling();
        }
        else if (currentState == EnemyState.Chase)
        {
            ChaseTarget();
        }
        else if (currentState == EnemyState.Attack)
        {
            AttackTarget(damageEnemyOne);
        }
    }

    void UpdateEnemyState() 
    {
        // Calculate distance between the enemy and the player
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // Check if the player is within the chasing distance
        if (currentState != EnemyState.Attack && isPlayerInRange)
        {
            currentState = EnemyState.Attack;
        }
        else if (distanceToTarget <= chaseDistance)
        {
            currentState = EnemyState.Chase;
        }
        else
        {
            currentState = EnemyState.Patrol;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (cooldownAttack <= 0)
        {
            if (collision.collider.CompareTag("Player"))
            {
                isPlayerInRange = true;
            }
            else
            {
                isPlayerInRange = false;
            }
        }
    }

    void ChaseTarget()
    {
        // Has the agent chase the specified target
        agent.SetDestination(target.position);
    }

    void AttackTarget(int damage)
    {
        // Find the player game object
        GameObject player = GameObject.FindWithTag("Player");

        // Checks to see if player is found
        if (player != null)
        {
            // Sends the UI to take damage
            PlayerUI playerUIComponent = player.GetComponent<PlayerUI>();

            if (playerUIComponent != null && cooldownAttack <= 0)
            {
                playerUIComponent.TakeDamage(damage);
                agent.isStopped = true;
                isPlayerInRange = false;
                cooldownAttack = 1f;
            }
        }
        else
        {
            Debug.LogWarning("PlayerUI component not found on the player GameObject");
        }
    }

    void Patrolling() 
    {
        // Generate random destination within patrol area
        Vector3 randomPatrolPoint = UnityEngine.Random.insideUnitSphere * patrolDistance;
        randomPatrolPoint += transform.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomPatrolPoint, out hit, patrolDistance, 1);

        // Set patrol destination
        agent.SetDestination(hit.position);
    }
}
