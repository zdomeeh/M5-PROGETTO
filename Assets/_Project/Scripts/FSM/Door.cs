using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private NavMeshSurface navMeshSurface;
    private bool isOpen = false;

    // Toggle della porta
    public void ToggleDoor()
    {
        isOpen = !isOpen;

        // Attiva/disattiva la porta principale
        gameObject.SetActive(!isOpen);

        // Aggiorna la NavMesh
        if (navMeshSurface != null)
            navMeshSurface.BuildNavMesh();
    }
}