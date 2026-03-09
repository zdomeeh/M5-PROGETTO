using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class StaticEnemy : EnemyController
{
    [Header("Rotation")]
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
        base.Awake();
        homePosition = transform.position;
        homeRotation = transform.rotation;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        currentState = EnemyState.Idle;
    }

    protected override void Update()
    {
        if (currentState == EnemyState.Stunned) return; // blocco completo se stunnato

        HandleState();
    }

    private void HandleState()
    {
        switch (currentStaticState)
        {
            case State.IdleRotate:
                rotationTimer += Time.deltaTime;
                if (rotationTimer >= rotationInterval)
                {
                    transform.Rotate(Vector3.up, rotationAngle);
                    rotationTimer = 0f;
                }
                agent.isStopped = true;

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
                    agent.SetDestination(player.position);
                }
                else
                {
                    currentStaticState = State.ReturnHome;
                    agent.SetDestination(homePosition);
                }
                break;

            case State.ReturnHome:
                agent.SetDestination(homePosition);

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

    private bool CanSeePlayer()
    {
        if (player == null) return false;
        return Vector3.Distance(player.position, transform.position) < 8f; // distanza semplice
    }
}