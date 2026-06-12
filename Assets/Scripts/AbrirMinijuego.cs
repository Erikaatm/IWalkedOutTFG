using UnityEngine;

public class AbrirMinijuego : MonoBehaviour, IInteractable
{
    public enum TipoMinijuego { Cocina, Padre }
    public TipoMinijuego tipo;
    public int interactuableID;

    private bool marcadoResuelto = false;

    void Update()
    {
        if (marcadoResuelto) return;

        if (tipo == TipoMinijuego.Cocina && MinijuegoCocina.Instance != null)
        {
            if (MinijuegoCocina.Instance.EstaResuelto()) MarcarResuelto();
        }
        else if (tipo == TipoMinijuego.Padre && MinijuegoPiano.Instance != null)
        {
            if (MinijuegoPiano.Instance.EstaResuelto()) MarcarResuelto();
        }
    }

    public bool CanInteract() { return true; }

    public void Interact()
    {
        if (tipo == TipoMinijuego.Cocina)
        {
            if (MinijuegoCocina.Instance == null) return;
            if (MinijuegoCocina.Instance.EstaAbierto()) { MinijuegoCocina.Instance.Cerrar(); return; }
            if (MinijuegoCocina.Instance.EstaResuelto()) return;
            MinijuegoCocina.Instance.Abrir();
        }
        else if (tipo == TipoMinijuego.Padre)
        {
            if (MinijuegoPiano.Instance == null) return;
            if (MinijuegoPiano.Instance.EstaAbierto()) { MinijuegoPiano.Instance.Cerrar(); return; }
            if (MinijuegoPiano.Instance.EstaResuelto()) return;
            MinijuegoPiano.Instance.Abrir();
        }
    }

    public void MarcarResuelto()
    {
        marcadoResuelto = true;

        SaveController save = FindObjectOfType<SaveController>();
        if (save != null) { save.MarcarInterrogacionDesactivada(interactuableID); save.SaveGame(); }

        Transform interrogacion = transform.Find("interrogacion");
        if (interrogacion != null) interrogacion.gameObject.SetActive(false);
        Transform estrella = transform.Find("estrella");
        if (estrella != null) estrella.gameObject.SetActive(true);
    }
}