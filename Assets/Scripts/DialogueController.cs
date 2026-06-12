using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance { get; private set; }
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public GameObject opcionesPanel;
    public Button[] botonesOpcion;
    public TMP_Text[] textosOpcion;
    public float velocidadEscritura = 0.03f;
    public bool escribiendo = false;

    private bool textoCompleto = false;
    private Coroutine coroutineEscritura;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
    }

    void Start()
    {
        dialoguePanel.SetActive(false);
    }

    public void ShowText(string text)
    {
        dialoguePanel.SetActive(true);
        opcionesPanel.SetActive(false);
        if (coroutineEscritura != null) StopCoroutine(coroutineEscritura);
        coroutineEscritura = StartCoroutine(EscribirTexto(text));
    }

    IEnumerator EscribirTexto(string texto)
    {
        escribiendo = true;
        textoCompleto = false;
        dialogueText.text = "";

        foreach (char letra in texto)
        {
            if (textoCompleto)
            {
                dialogueText.text = texto;
                break;
            }
            dialogueText.text += letra;
            yield return new WaitForSecondsRealtime(velocidadEscritura);
        }

        escribiendo = false;
        textoCompleto = true;
    }

    public void CompletarOAvanzar()
    {
        if (escribiendo)
            textoCompleto = true;
    }

    public void ShowOpciones(string texto, string[] opciones, System.Action[] acciones)
    {
        dialoguePanel.SetActive(true);
        opcionesPanel.SetActive(true);
        dialogueText.text = texto;

        for (int i = 0; i < botonesOpcion.Length; i++)
        {
            int index = i;
            textosOpcion[i].text = opciones[i];
            botonesOpcion[i].onClick.RemoveAllListeners();
            botonesOpcion[i].onClick.AddListener(() => acciones[index]());
        }
    }

    public void CloseDialogue()
    {
        if (coroutineEscritura != null) StopCoroutine(coroutineEscritura);
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        opcionesPanel.SetActive(false);
        escribiendo = false;
        textoCompleto = false;
    }
}