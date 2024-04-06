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
    private PlayerUI playerUI;
    private int damageEnemyOne = 15;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
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
        if (distanceToTarget <= chaseDistance)
        {
            currentState = EnemyState.Chase;
        }
        else if (currentState != EnemyState.Attack && IsPlayerInRange())
        {
            currentState = EnemyState.Attack;
        }
        else
        {
            currentState = EnemyState.Patrol;
        }
    }

    bool IsPlayerInRange()
    {
        // Calculate direction to the player
        Vector3 direction = (target.position - transform.position).normalized;

        // Perform a spherecast to check for collision with the player
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, sphereSize, direction, out hit, maxDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("YES");
                return true;
            }
        }

        Debug.DrawLine(transform.position, transform.position + direction * maxDistance, Color.green);

        return false;
    }

    void ChaseTarget()
    {
        // Has the agent chase the specified target
        agent.SetDestination(target.position);
    }

    void AttackTarget(int damage)
    {
        // Do damage to the target
        playerUI.TakeDamage((int) damage);
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
