using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteraction : MonoBehaviour
{
    [SerializeField] private Door door;                       // Porta da aprire
    [SerializeField] private InteractionUI interactionUI;     // UI hint

    private bool playerInRange = false;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (door != null)
                door.ToggleDoor();

            // Nascondi subito la UI hint quando la porta si apre
            if (interactionUI != null)
                interactionUI.HideHint();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter: " + other.name);

        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactionUI != null)
                interactionUI.ShowHint();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactionUI != null)
                interactionUI.HideHint();
        }
    }
}