using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float viewDistance = 8f;
    [Range(0, 180)]
    public float viewAngle = 90f;
    public LayerMask obstacleMask;
    public Transform eye;

    public float rotationSpeed = 5f;

    protected NavMeshAgent agent;
    protected Transform player;

    protected EnemyState currentState;
    protected EnemyState previousState;

    protected Vector3 lastKnownPlayerPosition;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Start()
    {
        SetInitialState();
    }

    protected virtual void Update()
    {
        UpdateState();

        if (CanSeePlayer())
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

        previousState = currentState;
        currentState = newState;

        OnStateEnter(newState);
    }

    protected virtual void UpdateState()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                UpdateIdle();
                break;

            case EnemyState.Patrol:
                UpdatePatrol();
                break;

            case EnemyState.Chase:
                UpdateChase();
                break;

            case EnemyState.Search:
                UpdateSearch();
                break;
        }
    }

    protected virtual void OnStateEnter(EnemyState state)
    {
        if (state == EnemyState.Chase)
        {
            agent.isStopped = false;
        }

        if (state == EnemyState.Search)
        {
            agent.SetDestination(lastKnownPlayerPosition);
        }
    }

    protected virtual void UpdateIdle() { }

    protected virtual void UpdatePatrol() { }

    protected virtual void UpdateChase()
    {
        agent.SetDestination(player.position);

        if (!CanSeePlayer())
        {
            ChangeState(EnemyState.Search);
        }
    }

    protected virtual void UpdateSearch()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            ChangeState(previousState);
        }
    }

    protected bool CanSeePlayer()
    {
        Vector3 dirToPlayer = player.position - eye.position;

        if (dirToPlayer.magnitude > viewDistance)
            return false;

        float dot = Vector3.Dot(eye.forward, dirToPlayer.normalized);

        if (dot < Mathf.Cos(viewAngle * 0.5f * Mathf.Deg2Rad))
            return false;

        if (Physics.Raycast(eye.position, dirToPlayer.normalized, out RaycastHit hit, viewDistance, ~obstacleMask))
        {
            return hit.transform == player;
        }

        return false;
    }
}