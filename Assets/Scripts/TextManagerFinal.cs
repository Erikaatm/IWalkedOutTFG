using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextManagerFinal : MonoBehaviour
{
    [Header("UI Elements")]
    public Image blackPanel;
    public TextMeshProUGUI narratorText;

    [Header("Textos finales")]
    [TextArea] public string textoCaos = "El caos que sembraste definió tu camino...";
    [TextArea] public string textoNeutral = "Viviste en el equilibrio, sin extremos...";
    [TextArea] public string textoPerfeccion = "La perfección que buscaste te llevó hasta aquí...";

    [Header("Configuracion")]
    public float typingSpeed = 0.05f;
    public string nombreEscenaCreditos = "Creditos";

    void Start()
    {
        narratorText.text = "";
        SetAlpha(blackPanel, 1f);
        StartCoroutine(PlayFinal());
    }

    IEnumerator PlayFinal()
    {
        yield return new WaitForSeconds(1.5f);

        string texto = ObtenerTextoPorPulso();
        yield return StartCoroutine(TypeText(texto));
        yield return StartCoroutine(TypeText("\n\nPulsa E para continuar..."));

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));

        ReiniciarSave();

        SceneManager.LoadScene(nombreEscenaCreditos);
    }

    string ObtenerTextoPorPulso()
    {
        string path = Path.Combine(Application.persistentDataPath, "saveData.json");
        if (!File.Exists(path))
            return textoNeutral;
        SaveData data = JsonUtility.FromJson<SaveData>(File.ReadAllText(path));
        if (data.pulsoRoto > data.pulsoQuieto && data.pulsoRoto > data.pulsoClaro)
            return textoCaos;
        else if (data.pulsoClaro > data.pulsoQuieto && data.pulsoClaro > data.pulsoRoto)
            return textoPerfeccion;
        else
            return textoNeutral;
    }

    IEnumerator TypeText(string fullText)
    {
        foreach (char letter in fullText)
        {
            narratorText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    void SetAlpha(Image image, float alpha)
    {
        Color c = image.color;
        c.a = alpha;
        image.color = c;
    }

    void ReiniciarSave()
    {
        SaveData saveData = new SaveData
        {
            playerPosition = Vector3.zero,
            hotbarSaveData = new System.Collections.Generic.List<InventorySaveData>(),
            objetosCogidos = new System.Collections.Generic.List<int>(),
            objetosColocados = new System.Collections.Generic.List<ObjetoColocadoData>(),
            zonasUsadas = new System.Collections.Generic.List<int>(),
            pulsoRoto = 0,
            pulsoQuieto = 0,
            pulsoClaro = 0
        };

        string path = System.IO.Path.Combine(Application.persistentDataPath, "saveData.json");
        System.IO.File.WriteAllText(path, JsonUtility.ToJson(saveData));
    }
}