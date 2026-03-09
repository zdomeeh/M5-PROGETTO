using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [Header("Movement")]
    public NavMeshAgent agent;

    [Header("State")]
    public EnemyState currentState = EnemyState.Idle;
    protected EnemyState previousState;

    [Header("Stun")]
    protected Renderer rend;

    protected virtual void Awake()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        rend = GetComponent<Renderer>();
        if (rend == null)
            rend = GetComponentInChildren<Renderer>();
    }

    protected virtual void Update()
    {
        // Se stunnato, blocca Update base
        if (currentState == EnemyState.Stunned)
        {
            agent.isStopped = true;
            return;
        }
    }

    public virtual void ApplyStun(float duration)
    {
        if (currentState == EnemyState.Stunned) return;

        Debug.Log("STUN APPLICATO su " + gameObject.name);

        previousState = currentState;
        currentState = EnemyState.Stunned;

        // Blocca agente
        if (agent != null)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            agent.ResetPath();
        }

        // Cambia colore
        if (rend != null)
            rend.material.color = Color.blue;

        StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);

        // Ripristina stato precedente
        currentState = previousState;

        if (agent != null)
            agent.isStopped = false;

        if (rend != null)
            rend.material.color = Color.white;
    }

    public bool IsStunned()
    {
        return currentState == EnemyState.Stunned;
    }
}