using System.Collections;
using UnityEngine;

public class PatrolEnemy : EnemyController
{
    public Transform[] patrolPoints;
    public float waitTimeAtPoint = 0.5f;
    public float rotationSpeed = 5f;

    private int currentPatrolIndex = 0;
    private bool waiting = false;

    // Awake viene chiamato all'inizio
    protected override void Awake()
    {
        base.Awake();  // chiama Awake della classe base (EnemyController)

        // Se ci sono punti di patrol, vai al primo
        if (patrolPoints.Length > 0)
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);

        currentState = EnemyState.Patrol; // Stato iniziale: Patrol
    }

    // Update viene chiamato ogni frame
    protected override void Update()
    {
        if (currentState == EnemyState.Stunned) return; // non fare nulla se stunnato
        if (patrolPoints.Length == 0) return;           // non fare nulla se non ci sono punti

        // Se il nemico ha raggiunto la destinazione e non sta aspettando
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && !waiting)
            StartCoroutine(MoveToNextPoint());

        RotateTowardsMovement(); // ruota il nemico verso la direzione del movimento
    }

    // Ruota il nemico verso la direzione in cui si muove
    private void RotateTowardsMovement()
    {
        if (!agent.hasPath) return;  // se non c'è percorso, esci

        Vector3 direction = (agent.steeringTarget - transform.position).normalized;
        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    // Coroutine che gestisce la pausa e il movimento al prossimo punto
    private IEnumerator MoveToNextPoint()
    {
        waiting = true;

        agent.SetDestination(transform.position); // ferma il movimento
        yield return new WaitForSeconds(waitTimeAtPoint); // aspetta un po'

        // Se il nemico viene stunnato durante l'attesa, esci
        if (currentState == EnemyState.Stunned) { waiting = false; yield break; }

        // Aggiorna l'indice del punto successivo (ciclo continuo)
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position); // vai al prossimo punto

        waiting = false;
    }
}