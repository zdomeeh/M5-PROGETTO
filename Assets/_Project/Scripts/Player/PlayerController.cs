using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;

    private NavMeshAgent agent;
    private Camera mainCamera;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>(); // Prende il NavMeshAgent del player
        mainCamera = Camera.main;             // Prende la camera principale
    }

    private void Update()
    {
        HandleMovement(); // Gestisce il movimento ogni frame
    }

    private void HandleMovement()
    {
        if (!Input.GetMouseButtonDown(0)) return; // Se non clicca il tasto sinistro del mouse, esci

        // Crea un raggio dalla camera verso il punto cliccato
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Controlla se il raggio colpisce il terreno
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundMask))
        {
            // Sposta il player verso il punto cliccato
            agent.SetDestination(hit.point);
        }
    }
}