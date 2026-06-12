using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class Creditos : MonoBehaviour
{
    public RectTransform contenedorCrawl;
    public float velocidad = 80f;
    public string nombreEscenaSiguiente = "Lobby";
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
        posicionFinal = contenedorCrawl.rect.height + 600f;
        panelTitulo.SetActive(false);
        if (textoSaltar != null) textoSaltar.gameObject.SetActive(true);

        // Comprueba si el crawl ya se vio
        string path = System.IO.Path.Combine(Application.persistentDataPath, "saveData.json");
        if (System.IO.File.Exists(path))
        {
            SaveData data = JsonUtility.FromJson<SaveData>(System.IO.File.ReadAllText(path));
            if (data.introVista)
            {
                terminado = true;
                StartCoroutine(MostrarTitulo());
                return;
            }
        }
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

        if (contenedorCrawl.anchoredPosition.y >= posicionFinal)
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

        string path = System.IO.Path.Combine(Application.persistentDataPath, "saveData.json");
        if (System.IO.File.Exists(path))
        {
            SaveData data = JsonUtility.FromJson<SaveData>(System.IO.File.ReadAllText(path));
            data.introVista = true;
            data.escenaAnterior = "EscenaIntro";
            System.IO.File.WriteAllText(path, JsonUtility.ToJson(data));
        }

        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(nombreEscenaSiguiente);
    }
}