using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class BasicAI : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent agent;
    private Animator anim;
    public bool isAttacking;

    public float health = 100f;

    [Header("Attack Settings")]
    public float damage;
    public float maxChaseDistance;
    public float minAttackDistance = 1.5f;
    public float maxAttackDistance = 2.5f;

    [Header("Cooldown")]
    public float attackCooldown = 2f;
    private bool isOnCooldown = false;
    private float cooldownTimer = 0f;

    [Header("Movement")]
    public float currentWanderTime;
    public float wanderWaitTime = 10f;
    public bool canMoveWhileAttacking;
    [Space]
    public float walkSpeed = 2f;
    public float runSpeed = 3.5f;
    public float wanderRange = 5f;

    private string debugState = null;

    public bool walk;
    public bool run;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentWanderTime = wanderWaitTime;

        if (agent == null)
        {
           // Debug.LogError("No NavMeshAgent component found.");
        }
    }

    private void Update()
    {
        if (health <= 0)
        {
            Die();
            return;
        }

        UpdateAnimations();

        if (target != null)
        {
            if (Vector3.Distance(target.transform.position, transform.position) > maxChaseDistance)
                target = null;

            if (!isAttacking)
                Chase();
        }
        else
        {
            Wander();
        }

        UpdateCooldown();
    }

    private void Die()
    {
        agent.SetDestination(transform.position);
        Destroy(agent);
        anim.SetTrigger("Die");
        GetComponent<GatherableObject>().enabled = true;
        Destroy(this);
    }

    private void OnDrawGizmos()
    {
        if (agent != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, agent.destination);
        }
    }

    public void UpdateAnimations()
    {
        anim.SetBool("Walk", walk);
        anim.SetBool("Run", run);
    }

    public void Wander()
    {
        if (currentWanderTime >= wanderWaitTime)
        {
           // Debug.Log("Generating new wander position.");
            Vector3 wanderPos = transform.position;
            wanderPos.x += Random.Range(-wanderRange, wanderRange);
            wanderPos.z += Random.Range(-wanderRange, wanderRange);
            currentWanderTime = 0;
            agent.speed = walkSpeed;
            agent.SetDestination(wanderPos);
            walk = true;
            run = false;
        }
        else
        {
            // Additional debug logs for troubleshooting
            //Debug.Log($"Agent.velocity: {agent.velocity.magnitude}");
            //Debug.Log($"Agent.isStopped: {agent.isStopped}");
            //Debug.Log($"Agent.pathPending: {agent.pathPending}");
            //Debug.Log($"Agent.remainingDistance: {agent.remainingDistance}");
            //Debug.Log($"Agent.stoppingDistance: {agent.stoppingDistance}");
            //Debug.Log($"Agent.hasPath: {agent.hasPath}");

            // Define a small tolerance for considering the agent as stopped
            float stoppingThreshold = 0.1f;

            // Improved condition to determine if the agent has effectively stopped
            if (!agent.pathPending && agent.remainingDistance <= stoppingThreshold)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                   // Debug.Log("Agent has effectively stopped.");
                    currentWanderTime += Time.deltaTime;
                    walk = false;
                    run = false;
                }
            }
        }
    }


    public void Chase()
    {
        agent.SetDestination(target.position);
        walk = false;
        run = true;
        agent.speed = runSpeed;
        if (Vector3.Distance(target.position, transform.position) <= minAttackDistance && !isAttacking)
            StartAttack();
    }

    public void StartAttack()
    {
        if (!isAttacking && !isOnCooldown)
        {
            isAttacking = true;
            if (!canMoveWhileAttacking)
                agent.SetDestination(transform.position);
            anim.SetTrigger("Attack");
            Invoke("FinishAttack", 0.5f);
        }
    }

    public void FinishAttack()
    {
        target.GetComponent<PlayerStats>().health -= damage;
        isAttacking = false;
        isOnCooldown = true;
        cooldownTimer = attackCooldown;
    }

    private void UpdateCooldown()
    {
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                isOnCooldown = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
            target = other.transform;
    }
}
