using System.Collections;
using System.Collections.Generic;
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
        if (!PlayerPrefs.HasKey("saveData"))
            return textoNeutral;

        SaveData data = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("saveData"));

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
            objetosCogidos = new List<int>(),
            pulsoRoto = 0,
            pulsoQuieto = 0,
            pulsoClaro = 0
        };
        PlayerPrefs.SetString("saveData", JsonUtility.ToJson(saveData));
        PlayerPrefs.Save();
    }
}