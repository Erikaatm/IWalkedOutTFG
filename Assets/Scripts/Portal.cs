using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour, IInteractable
{
    public string escenaDestino;

    public bool CanInteract() { return true; }

    public void Interact()
    {
        SceneManager.LoadScene(escenaDestino);
    }
}