using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PatrolEnemy : EnemyController
{
    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    public float waitTimeAtPoint = 0.5f;
    public float rotationSpeed = 5f;

    private int currentPatrolIndex = 0;
    private bool waiting = false;

    protected override void Awake()
    {
        base.Awake();
        if (patrolPoints.Length > 0)
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        currentState = EnemyState.Patrol;
    }

    protected override void Update()
    {
        if (currentState == EnemyState.Stunned) return; // blocco completo se stunnato

        if (patrolPoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && !waiting)
            StartCoroutine(MoveToNextPoint());

        RotateTowardsMovement();
    }

    private void RotateTowardsMovement()
    {
        if (!agent.hasPath) return;

        Vector3 direction = (agent.steeringTarget - transform.position).normalized;
        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private IEnumerator MoveToNextPoint()
    {
        waiting = true;

        agent.SetDestination(transform.position); // ferma il movimento
        yield return new WaitForSeconds(waitTimeAtPoint);

        if (currentState == EnemyState.Stunned) { waiting = false; yield break; } // blocco se stunnato

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);

        waiting = false;
    }
}