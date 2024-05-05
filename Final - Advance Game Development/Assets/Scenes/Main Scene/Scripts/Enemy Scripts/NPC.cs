using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum AIType
{
    Passive,
    Scared,
    Aggressive
}

public enum AIState
{
    Idle,
    Wandering,
    Attacking,
    Fleeing
}

public class NPC : MonoBehaviour, IDamagable
{
    [Header("Stats")]
    public int health;
    public float walkSpeed;
    public float runSpeed;
    public ItemDatabase[] dropOnDeath;

    [Header("AI")]
    public AIType aiType;
    private AIState aiState;
    public float detectDistance;
    public float safeDistance;

    [Header("Wandering")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime;
    public float maxWanderWaitTime;

    [Header("Combat")]
    public int damage;
    public float attackRate;
    private float lastAttackTime;
    public float attackDistance;

    [Header("Sound")]
    public AudioSource audioSource;

    // Components
    private NavMeshAgent agent;
    private Animator anim;
    private SkinnedMeshRenderer[] meshRenderers;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on the NPC game object.");
            // You might want to handle this situation appropriately, such as disabling the NPC or adding a NavMeshAgent component dynamically.
        }
        anim = GetComponentInChildren<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    private void Start()
    {
        SetState(AIState.Wandering);
    }

    private void Update()
    {
        float playerDistance = Vector3.Distance(transform.position, PlayerController.Instance.transform.position);
        anim.SetBool("Moving", aiState != AIState.Idle);

        switch (aiState)
        {
            case AIState.Idle:
            case AIState.Wandering:
                PassiveUpdate(playerDistance);
                break;
            case AIState.Attacking:
                AttackingUpdate(playerDistance);
                break;
            case AIState.Fleeing:
                FleeingUpdate(playerDistance);
                break;
        }
    }

    void PassiveUpdate(float playerDistance)
    {
        if (aiState == AIState.Wandering && agent.remainingDistance < 0.1f)
        {
            SetState(AIState.Idle);
            Invoke("WanderToNewLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime));
        }

        if (aiType == AIType.Aggressive && playerDistance < detectDistance)
        {
            SetState(AIState.Attacking);
        }
        else if (aiType == AIType.Scared && playerDistance < detectDistance)
        {
            SetState(AIState.Fleeing);
            agent.SetDestination(GetFleeLocation());
        }
    }

    void AttackingUpdate(float playerDistance)
    {
        if (playerDistance > attackDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(PlayerController.Instance.transform.position);
        }
        else
        {
            agent.isStopped = true;

            if (Time.time - lastAttackTime > attackRate)
            {
                lastAttackTime = Time.time;
                PlayerController.Instance.GetComponent<IDamagable>().TakeDamage(damage);
                anim.SetTrigger("Attack");
            }
        }
    }

    void FleeingUpdate(float playerDistance)
    {
        if (playerDistance < safeDistance && agent.remainingDistance < 0.1f)
        {
            agent.SetDestination(GetFleeLocation());
        }
        else if (playerDistance > safeDistance)
        {
            SetState(AIState.Wandering);
        }
    }

    void SetState(AIState newState)
    {
        aiState = newState;
        agent.speed = newState == AIState.Attacking || newState == AIState.Fleeing ? runSpeed : walkSpeed;
        agent.isStopped = newState == AIState.Idle || newState == AIState.Wandering;
    }

    void WanderToNewLocation()
    {
        if (aiState != AIState.Idle)
            return;

        SetState(AIState.Wandering);
        agent.SetDestination(GetWanderLocation());
    }

    Vector3 GetWanderLocation()
    {
        NavMeshHit hit;
        Vector3 randomDirection = Random.insideUnitSphere * Random.Range(minWanderDistance, maxWanderDistance);
        randomDirection += transform.position;
        NavMesh.SamplePosition(randomDirection, out hit, maxWanderDistance, NavMesh.AllAreas);

        int attempts = 0;
        while (attempts < 30 && Vector3.Distance(transform.position, hit.position) < detectDistance)
        {
            randomDirection = Random.insideUnitSphere * Random.Range(minWanderDistance, maxWanderDistance);
            randomDirection += transform.position;
            NavMesh.SamplePosition(randomDirection, out hit, maxWanderDistance, NavMesh.AllAreas);
            attempts++;
        }

        return hit.position;
    }

    Vector3 GetFleeLocation()
    {
        NavMeshHit hit;
        Vector3 randomDirection = Random.onUnitSphere * safeDistance;
        randomDirection += transform.position;

        int attempts = 0;
        while (attempts < 30 && Vector3.Angle(transform.position - PlayerController.Instance.transform.position, transform.position + randomDirection) > 90)
        {
            randomDirection = Random.onUnitSphere * safeDistance;
            randomDirection += transform.position;
            attempts++;
        }

        NavMesh.SamplePosition(randomDirection, out hit, safeDistance, NavMesh.AllAreas);
        return hit.position;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        StartCoroutine(DamageFlash());
        if (aiType == AIType.Passive)
            SetState(AIState.Fleeing);

        if (health <= 0)
            Die();
    }

    void Die()
    {
        foreach (var drop in dropOnDeath)
        {
            Instantiate(drop.dropPrefab, transform.position, Quaternion.identity);
        }
        anim.SetTrigger("Die");
        Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length);
    }

    IEnumerator DamageFlash()
    {
        audioSource.Play();
        foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.material.color = new Color(1.0f, 0.5f, 0.5f);
        }
        yield return new WaitForSeconds(0.1f);
        foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.material.color = Color.white;
        }
    }
}
