using UnityEngine;

public class ObjetoRecuerdo : MonoBehaviour, IInteractable
{
    [Header("Datos del recuerdo")]
    public string titulo;
    [TextArea(3, 6)]
    public string descripcion;
    public Sprite imagen;
    public string escena;
    public bool esPista = false;
    public int interactuableID;

    private bool guardado = false;

    public bool CanInteract() { return true; }

    public void Interact()
    {
        SaveController save = FindObjectOfType<SaveController>();
        if (save != null)
        {
            save.MarcarInterrogacionDesactivada(interactuableID);
            save.SaveGame();
        }

        // Siempre muestra el popup
        PopupRecuerdo.Instance.Mostrar(titulo, descripcion, imagen);

        // Solo guarda en el diario la primera vez
        if (!guardado)
        {
            EntradaDiario entrada = new EntradaDiario
            {
                titulo = titulo,
                descripcion = descripcion,
                imagen = imagen,
                escena = escena,
                esPista = esPista
            };
            DiarioManager.Instance.AgregarEntrada(entrada);
            guardado = true;
        }

        Transform interrogacion = transform.Find("interrogacion");
        if (interrogacion != null) interrogacion.gameObject.SetActive(false);
    }
}