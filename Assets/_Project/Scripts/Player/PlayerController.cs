using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;

    private NavMeshAgent agent;
    private Camera mainCamera;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundMask))
        {
            agent.SetDestination(hit.point);
        }
    }
}