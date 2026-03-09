using UnityEngine;

public class StaticEnemy : EnemyController
{
    public float rotationAngle = 90f;
    public float rotationInterval = 2f;

    private Vector3 homePosition;
    private Quaternion homeRotation;
    private float rotationTimer;

    private enum State { IdleRotate, Chase, ReturnHome }
    private State currentStaticState = State.IdleRotate;

    private Transform player;

    protected override void Awake()
    {
        base.Awake();                        // Awake della classe base EnemyController
        homePosition = transform.position;   // salva posizione iniziale
        homeRotation = transform.rotation;   // salva rotazione iniziale
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        currentState = EnemyState.Idle;      // stato base: Idle
    }

    protected override void Update()
    {
        if (currentState == EnemyState.Stunned) return; // blocco completo se stunnato
        HandleState();                                    // gestisci comportamenti in base allo stato
    }

    // Gestisce i diversi comportamenti in base allo stato
    private void HandleState()
    {
        switch (currentStaticState)
        {
            case State.IdleRotate:
                rotationTimer += Time.deltaTime;
                if (rotationTimer >= rotationInterval)
                {
                    transform.Rotate(Vector3.up, rotationAngle); // ruota di rotationAngle gradi
                    rotationTimer = 0f;
                }
                agent.isStopped = true;  // nemico fermo

                // Se vede il player, passa allo stato Chase
                if (CanSeePlayer())
                {
                    currentStaticState = State.Chase;
                    currentState = EnemyState.Chase;
                    agent.isStopped = false;
                    agent.SetDestination(player.position);
                }
                break;

            case State.Chase:
                if (CanSeePlayer())
                {
                    // Insegue il player
                    agent.SetDestination(player.position);
                }
                else
                {
                    // Non vede più il player, torna a casa
                    currentStaticState = State.ReturnHome;
                    agent.SetDestination(homePosition);
                }
                break;

            case State.ReturnHome:
                agent.SetDestination(homePosition);

                // Quando arriva a casa, ripristina posizione e rotazione iniziale
                if (!agent.pathPending && agent.remainingDistance < 0.2f)
                {
                    transform.position = homePosition;
                    transform.rotation = homeRotation;
                    currentStaticState = State.IdleRotate;
                    currentState = EnemyState.Idle;
                }
                break;
        }
    }

    // Controlla se il player è visibile (qui distanza semplice)
    private bool CanSeePlayer()
    {
        if (player == null) return false;
        return Vector3.Distance(player.position, transform.position) < 8f;
    }
}