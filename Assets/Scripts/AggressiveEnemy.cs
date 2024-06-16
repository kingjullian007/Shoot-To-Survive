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
        if (Singleton.Instance.GameManagerInstance.CurrentState != GameState.GamePlay)
        {
            return;
        }
        base.Update();
        FollowPlayer();
        //AttackPlayerIfClose();
        Attack();
    }

    private void FollowPlayer ()
    {
        if (playerTransform != null)
        {
            var distance = Vector3.Distance(transform.position, playerTransform.position);
            if (distance > stopDistance)
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
                RotateTowardsPlayer(); // Rotate towards player when stopping
            }
        }
    }

    private void RotateTowardsPlayer ()
    {
        var direction = ( playerTransform.position - transform.position ).normalized;
        var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * navMeshAgent.angularSpeed);
    }

    protected override void Attack () 
    { 
        base.Attack();
        if (playerTransform != null)
        {
            var distance = Vector3.Distance(transform.position, playerTransform.position);
            if (distance <= stopDistance)
            {
                RotateTowardsPlayer(); // Ensure rotation towards player when attacking
                if (Time.time > lastAttackTime + attackInterval)
                {
                    lastAttackTime = Time.time;
                    // Trigger melee attack animation
                    animator.SetTrigger("MeleeAttack");
                }
            }
        }
    }
    
}
