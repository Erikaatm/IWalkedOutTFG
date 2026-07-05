using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class Creditos : MonoBehaviour
{
    public float velocidad = 80f;
    public string nombreEscenaSiguiente = "Lobby";
    public RectTransform contenedorCrawl;
    public RectTransform textoContenido;
    public GameObject panelTitulo;
    public RectTransform textoTitulo;
    public TMP_Text textoSaltar;
    public GameObject blackPanel;
    public AudioSource musicaFondo;

    private bool terminado = false;
    private float posicionFinal;
    private float tiempoManteniendoSpace = 0f;
    private float tiempoParaSaltar = 1f;

    void Start()
    {
        panelTitulo.SetActive(false);
        if (textoSaltar != null) textoSaltar.gameObject.SetActive(true);
        StartCoroutine(IniciarCrawl());
    }

    void Update()
    {
        if (terminado) return;
        contenedorCrawl.anchoredPosition += Vector2.up * velocidad * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            tiempoManteniendoSpace += Time.deltaTime;
            if (tiempoManteniendoSpace >= tiempoParaSaltar)
            {
                terminado = true;
                StartCoroutine(MostrarTitulo());
            }
        }
        else
        {
            tiempoManteniendoSpace = 0f;
        }
    }

    IEnumerator IniciarCrawl()
    {
        float posInicial = contenedorCrawl.anchoredPosition.y;
        LayoutRebuilder.ForceRebuildLayoutImmediate(textoContenido);
        yield return new WaitUntil(() => textoContenido.rect.height > 0);

        float alturaCanvas = contenedorCrawl.GetComponentInParent<Canvas>()
                                .GetComponent<RectTransform>().rect.height;
        float posFinal = posInicial + textoContenido.rect.height + alturaCanvas;

        if (PlayerPrefs.HasKey("saveData"))
        {
            SaveData data = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("saveData"));
            if (data.introVista)
            {
                terminado = true;
                StartCoroutine(MostrarTitulo());
                yield break;
            }
        }

        yield return new WaitUntil(() => terminado || contenedorCrawl.anchoredPosition.y >= posFinal);

        if (!terminado)
        {
            terminado = true;
            StartCoroutine(MostrarTitulo());
        }
    }

    IEnumerator MostrarTitulo()
    {
        contenedorCrawl.gameObject.SetActive(false);
        if (textoSaltar != null) textoSaltar.gameObject.SetActive(false);
        panelTitulo.SetActive(true);
        textoTitulo.localScale = Vector3.zero;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 0.3f;
            textoTitulo.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        Image fondo = blackPanel.GetComponent<Image>();
        Color color = fondo.color;
        color.a = 0f;
        fondo.color = color;
        blackPanel.SetActive(true);

        while (color.a < 1f)
        {
            color.a += Time.deltaTime * 1.5f;
            fondo.color = color;

            if (musicaFondo != null)
                musicaFondo.volume = Mathf.Lerp(0.5f, 0f, color.a);

            yield return null;
        }

        if (PlayerPrefs.HasKey("saveData"))
        {
            SaveData data = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("saveData"));
            data.introVista = true;
            data.escenaAnterior = "EscenaIntro";
            PlayerPrefs.SetString("saveData", JsonUtility.ToJson(data));
            PlayerPrefs.Save();
        }

        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(nombreEscenaSiguiente);
    }
}