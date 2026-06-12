using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRadius = 1.5f;
    public KeyCode interactKey = KeyCode.E;
    public LayerMask npcLayer;
    public GameObject interactPrompt;
    private IInteractable currentInteractable;

    void Update()
    {
        FindInteractable();
        if (Input.GetKeyDown(interactKey) && currentInteractable != null)
        {
            if (currentInteractable.CanInteract())
                currentInteractable.Interact();
        }
    }

    void FindInteractable()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactionRadius, npcLayer);
        IInteractable found = null;
        float menorDistancia = float.MaxValue;

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject == gameObject) continue;

            IInteractable interactable = hit.GetComponent<IInteractable>();
            if (interactable == null) continue;

            float distancia = Vector2.Distance(transform.position, hit.ClosestPoint(transform.position));
            if (distancia < menorDistancia)
            {
                menorDistancia = distancia;
                found = interactable;
            }
        }

        if (found != currentInteractable)
        {
            currentInteractable = found;
            if (interactPrompt != null)
                interactPrompt.SetActive(found != null);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}