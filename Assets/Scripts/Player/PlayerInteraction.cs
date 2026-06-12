using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public static bool bloqueado = false;

    public float interactionRadius = 1.5f;
    public KeyCode interactKey = KeyCode.E;
    public LayerMask npcLayer;
    public GameObject interactPrompt;
    private IInteractable currentInteractable;

    void Start()
    {
        CanvasUI canvasUI = FindObjectOfType<CanvasUI>();
        if (canvasUI != null)
            interactPrompt = canvasUI.interactPrompt;
        currentInteractable = null;
        if (interactPrompt != null) interactPrompt.SetActive(false);
    }

    void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        currentInteractable = null;
        if (interactPrompt != null) interactPrompt.SetActive(false);

        if (scene.name == "EscenaFinal" || scene.name == "Creditos" || scene.name == "EscenaIntro" || scene.name == "MenuPrincipal")
        {
            if (interactPrompt != null) interactPrompt.SetActive(false);
            this.enabled = false;
        }
        else
            this.enabled = true;
    }

    void Update()
    {
        if (bloqueado) return;

        FindInteractable();
        if (Input.GetKeyDown(interactKey))
        {
            if (DialogueController.Instance != null &&
                DialogueController.Instance.dialoguePanel.activeSelf)
            {
                DialogueController.Instance.CompletarOAvanzar();
                if (!DialogueController.Instance.escribiendo)
                    DialogueController.Instance.CloseDialogue();
                return;
            }
            if (currentInteractable != null && currentInteractable.CanInteract())
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
        currentInteractable = found;
        if (interactPrompt != null)
            interactPrompt.SetActive(found != null);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}