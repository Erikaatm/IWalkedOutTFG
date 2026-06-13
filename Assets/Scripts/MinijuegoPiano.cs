using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MinijuegoPiano : MonoBehaviour
{
    public static MinijuegoPiano Instance;
    public GameObject panel;
    public GameObject fondoOscuro;
    public Image[] slotsUsados;
    public GameObject resultadoCorrecto;
    public GameObject resultadoIncorrecto;
    public AudioClip[] sonidosMelodia;
    public AudioSource audioSource;
    public DialogoMinijuego dialogoAlAcertar;
    public AudioClip sonidoCorrecto;
    public AudioClip sonidoIncorrecto;
    public AudioSource audioResultado;

    private List<string> notasMetidas = new List<string>();
    private string[] melodiaCorrecta = { "Sol", "Mi", "Sol", "La", "Sol", "Fa", "Mi", "Re", "Do", "Re", "Mi", "Sol", "La", "Sol", "Mi", "Do" };
    private bool resuelto = false;
    private Sprite[] spritesOriginales;
    private bool bloqueado = false;
    private bool dialogoTerminado = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(transform.root.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        spritesOriginales = new Sprite[slotsUsados.Length];
        for (int i = 0; i < slotsUsados.Length; i++)
            spritesOriginales[i] = slotsUsados[i].sprite;

        panel.SetActive(false);
        fondoOscuro.SetActive(false);
    }

    public void Abrir()
    {
        if (panel == null || fondoOscuro == null) return;
        bloqueado = false;
        PauseController.SetPause(true);
        panel.SetActive(true);
        fondoOscuro.SetActive(true);
        Time.timeScale = 0f;
        notasMetidas.Clear();
        LimpiarSlots();
        resultadoCorrecto.SetActive(false);
        resultadoIncorrecto.SetActive(false);
        if (MusicaFondo.Instance != null) MusicaFondo.Instance.Pausar();
    }

    public void Cerrar()
    {
        PauseController.SetPause(false);
        panel.SetActive(false);
        fondoOscuro.SetActive(false);
        Time.timeScale = 1f;
        if (MusicaFondo.Instance != null) MusicaFondo.Instance.Reanudar();
    }

    public bool EstaAbierto()
    {
        return panel != null && panel.activeSelf;
    }

    public bool EstaResuelto()
    {
        return resuelto;
    }

    public void ResetMinijuego()
    {
        resuelto = false;
        dialogoTerminado = false;
        bloqueado = false;
        notasMetidas.Clear();
    }

    void LimpiarSlots()
    {
        for (int i = 0; i < slotsUsados.Length; i++)
        {
            slotsUsados[i].sprite = spritesOriginales[i];
            slotsUsados[i].color = Color.white;
            TMP_Text texto = slotsUsados[i].GetComponentInChildren<TMP_Text>();
            if (texto != null) texto.text = "";
        }
    }

    public void PulsarTecla(string nota)
    {
        if (bloqueado) return;
        if (notasMetidas.Count >= 16) return;

        notasMetidas.Add(nota);

        int index = notasMetidas.Count - 1;

        slotsUsados[index].color = Color.white;
        TMP_Text texto = slotsUsados[index].GetComponentInChildren<TMP_Text>();

        if (texto != null) texto.text = nota;
        if (notasMetidas.Count == 16)
            StartCoroutine(ComprobarResultado());
    }

    IEnumerator ComprobarResultado()
    {
        bloqueado = true;
        yield return new WaitForSecondsRealtime(0.5f);

        bool correcto = true;
        for (int i = 0; i < 16; i++)
        {
            if (notasMetidas[i] != melodiaCorrecta[i])
            {
                correcto = false;
                break;
            }
        }

        if (correcto)
        {
            resuelto = true;
            resultadoCorrecto.SetActive(true);
            audioResultado.PlayOneShot(sonidoCorrecto);
            StartCoroutine(EsperarYReproducirMelodia());
            StartCoroutine(MostrarDialogoFinal());
        }
        else
        {
            resultadoIncorrecto.SetActive(true);
            audioResultado.PlayOneShot(sonidoIncorrecto);
            bloqueado = true;
        }
    }

    IEnumerator ReproducirMelodia()
    {
        string[] notas = { "Do", "Re", "Mi", "Fa", "Sol", "La", "Si" };
        TeclaPiano[] teclas = GetComponentsInChildren<TeclaPiano>();

        foreach (string nota in melodiaCorrecta)
        {
            int index = System.Array.IndexOf(notas, nota);
            if (index >= 0 && index < sonidosMelodia.Length)
            {
                audioSource.PlayOneShot(sonidosMelodia[index]);

                // Anima la tecla correspondiente
                foreach (TeclaPiano tecla in teclas)
                {
                    if (tecla.nota == nota)
                    {
                        tecla.AnimarDesdeExterno();
                        break;
                    }
                }

                yield return new WaitForSecondsRealtime(0.4f);
            }
        }
    }


    IEnumerator MostrarDialogoFinal()
    {
        PlayerInteraction.bloqueado = true;

        yield return new WaitForSecondsRealtime(1.5f + 16 * 0.4f);
        resultadoCorrecto.SetActive(false);
        Cerrar();

        PlayerInteraction.bloqueado = false;

        for (int i = 0; i < dialogoAlAcertar.textosIniciales.Length; i++)
        {
            DialogueController.Instance.ShowText(dialogoAlAcertar.textosIniciales[i]);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            if (DialogueController.Instance.escribiendo)
            {
                DialogueController.Instance.CompletarOAvanzar();
                yield return null;
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            }
            yield return null;
        }

        PlayerInteraction.bloqueado = true; // bloquea la E antes de mostrar opciones

        System.Action[] acciones = new System.Action[dialogoAlAcertar.opciones.Length];
        for (int i = 0; i < acciones.Length; i++)
        {
            int index = i;
            acciones[index] = () => {
                PulsoManager.Instance.ModificarPulso(dialogoAlAcertar.respuestas[index].valorPulso);
                StartCoroutine(MostrarTextoRespuesta(dialogoAlAcertar.respuestas[index]));
            };
        }

        DialogueController.Instance.ShowOpciones(
            dialogoAlAcertar.textosIniciales[dialogoAlAcertar.textosIniciales.Length - 1],
            dialogoAlAcertar.opciones,
            acciones
        );
    }

    IEnumerator MostrarTextoRespuesta(RespuestaDialogo respuesta)
    {
        foreach (string texto in respuesta.textos)
        {
            DialogueController.Instance.ShowText(texto);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            if (DialogueController.Instance.escribiendo)
            {
                DialogueController.Instance.CompletarOAvanzar();
                yield return null;
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            }
            yield return null;
        }
        DialogueController.Instance.CloseDialogue();
        PlayerInteraction.bloqueado = false;
        dialogoTerminado = true;
    }

    IEnumerator EsperarYReproducirMelodia()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        StartCoroutine(ReproducirMelodia());
    }

    public bool DialogoTerminado() { return dialogoTerminado; }
}