using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiarioUI : MonoBehaviour
{
    [Header("Panel principal")]
    public GameObject panelDiario;

    [Header("Tabs")]
    public RectTransform tabCocina;
    public RectTransform tabPadre;

    [Header("Posiciones tabs")]
    public float tabActivaY = -20f;
    public float tabInactivaY = -30f;

    [Header("Contenedor de entradas")]
    public Transform contenedorEntradas;
    public GameObject prefabEntrada;

    private string escenaActual = "Cocina";
    private bool abierto = false;

    void Start()
    {
        panelDiario.SetActive(false);
        ActualizarTabs();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            ToggleDiario();
    }

    public void ToggleDiario()
    {
        abierto = !abierto;
        panelDiario.SetActive(abierto);
        if (abierto)
        {
            Time.timeScale = 0f;
            MostrarEntradas(escenaActual);
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void MostrarCocina()
    {
        escenaActual = "Cocina";
        ActualizarTabs();
        MostrarEntradas("Cocina");
    }

    public void MostrarPadre()
    {
        escenaActual = "Padre";
        ActualizarTabs();
        MostrarEntradas("Padre");
    }

    void ActualizarTabs()
    {
        
        tabCocina.anchoredPosition = new Vector2(tabCocina.anchoredPosition.x,
            escenaActual == "Cocina" ? tabActivaY : tabInactivaY);
        tabPadre.anchoredPosition = new Vector2(tabPadre.anchoredPosition.x,
            escenaActual == "Padre" ? tabActivaY : tabInactivaY);

        tabCocina.GetComponent<Image>().color = escenaActual == "Cocina"
            ? Color.white
            : new Color(0.85f, 0.85f, 0.85f);
        tabPadre.GetComponent<Image>().color = escenaActual == "Padre"
            ? Color.white
            : new Color(0.85f, 0.85f, 0.85f);

        if (escenaActual == "Cocina")
        {
            tabCocina.GetComponent<Canvas>().sortingOrder = 2;  
            tabPadre.GetComponent<Canvas>().sortingOrder = 0;   
        }
        else
        {
            tabPadre.GetComponent<Canvas>().sortingOrder = 2;
            tabCocina.GetComponent<Canvas>().sortingOrder = 0;
        }
    }

    void MostrarEntradas(string escena)
    {
        foreach (Transform hijo in contenedorEntradas)
            Destroy(hijo.gameObject);

        List<EntradaDiario> entradas = DiarioManager.Instance.GetEntradasPorEscena(escena);
        foreach (EntradaDiario entrada in entradas)
        {
            GameObject obj = Instantiate(prefabEntrada);
            obj.transform.SetParent(contenedorEntradas, false);
            EntradaUI entradaUI = obj.GetComponent<EntradaUI>();
            entradaUI.Configurar(entrada);
        }
    }
}