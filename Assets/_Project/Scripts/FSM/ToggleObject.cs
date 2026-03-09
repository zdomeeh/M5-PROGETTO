using Unity.AI.Navigation;
using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    // Riferimento all'oggetto che vogliamo attivare/disattivare
    [SerializeField] private GameObject targetObject;

    // Riferimento al NavMeshSurface che vogliamo ricostruire
    [SerializeField] private NavMeshSurface navMeshSurface;

    // Funzione pubblica che viene chiamata per eseguire l'azione
    public void Toggle()
    {
        // Se l'oggetto da controllare esiste, lo disattiva
        if (targetObject != null)
            targetObject.SetActive(false);

        // Se il NavMeshSurface esiste, ricostruisce il NavMesh
        if (navMeshSurface != null)
            navMeshSurface.BuildNavMesh();
    }
}