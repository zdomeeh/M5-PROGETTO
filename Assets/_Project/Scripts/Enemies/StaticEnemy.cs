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
    private State previousStaticState;

    protected override void Awake()
    {
        base.Awake();
        homePosition = transform.position;
        homeRotation = transform.rotation;
    }

    protected override void Update()
    {
        if (currentState == EnemyState.Stunned) return; // ferma tutto se stunnato

        base.Update();

        switch (currentStaticState)
        {
            case State.IdleRotate: IdleRotateUpdate(); break;
            case State.Chase: ChaseUpdate(); break;
            case State.ReturnHome: ReturnHomeUpdate(); break;
        }
    }

    protected override void StartChase()
    {
        currentStaticState = State.Chase;
        base.StartChase();
    }

    void IdleRotateUpdate()
    {
        rotationTimer += Time.deltaTime;

        if (rotationTimer >= rotationInterval)
        {
            transform.Rotate(Vector3.up, rotationAngle);
            rotationTimer = 0f;
        }

        agent.isStopped = true;
    }

    void ChaseUpdate()
    {
        if (CanSeePlayer())
        {
            agent.SetDestination(player.position);
        }
        else
        {
            currentStaticState = State.ReturnHome;
            agent.SetDestination(homePosition);
        }
    }

    void ReturnHomeUpdate()
    {
        agent.SetDestination(homePosition);

        if (!agent.pathPending && agent.remainingDistance < 0.2f)
        {
            agent.isStopped = true;
            transform.position = homePosition;
            transform.rotation = homeRotation;
            currentStaticState = State.IdleRotate;
        }
    }

    // Nuovo metodo per stun che ferma la rotazione
    public new void ApplyStun(float duration)
    {
        if (currentState == EnemyState.Stunned) return;

        previousStaticState = currentStaticState;
        currentState = EnemyState.Stunned;
        StartCoroutine(StunCoroutine(duration));
    }

    IEnumerator StunCoroutine(float duration)
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(duration);
        currentState = EnemyState.Idle; // torna a Idle
        currentStaticState = previousStaticState;
        agent.isStopped = false;
    }
}