using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class MenuOpciones : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] Toggle togglePantallaCompleta;

    Resolution[] resolutions;
    bool esPantallaCompleta;
    bool inicializando = false; // bandera para bloquear eventos programáticos

    void Start()
    {
        if (!PlayerPrefs.HasKey("ResolucionAncho"))
            AplicarResolucionPorDefecto();
        else
            AplicarAjustesGuardados();
    }

    void AplicarResolucionPorDefecto()
    {
        esPantallaCompleta = true;

        Resolution[] res = ObtenerResolucionesFiltradas();
        Resolution elegida = res[0];
        foreach (Resolution r in res)
        {
            if (r.width == 1920 && r.height == 1080)
            {
                elegida = r;
                break;
            }
        }

        Screen.SetResolution(elegida.width, elegida.height, FullScreenMode.ExclusiveFullScreen);
        GuardarResolucion(elegida.width, elegida.height);
        PlayerPrefs.SetInt("PantallaCompleta", 1);
        PlayerPrefs.Save();
    }

    void AplicarAjustesGuardados()
    {
        esPantallaCompleta = PlayerPrefs.GetInt("PantallaCompleta", 1) == 1;
        int ancho = PlayerPrefs.GetInt("ResolucionAncho", 1920);
        int alto = PlayerPrefs.GetInt("ResolucionAlto", 1080);
        Screen.SetResolution(ancho, alto,
            esPantallaCompleta ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed);
    }

    void GuardarResolucion(int ancho, int alto)
    {
        PlayerPrefs.SetInt("ResolucionAncho", ancho);
        PlayerPrefs.SetInt("ResolucionAlto", alto);
    }

    Resolution[] ObtenerResolucionesFiltradas()
    {
        Resolution[] todas = Screen.resolutions;
        System.Array.Sort(todas, (a, b) =>
        {
            int cmp = b.width.CompareTo(a.width);
            return cmp != 0 ? cmp : b.height.CompareTo(a.height);
        });

        List<Resolution> filtradas = new List<Resolution>();
        HashSet<string> vistas = new HashSet<string>();
        foreach (Resolution r in todas)
        {
            string clave = r.width + "x" + r.height;
            if (vistas.Add(clave))
                filtradas.Add(r);
        }
        return filtradas.ToArray();
    }

    public void InicializarOpciones()
    {
        if (resolutionDropdown == null) return;

        inicializando = true; // bloquear eventos mientras configuramos

        esPantallaCompleta = PlayerPrefs.GetInt("PantallaCompleta", 1) == 1;
        resolutions = ObtenerResolucionesFiltradas();

        List<string> opciones = new List<string>();
        foreach (Resolution r in resolutions)
            opciones.Add(r.width + " x " + r.height);

        resolutionDropdown.onValueChanged.RemoveAllListeners();
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(opciones);

        int ancho = PlayerPrefs.GetInt("ResolucionAncho", 1920);
        int alto = PlayerPrefs.GetInt("ResolucionAlto", 1080);
        int indiceGuardado = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == ancho && resolutions[i].height == alto)
            {
                indiceGuardado = i;
                break;
            }
        }

        resolutionDropdown.value = indiceGuardado;
        resolutionDropdown.RefreshShownValue();

        inicializando = false; // desbloquear antes de añadir el listener

        resolutionDropdown.onValueChanged.AddListener(CambiarResolucion);

        if (togglePantallaCompleta != null)
        {
            togglePantallaCompleta.onValueChanged.RemoveAllListeners();
            togglePantallaCompleta.isOn = esPantallaCompleta;
            togglePantallaCompleta.onValueChanged.AddListener(PantallaCompleta);
        }
    }

    public void CambiarResolucion(int indice)
    {
        if (inicializando) return; // ignorar si estamos configurando
        if (resolutions == null || indice >= resolutions.Length) return;

        Resolution res = resolutions[indice];
        Screen.SetResolution(res.width, res.height,
            esPantallaCompleta ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed);
        GuardarResolucion(res.width, res.height);
        PlayerPrefs.Save();
    }

    public void PantallaCompleta(bool pantallaCompleta)
    {
        esPantallaCompleta = pantallaCompleta;
        Screen.fullScreenMode = pantallaCompleta ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed;
        PlayerPrefs.SetInt("PantallaCompleta", pantallaCompleta ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void CambiarMusica(float musica)
    {
        audioMixer.SetFloat("Musica", musica);
    }
}