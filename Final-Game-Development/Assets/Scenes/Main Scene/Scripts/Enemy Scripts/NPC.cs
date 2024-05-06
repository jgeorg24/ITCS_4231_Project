using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public enum AIType
{
    Passive,    // The NPC won't actively engage in combat.
    Scared,     // The NPC will flee when the player is nearby.
    Aggressive  // The NPC will attack the player when the player is nearby.
}

public enum AIState
{
    Idle,       // NPC is not moving or performing any action.
    Wandering,  // NPC is randomly moving around.
    Attacking,  // NPC is actively attacking the player.
    Fleeing     // NPC is fleeing from the player.
}

public class NPC : MonoBehaviour, IDamagable
{
    public NpcBar health;

    [Header("Stats")]
    public float walkSpeed;         // Speed at which NPC walks.
    public float runSpeed;          // Speed at which NPC runs.

    [Header("AI")]
    public AIType aiType;           // Type of AI behavior.
    private AIState aiState;        // Current state of the NPC.
    public float detectDistance;    // Distance at which NPC detects the player.
    public float safeDistance;      // Distance at which NPC feels safe from the player.

    [Header("Wandering")]
    public float minWanderDistance; // Minimum distance NPC wanders.
    public float maxWanderDistance; // Maximum distance NPC wanders.
    public float minWanderWaitTime; // Minimum time NPC waits before wandering again.
    public float maxWanderWaitTime; // Maximum time NPC waits before wandering again.

    [Header("Combat")]
    public int damage;              // Damage NPC deals.
    public float attackRate;        // Rate at which NPC attacks.
    private float lastAttackTime;   // Time when NPC last attacked.
    public float attackDistance;    // Distance at which NPC attacks the player.

    [Header("Sound")]
    public AudioSource audioSource;

    private NavMeshAgent agent;
    public Animator anim;
    private SkinnedMeshRenderer[] meshRenderers;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on the NPC game object.");
        }
        anim = GetComponentInChildren<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    private void Start()
    {
        SetState(AIState.Wandering);
        health.currentValue = health.startValue;
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

        health.uiBar.value = health.GetPercentage();
        health.counter.text = ((int)health.currentValue).ToString() + "/" + health.maxValue.ToString();
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
        health.Subtract(amount);

        StartCoroutine(DamageFlash());
        if (aiType == AIType.Passive)
            SetState(AIState.Fleeing);

        if (health.currentValue <= 0)
            Die();
    }

    void Die()
    {
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

[System.Serializable]
public class NpcBar
{
    [HideInInspector]
    public float currentValue;
    public float maxValue;
    public float startValue;
    public Slider uiBar;
    public TextMeshProUGUI counter;

    public void Add(float amount)
    {
        currentValue = Mathf.Min(currentValue + amount, maxValue);
    }

    public void Subtract(float amount)
    {
        currentValue = Mathf.Max(currentValue - amount, 0);
    }

    public float GetPercentage()
    {
        return currentValue / maxValue;
    }
}
