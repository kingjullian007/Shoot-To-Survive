using UnityEngine;
using UnityEngine.AI;

public class EnemyAggressive : Enemy
{
    [SerializeField] private float stopDistance = 2f; // Distance at which the enemy stops and attacks
    [SerializeField] private float attackInterval = 1f; // Time between attacks
    private float lastAttackTime;

    private NavMeshAgent navMeshAgent;
    private Animator animator;

    protected override void Start ()
    {
        base.Start();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = stopDistance;

        animator = GetComponent<Animator>();
    }

    protected override void Update ()
    {
        base.Update();
        FollowPlayer();
        AttackPlayerIfClose();
    }

    private void FollowPlayer ()
    {
        if (playerTransform != null && Vector3.Distance(transform.position, playerTransform.position) > stopDistance)
        {
            navMeshAgent.SetDestination(playerTransform.position);
            // Set animator parameter for movement
            animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
        }
        else
        {
            navMeshAgent.ResetPath();
            // Set animator parameter for idle or stopping
            animator.SetFloat("Speed", 0f);
        }
    }

    private void AttackPlayerIfClose ()
    {
        var distance = Vector3.Distance(transform.position, playerTransform.position);
        if (playerTransform != null && distance <= stopDistance)
        {
            if (Time.time > lastAttackTime + attackInterval)
            {
                lastAttackTime = Time.time;
                // Trigger melee attack animation
                animator.SetTrigger("MeleeAttack");
                Debug.Log("Perform melee attack");
                // Optionally, reduce player health or trigger an attack animation
            }
        }
    }

    protected override void Attack ()
    {
        // This method can be used for additional attack behaviors if needed
    }
}
