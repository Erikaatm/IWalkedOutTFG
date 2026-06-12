using UnityEngine;
public class PuertaBloqueada : MonoBehaviour, IInteractable
{
    public string mensaje = "Próximamente...";
    public int interactuableID;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public bool CanInteract() { return true; }

    public void Interact()
    {
        Transform interrogacion = transform.Find("interrogacion");
        if (interrogacion != null) interrogacion.gameObject.SetActive(false);

        SaveController save = FindObjectOfType<SaveController>();
        if (save != null)
        {
            save.MarcarInterrogacionDesactivada(interactuableID);
            save.SaveGame();
        }

        animator.SetTrigger("Bloqueada");
        DialogueController.Instance.ShowText(mensaje);
    }
}