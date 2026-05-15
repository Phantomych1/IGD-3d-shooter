using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float chaseRange = 50f;
    public float attackRange = 2f;
    public float damage = 10f;
    public float attackCooldown = 1.5f;

    private Animator anim;
    private float lastAttackTime;
    private PlayerHealth playerHealth;
    private NavMeshAgent agent;

    [System.Obsolete]
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            player = playerHealth.transform;
        }
    }

    void Update()
    {
        if (player == null) return;
        if (agent == null || !agent.isActiveAndEnabled || !agent.isOnNavMesh) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
        else if (distanceToPlayer <= chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            Idle();
        }
    }

    void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
        if (anim != null) anim.SetBool("isChasing", true);
    }

    void Idle()
    {
        agent.isStopped = true;
        if (anim != null) anim.SetBool("isChasing", false);
    }

    void AttackPlayer()
    {
        agent.isStopped = true;
        if (anim != null) anim.SetBool("isChasing", false);

        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            if (anim != null) anim.SetTrigger("Attack");
            if (playerHealth != null) playerHealth.TakeDamage(damage);
            lastAttackTime = Time.time;
        }
    }
}