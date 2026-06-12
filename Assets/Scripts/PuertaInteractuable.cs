using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PuertaInteractuable : MonoBehaviour, IInteractable
{
    public string escenaDestino;
    public int interactuableID;
    public GameObject blackPanel;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public bool CanInteract() { return true; }

    public void Interact()
    {
        SaveController save = FindObjectOfType<SaveController>();
        if (save != null)
        {
            save.MarcarInterrogacionDesactivada(interactuableID);
            save.SaveGame();
            Debug.Log("Guardado. Interrogaciones: " + save.interrogacionesDesactivadas.Count);
        }
        else
        {
            Debug.Log("SaveController no encontrado");
        }

        Transform interrogacion = transform.Find("interrogacion");
        if (interrogacion != null) interrogacion.gameObject.SetActive(false);

        StartCoroutine(AbrirYCargar());
    }


    IEnumerator AbrirYCargar()
    {
        animator.SetTrigger("Abrir");
        yield return new WaitForSeconds(0.8f);

        // Fade a negro
        UnityEngine.UI.Image fondo = blackPanel.GetComponent<UnityEngine.UI.Image>();
        Color color = fondo.color;
        color.a = 0f;
        fondo.color = color;
        blackPanel.SetActive(true);

        while (color.a < 1f)
        {
            color.a += Time.deltaTime * 1.5f;
            fondo.color = color;
            yield return null;
        }

        SaveController save = FindObjectOfType<SaveController>();
        if (save != null)
        {
            save.escenaAnterior = SceneManager.GetActiveScene().name;
            save.SaveGame();
        }

        SceneManager.LoadScene(escenaDestino);
    }
}