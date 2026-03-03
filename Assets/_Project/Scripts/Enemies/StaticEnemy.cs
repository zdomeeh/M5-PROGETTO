using System.Collections;
using UnityEngine;

public class StaticEnemy : EnemyController
{
    [SerializeField] private float rotationInterval = 2f;
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
        else if (rotateCoroutine != null)
            StopCoroutine(rotateCoroutine);
    }

    private IEnumerator RotateRoutine()
    {
        while (true)
        {
            transform.Rotate(Vector3.up, 90f);
            yield return new WaitForSeconds(rotationInterval);
        }
    }
}