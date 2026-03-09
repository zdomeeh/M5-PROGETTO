using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent agent;

    public EnemyState currentState = EnemyState.Idle;
    protected EnemyState previousState;

    protected Renderer rend;

    public float viewDistance = 8f;
    public float viewAngle = 90f;

    // Awake viene chiamato all'inizio
    protected virtual void Awake()
    {
        // Prendi il NavMeshAgent se non è collegato
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        // Prendi il Renderer per cambiare colore
        rend = GetComponent<Renderer>();
        if (rend == null)
            rend = GetComponentInChildren<Renderer>();
    }

    // Update chiamato ogni frame
    protected virtual void Update()
    {
        // Se il nemico è stunnato, blocca il movimento
        if (currentState == EnemyState.Stunned)
        {
            agent.isStopped = true;
            return;
        }
    }

    // Applica lo stun al nemico
    public virtual void ApplyStun(float duration)
    {
        if (currentState == EnemyState.Stunned) return; // non applicare se già stunnato

        Debug.Log("STUN APPLICATO su " + gameObject.name);

        previousState = currentState;      // salva stato precedente
        currentState = EnemyState.Stunned; // imposta stato Stunned

        // Blocca il NavMeshAgent
        if (agent != null)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            agent.ResetPath();
        }

        // Cambia colore del nemico per indicare lo stun
        if (rend != null)
            rend.material.color = Color.blue;

        // Avvia coroutine per rimuovere lo stun dopo la durata
        StartCoroutine(StunCoroutine(duration));
    }

    // Coroutine che gestisce la durata dello stun
    private IEnumerator StunCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);

        // Ripristina stato precedente
        currentState = previousState;

        // Riattiva il movimento
        if (agent != null)
            agent.isStopped = false;

        // Ripristina colore normale
        if (rend != null)
            rend.material.color = Color.white;
    }

    // Funzione di utilità per controllare se il nemico è stunnato
    public bool IsStunned()
    {
        return currentState == EnemyState.Stunned;
    }

    // Mostra in scena i gizmo della visuale del nemico (utile in editor)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        // Calcola i bordi del campo visivo
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward;

        // Disegna linee del campo visivo
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * viewDistance);

        // Disegna un cerchio che indica la distanza di vista
        Gizmos.DrawWireSphere(transform.position, viewDistance);
    }
}