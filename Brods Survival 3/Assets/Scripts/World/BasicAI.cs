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

    public bool chasing = false;

    public float health = 100f;

    [Header("Audio")]
    public AudioSource audioS;
    public AudioClip animalWalk;
    public AudioClip animalAg;
    public AudioClip animalDie;
    public AudioClip animalAttack;

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

    //  private string debugState = null;

    public bool walk;
    public bool run;

    private void Start()
    {
        audioS = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentWanderTime = wanderWaitTime;

        if (agent == null)
        {
            Debug.LogError("No NavMeshAgent component found.");
            return;
        }

        if (!agent.isOnNavMesh)
        {
            Debug.LogError("NavMeshAgent is not on a NavMesh.");
            return;
        }
    }

    private void Update()
    {
        if (health <= 0)
        {
            audioS.Stop();
            audioS.PlayOneShot(animalDie);
            Die();
            return;
        }

        UpdateAnimations();
        if (target != null)
        {
            if (Vector3.Distance(target.transform.position, transform.position) > maxChaseDistance)
                target = null;

            if (!isAttacking)
            {
                Chase();
                //Debug.Log("Chasing");
            }

        }
        else
        {
            Wander();
        }

        UpdateCooldown();
    }

    private void Die()
    {
        if (agent == null || !agent.isOnNavMesh)
            return;
        chasing = false;
        agent.SetDestination(transform.position);
        Destroy(agent);
        anim.SetTrigger("Die");

        // Enable the remaining part of the GatherableObject script
        GatherableObject gatherableObject = GetComponent<GatherableObject>();
        if (gatherableObject != null)
        {
            gatherableObject.EnableAllFunctions();
        }

        Destroy(this);
    }




    private void OnDrawGizmos()
    {
        if (agent != null && agent.isOnNavMesh)
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
        chasing = false;

        if (agent == null || !agent.isOnNavMesh)
        {
            Debug.LogWarning("NavMeshAgent is not properly initialized or not on a NavMesh.");
            return;
        }

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
            if (!audioS.isPlaying)
                audioS.PlayOneShot(animalWalk);
        }
        else
        {
            float stoppingThreshold = 0.1f;

            if (!agent.pathPending && agent.remainingDistance <= stoppingThreshold)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    currentWanderTime += Time.deltaTime;
                    walk = false;
                    run = false;
                }
            }
        }
    }

    public void PlayAnimalChase()
    {
        if (chasing && !audioS.isPlaying)
        {
            audioS.PlayOneShot(animalAg);
        }
    }

    public void Chase()
    {
        if (agent == null || !agent.isOnNavMesh)
        {
            Debug.LogWarning("NavMeshAgent is not properly initialized or not on a NavMesh.");
            return;
        }
        chasing = true;
        PlayAnimalChase();

        agent.SetDestination(target.position);
        walk = false;
        run = true;
        agent.speed = runSpeed;
        if (Vector3.Distance(target.position, transform.position) <= minAttackDistance && !isAttacking)
            StartAttack();
    }

    public void StartAttack()
    {
        if (!audioS.isPlaying)
            audioS.PlayOneShot(animalAttack);
        chasing = false;
        if (!isAttacking && !isOnCooldown)
        {
            isAttacking = true;
            if (!canMoveWhileAttacking && agent != null && agent.isOnNavMesh)
                agent.SetDestination(transform.position);
            anim.SetTrigger("Attack");
            Invoke("FinishAttack", 0.5f);
        }
    }

    public void FinishAttack()
    {
        if (target != null)
        {
            target.GetComponent<PlayerStats>().health -= damage;
        }
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
            if (gameObject.CompareTag("Enemy"))
            {
                target = other.transform;
            }

    }
}
