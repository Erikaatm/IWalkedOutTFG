using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MinijuegoCocina : MonoBehaviour
{
    public static MinijuegoCocina Instance;
    public GameObject panel;
    public GameObject fondoOscuro;
    public Image imagenFogon;
    public Image imagenSarten;
    public Image[] slotsUsados;
    public GameObject resultadoCorrecto;
    public GameObject resultadoIncorrecto;
    public Transform zonaIngredientes;
    public DialogoMinijuego dialogoAlAcertar;
    public AudioClip sonidoCorrecto;
    public AudioClip sonidoIncorrecto;
    public AudioSource audioResultado;

    private bool dialogoTerminado = false;
    public bool DialogoTerminado() { return dialogoTerminado; }

    private List<int> ordenMetido = new List<int>();
    private int[] ordenCorrecto = { 1, 2, 3, 4, 5, 6 };
    private Vector3[] posicionesOriginales;
    private bool resuelto = false;


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

    private void OnDestroy()
    {
        Debug.Log("MinijuegoCocina DESTRUIDO");
    }

    private void Start()
    {
        posicionesOriginales = new Vector3[zonaIngredientes.childCount];
        for (int i = 0; i < zonaIngredientes.childCount; i++)
        {
            posicionesOriginales[i] = zonaIngredientes.GetChild(i).localPosition;
            zonaIngredientes.GetChild(i).GetComponent<IngredienteArrastrable>().posicionOriginal = posicionesOriginales[i];
        }

        panel.SetActive(false);
        fondoOscuro.SetActive(false);
        imagenFogon.preserveAspect = true;
    }

    public void Abrir()
    {
        if (panel == null || fondoOscuro == null) return;
        PauseController.SetPause(true);
        panel.SetActive(true);
        fondoOscuro.SetActive(true);
        Time.timeScale = 0f;
        ordenMetido.Clear();
        LimpiarSlots();
        ReiniciarIngredientes();
        resultadoCorrecto.SetActive(false);
        resultadoIncorrecto.SetActive(false);
    }

    public void Cerrar()
    {
        PauseController.SetPause(false);
        panel.SetActive(false);
        fondoOscuro.SetActive(false);
        Time.timeScale = 1f;
    }

    public bool EstaAbierto() 
    {
        return panel != null && panel.activeSelf;
    }

    void LimpiarSlots()
    {
        foreach (Image slot in slotsUsados)
        {
            slot.sprite = null;
            slot.color = new Color(1, 1, 1, 0);
        }
    }

    public void AnadirIngrediente(IngredienteArrastrable ingrediente)
    {
        if (ordenMetido.Count >= 6) return;

        ordenMetido.Add(ingrediente.orden);

        // Actualiza el slot
        int index = ordenMetido.Count - 1;
        slotsUsados[index].sprite = ingrediente.GetComponentInChildren<Image>().sprite;
        slotsUsados[index].color = Color.white;
        slotsUsados[index].preserveAspect = true;

        // Oculta el ingrediente
        ingrediente.gameObject.SetActive(false);

        // Si es el 6º comprueba
        if (ordenMetido.Count == 6)
            StartCoroutine(ComprobarResultado());
    }

    IEnumerator ComprobarResultado()
    {
        yield return StartCoroutine(ShakeSarten());

        bool correcto = true;
        for (int i = 0; i < 6; i++)
        {
            if (ordenMetido[i] != ordenCorrecto[i])
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
            StartCoroutine(MostrarDialogoFinal());
        }
        else
        {
            resultadoIncorrecto.SetActive(true);
            audioResultado.PlayOneShot(sonidoIncorrecto);
        }

    }

    IEnumerator ShakeSarten()
    {
        Vector3 posOriginal = imagenSarten.rectTransform.localPosition;
        float duracion = 0.5f;
        float magnitud = 10f;
        float elapsed = 0f;

        while (elapsed < duracion)
        {
            float x = posOriginal.x + Random.Range(-magnitud, magnitud);
            float y = posOriginal.y + Random.Range(-magnitud, magnitud);
            imagenSarten.rectTransform.localPosition = new Vector3(x, y, posOriginal.z);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        imagenSarten.rectTransform.localPosition = posOriginal;
    }

    void ReiniciarIngredientes()
    {
        List<Vector3> posicionesBarajadas = new List<Vector3>(posicionesOriginales);
        for (int i = posicionesBarajadas.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Vector3 temp = posicionesBarajadas[i];
            posicionesBarajadas[i] = posicionesBarajadas[j];
            posicionesBarajadas[j] = temp;
        }

        for (int i = 0; i < zonaIngredientes.childCount; i++)
        {
            Transform ing = zonaIngredientes.GetChild(i);
            ing.localPosition = posicionesBarajadas[i];
            ing.GetComponent<IngredienteArrastrable>().posicionOriginal = posicionesBarajadas[i];
            ing.gameObject.SetActive(true);
            ing.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }

    public bool EstaResuelto()
    {
        return resuelto;
    }

    public void ResetMinijuego()
    {
        resuelto = false;
        dialogoTerminado = false;
        ordenMetido.Clear();
    }

    IEnumerator MostrarDialogoFinal()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        resultadoCorrecto.SetActive(false);
        Cerrar();

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
}