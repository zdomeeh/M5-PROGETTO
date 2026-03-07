using UnityEngine;

public class PatrolEnemy : EnemyController
{
    [SerializeField] private Transform[] patrolPoints;
    private int currentIndex;

    protected override void SetInitialState()
    {
        if (patrolPoints.Length == 0) return;
        currentIndex = 0;
        ChangeState(EnemyState.Patrol);
        agent.isStopped = false;              // Attiva il movimento
        agent.SetDestination(patrolPoints[0].position);
    }

    protected override void UpdatePatrol()
    {
        agent.isStopped = false;               // Assicurati che sia abilitato
        if (patrolPoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentIndex = (currentIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentIndex].position);
        }
    }
}