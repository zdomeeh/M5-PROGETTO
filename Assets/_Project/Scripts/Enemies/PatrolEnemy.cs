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

    void Start()
    {
        if (patrolPoints.Length == 0)
        {
            Debug.LogError("Patrol points non assegnati!");
            return;
        }

        agent.isStopped = false;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    protected override void Update()
    {
        if (currentState == EnemyState.Stunned) return; // fermo se stunnato

        base.Update();

        if (waiting || patrolPoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            StartCoroutine(MoveToNextPoint());
        }

        RotateTowardsMovement();
    }

    void RotateTowardsMovement()
    {
        if (!agent.hasPath) return;

        Vector3 direction = (agent.steeringTarget - transform.position).normalized;
        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    IEnumerator MoveToNextPoint()
    {
        waiting = true;
        agent.isStopped = true;

        yield return new WaitForSeconds(waitTimeAtPoint);

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);

        agent.isStopped = false;
        waiting = false;
    }

    protected override void StartChase()
    {
        base.StartChase();
    }

    void OnDrawGizmos()
    {
        if (eye == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(eye.position, eye.forward * viewDistance);

        Vector3 leftDir = Quaternion.Euler(0, -viewAngle / 2, 0) * eye.forward;
        Vector3 rightDir = Quaternion.Euler(0, viewAngle / 2, 0) * eye.forward;
        Gizmos.DrawRay(eye.position, leftDir * viewDistance);
        Gizmos.DrawRay(eye.position, rightDir * viewDistance);
    }
}