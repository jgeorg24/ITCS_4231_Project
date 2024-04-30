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
    
    public float attackRange = 2f;
    public float attackDamage = 10f;
    public float attackCooldown = 1f;
    public float knockbackForce = 10f;
    private float lastAttackTime;
    private bool isAttacking;

    private enum EnemyState
    {
        Patrol,
        Chase,
        Attack
    }

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("Target not assigned to EnemyStates.");
            return;
        }

        currentState = EnemyState.Patrol;
        playerUI = PlayerUI.Instance;

        if (playerUI == null)
        {
            Debug.LogError("PlayerUI instance not found.");
            return;
        }

        InvokeRepeating(nameof(UpdateEnemyState), 0f, 1f);
    }

    private void UpdateEnemyState()
    {
        float distanceToTarget; // Declare distanceToTarget outside of the if statement
        if (currentState == EnemyState.Attack || isAttacking)
        {
            distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget > attackRange)
            {
                currentState = EnemyState.Chase; // Transition to Chase state if out of attack range
                StopAttack(); // Stop attacking
            }
            return;
        }

        distanceToTarget = Vector3.Distance(transform.position, target.position); // Assign distanceToTarget here

        if (distanceToTarget <= attackRange)
        {
            currentState = EnemyState.Attack;
            StartCoroutine(AttackPlayer());
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

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrolling();
                break;
            case EnemyState.Chase:
                ChaseTarget();
                break;
        }
    }

    private void ChaseTarget()
    {
        if (agent != null)
            agent.SetDestination(target.position);
    }

    private void Patrolling()
    {
        if (agent == null)
            return;

        if (!agent.hasPath || agent.remainingDistance < 0.5f)
        {
            Vector3 randomPatrolPoint = UnityEngine.Random.insideUnitSphere * patrolDistance;
            NavMesh.SamplePosition(randomPatrolPoint, out NavMeshHit hit, patrolDistance, 1);
            agent.SetDestination(hit.position);
        }
    }

    private System.Collections.IEnumerator AttackPlayer()
    {
        isAttacking = true;
        bool hasDamagedPlayer = false;
        
        while (currentState == EnemyState.Attack)
        {
            if (!hasDamagedPlayer && Time.time - lastAttackTime > attackCooldown)
            {
                DamagePlayer(attackDamage);
                lastAttackTime = Time.time;
                hasDamagedPlayer = true;
            }
            yield return null;
        }
        
        isAttacking = false;
    }

private void DamagePlayer(float damageAmount)
{
    playerUI.TakeDamage((int)damageAmount);

    // Calculate knockback direction (upwards and backwards from player to enemy)
    Vector3 knockbackDirection = (target.position - transform.position).normalized;
    knockbackDirection.y = 0; // Ignore vertical component
    knockbackDirection.Normalize(); // Ensure unit length for diagonal knockback
    
    // Apply knockback force to the player
    Rigidbody playerRigidbody = target.GetComponent<Rigidbody>();
    if (playerRigidbody != null)
    {
        playerRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse); // Backwards force
        playerRigidbody.AddForce(Vector3.up * knockbackForce, ForceMode.Impulse); // Upwards force
    }
}

    private void StopAttack()
    {
        currentState = EnemyState.Chase; // Transition to Chase state
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, patrolDistance);
    }
}