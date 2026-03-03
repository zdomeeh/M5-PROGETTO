using UnityEngine;

public class PatrolEnemy : EnemyController
{
    [SerializeField] private Transform[] patrolPoints;
    private int currentIndex;

    protected override void SetInitialState()
    {
        ChangeState(EnemyState.Patrol);
        agent.SetDestination(patrolPoints[0].position);
    }

    protected override void UpdatePatrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentIndex = (currentIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentIndex].position);
        }
    }
}