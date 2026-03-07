using System.Collections;
using UnityEngine;

public class StaticEnemy : EnemyController
{
    [SerializeField] private float rotationInterval = 2f;
    [SerializeField] private float rotationAngle = 90f;

    private Coroutine rotateCoroutine;

    protected override void SetInitialState()
    {
        ChangeState(EnemyState.Idle);
    }

    protected override void OnStateEnter(EnemyState state)
    {
        base.OnStateEnter(state);

        if (state == EnemyState.Idle)
            rotateCoroutine = StartCoroutine(RotateRoutine());
    }

    protected override void OnStateExit(EnemyState state)
    {
        if (rotateCoroutine != null)
        {
            StopCoroutine(rotateCoroutine);
            rotateCoroutine = null;
        }
    }

    private IEnumerator RotateRoutine()
    {
        while (true)
        {
            transform.Rotate(Vector3.up, rotationAngle);
            if (eye != null) eye.forward = transform.forward; // aggiorna eye
            yield return new WaitForSeconds(rotationInterval);
        }
    }

    protected override void UpdateIdle()
    {
        agent.isStopped = true; // Statico
    }
}