using UnityEngine;

public class ButtonInteraction : MonoBehaviour
{
    [SerializeField] private ToggleObject toggleObject;
    [SerializeField] private InteractionUI interactionUI;

    private bool playerInRange = false;

    private void Update()
    {
        // Se il player è vicino e preme "E"
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Attiva o disattiva l'oggetto collegato
            if (toggleObject != null)
                toggleObject.Toggle();

            // Nasconde l'hint UI
            if (interactionUI != null)
                interactionUI.HideHint();
        }
    }

    // Quando il player entra nel trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;  // Il player è vicino

            // Mostra l'hint UI
            if (interactionUI != null)
                interactionUI.ShowHint();
        }
    }

    // Quando il player esce dal trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;  // Il player non è più vicino

            // Nasconde l'hint UI
            if (interactionUI != null)
                interactionUI.HideHint();
        }
    }
}