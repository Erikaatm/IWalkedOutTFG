using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class PanelAyuda : MonoBehaviour
{
    public GameObject panelAyuda;
    public Button botonReiniciar;
    private VideoPlayer videoPlayer;

    void Start()
    {
        // primero busca el VideoPlayer ANTES de desactivar
        videoPlayer = panelAyuda.GetComponentInChildren<VideoPlayer>(true);

        if (videoPlayer == null)
        {
            Debug.LogError("No se encontró VideoPlayer dentro de panelAyuda");
            return;
        }

        videoPlayer.loopPointReached += OnVideoTerminado;

        if (botonReiniciar != null)
            botonReiniciar.gameObject.SetActive(false);

        panelAyuda.SetActive(false); // desactiva al final
    }

    public void AbrirAyuda()
    {
        panelAyuda.SetActive(true);
        if (botonReiniciar != null)
            botonReiniciar.gameObject.SetActive(false);
        videoPlayer.Play();
    }

    public void CerrarAyuda()
    {
        videoPlayer.Stop();
        panelAyuda.SetActive(false);
    }

    void OnVideoTerminado(VideoPlayer vp)
    {
        if (botonReiniciar != null)
            botonReiniciar.gameObject.SetActive(true);
    }

    public void ReiniciarVideo()
    {
        if (botonReiniciar != null)
            botonReiniciar.gameObject.SetActive(false);
        videoPlayer.time = 0;
        videoPlayer.Play();
    }
}