using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Vision Settings")]
    public float viewDistance = 8f;
    [Range(0, 180)]
    public float viewAngle = 90f;
    public LayerMask obstacleMask;
    public Transform eye;

    [Header("Movement Settings")]
    public float rotationSpeed = 5f;

    protected NavMeshAgent agent;
    protected Transform player;

    protected EnemyState currentState;
    protected EnemyState previousState;
    protected Vector3 lastKnownPlayerPosition;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    protected virtual void Start()
    {
        SetInitialState();
    }

    protected virtual void Update()
    {
        if (player == null) return;

        UpdateState();

        // Controllo se il nemico può vedere il player
        if (currentState != EnemyState.Chase && CanSeePlayer())
        {
            lastKnownPlayerPosition = player.position;
            ChangeState(EnemyState.Chase);
        }
    }

    protected virtual void SetInitialState()
    {
        ChangeState(EnemyState.Idle);
    }

    protected void ChangeState(EnemyState newState)
    {
        if (currentState == newState) return;

        OnStateExit(currentState);

        previousState = currentState;
        currentState = newState;

        OnStateEnter(newState);
    }

    protected virtual void UpdateState()
    {
        switch (currentState)
        {
            case EnemyState.Idle: UpdateIdle(); break;
            case EnemyState.Patrol: UpdatePatrol(); break;
            case EnemyState.Chase: UpdateChase(); break;
            case EnemyState.Search: UpdateSearch(); break;
        }
    }

    // Stati base (da sovrascrivere nei figli)
    protected virtual void UpdateIdle()
    {
        agent.isStopped = true;
    }

    protected virtual void UpdatePatrol()
    {
        // In PatrolEnemy verrà sovrascritto
        agent.isStopped = false;
    }

    protected virtual void UpdateChase()
    {
        if (player == null) return;

        agent.isStopped = false;
        agent.SetDestination(player.position);

        // Se perde la vista, passa a Search dopo 1 secondo
        if (!CanSeePlayer())
            Invoke(nameof(StartSearch), 1f);
    }

    private void StartSearch()
    {
        if (currentState == EnemyState.Chase)
            ChangeState(EnemyState.Search);
    }

    protected virtual void UpdateSearch()
    {
        agent.isStopped = false;
        agent.SetDestination(lastKnownPlayerPosition);

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            ChangeState(previousState); // torna allo stato precedente
    }

    protected virtual void OnStateEnter(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.Chase:
                agent.isStopped = false;
                break;
            case EnemyState.Search:
                agent.isStopped = false;
                agent.SetDestination(lastKnownPlayerPosition);
                break;
        }
    }

    protected virtual void OnStateExit(EnemyState state) { }

    // Controlla se il nemico può vedere il player
    protected bool CanSeePlayer()
    {
        if (player == null || eye == null) return false;

        Vector3 dirToPlayer = player.position - eye.position;
        if (dirToPlayer.magnitude > viewDistance) return false;

        float angle = Vector3.Angle(eye.forward, dirToPlayer);
        if (angle > viewAngle * 0.5f) return false;

        // Raycast per ostacoli
        if (Physics.Raycast(eye.position, dirToPlayer.normalized, out RaycastHit hit, viewDistance, ~obstacleMask))
            return hit.transform == player;

        return false;
    }
}