using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TeclaPiano : MonoBehaviour, IPointerDownHandler
{
    public string nota;
    public AudioClip sonido;
    public AudioSource audioSource;

    private Image imagen;
    private Color colorNormal = new Color(1, 1, 1, 0);
    private Color colorPulsado = new Color(0.3f, 0.3f, 0.3f, 0.8f);

    public void OnPointerDown(PointerEventData eventData)
    {
        
        if (imagen == null)
            imagen = GetComponent<Image>();

        MinijuegoPiano.Instance.PulsarTecla(nota);
        audioSource.PlayOneShot(sonido);
        StartCoroutine(AnimarTecla());
    }

    System.Collections.IEnumerator AnimarTecla()
    {
        imagen.color = colorPulsado;
        yield return new WaitForSecondsRealtime(0.15f);
        imagen.color = colorNormal;
    }

    public void AnimarDesdeExterno()
    {
        StartCoroutine(AnimarTecla());
    }
}