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

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        panel.SetActive(false);
        if (fondoOscuro != null) fondoOscuro.SetActive(false);
    }

    public void Mostrar(string titulo, string descripcion, Sprite imagen)
    {
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
    }

    public void Cerrar()
    {
        panel.SetActive(false);
        if (fondoOscuro != null) fondoOscuro.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (panel.activeSelf && Input.GetKeyDown(KeyCode.E))
            Cerrar();
    }
}