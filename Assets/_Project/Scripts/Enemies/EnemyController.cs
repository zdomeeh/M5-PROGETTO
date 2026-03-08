using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [Header("Vision")]
    public Transform eye;
    public float viewDistance = 8f;
    public float viewAngle = 90f;
    public LayerMask obstacleMask;

    [Header("Movement")]
    public NavMeshAgent agent;
    public float chaseSpeed = 3.5f;

    [Header("State")]
    public EnemyState currentState = EnemyState.Idle;
    protected EnemyState previousState;

    protected Transform player;
    protected Vector3 lastKnownPlayerPos;

    protected virtual void Awake()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;
    }

    protected virtual void Update()
    {
        if (player == null || eye == null) return;

        // Non fare nulla se stunnato
        if (currentState == EnemyState.Stunned) return;

        if (CanSeePlayer())
        {
            lastKnownPlayerPos = player.position;
            StartChase();
        }
    }

    protected virtual void StartChase()
    {
        if (currentState != EnemyState.Chase)
            currentState = EnemyState.Chase;

        agent.isStopped = false;
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
    }

    protected bool CanSeePlayer()
    {
        Vector3 dir = player.position - eye.position;
        float distance = dir.magnitude;

        if (distance > viewDistance) return false;

        float angle = Vector3.Angle(eye.forward, dir);
        if (angle > viewAngle * 0.5f) return false;

        if (Physics.Raycast(eye.position, dir.normalized, out RaycastHit hit, viewDistance, ~obstacleMask))
        {
            if (hit.transform == player)
                return true;
        }

        return false;
    }

    // ----------------- NUOVO: Stun -----------------
    public void ApplyStun(float duration)
    {
        if (currentState == EnemyState.Stunned) return;

        previousState = currentState;
        currentState = EnemyState.Stunned;
        StartCoroutine(StunCoroutine(duration));
    }

    IEnumerator StunCoroutine(float duration)
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(duration);
        currentState = previousState;
        agent.isStopped = false;
    }
}