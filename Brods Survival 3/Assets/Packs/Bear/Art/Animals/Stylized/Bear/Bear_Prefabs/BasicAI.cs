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
    private float currentWanderTime;
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
            Wander();

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
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, agent.destination);
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
            if (agent.isStopped)
            {
                currentWanderTime += Time.deltaTime;
                walk = false;
                run = false;
            }
        }
    }

    public void Chase()
    {
        agent.SetDestination(target.transform.position);
        walk = false;
        run = true;
        agent.speed = runSpeed;
        if (Vector3.Distance(target.transform.position, transform.position) <= minAttackDistance && !isAttacking)
            StartAttack();
    }

    public void StartAttack()
    {
        if (!isAttacking && !isOnCooldown) // Only start attack if not already attacking and not on cooldown
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
