using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupRecuerdo : MonoBehaviour
{
    public static PopupRecuerdo Instance { get; private set; }

    [Header("UI")]
    public GameObject panel;
    public GameObject fondoOscuro;
    public TMP_Text tituloText;
    public TMP_Text descripcionText;
    public Image imagenObjeto;

    private bool desdeDiario = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        panel.SetActive(false);
        if (fondoOscuro != null) fondoOscuro.SetActive(false);
    }

    public void Mostrar(string titulo, string descripcion, Sprite imagen, bool desdeDiario = false)
    {
        this.desdeDiario = desdeDiario;
        tituloText.text = titulo;
        descripcionText.text = descripcion;
        if (imagen != null)
        {
            imagenObjeto.sprite = imagen;
            imagenObjeto.preserveAspect = true;
        }
        if (fondoOscuro != null) fondoOscuro.SetActive(true);
        panel.SetActive(true);
        Time.timeScale = 0f;
        if (desdeDiario) PlayerInteraction.bloqueado = true;
    }

    public void Cerrar()
    {
        panel.SetActive(false);
        if (fondoOscuro != null) fondoOscuro.SetActive(false);
        Time.timeScale = 1f;
        if (desdeDiario) PlayerInteraction.bloqueado = false;
        desdeDiario = false;
    }

    void Update()
    {
        if (panel.activeSelf && !desdeDiario && Input.GetKeyDown(KeyCode.E))
            Cerrar();
    }
}