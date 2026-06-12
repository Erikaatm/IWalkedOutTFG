using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EscenaFinal : MonoBehaviour
{
    public AudioSource monitorAudio;
    public AudioClip pitidoRitmico;
    public AudioClip lineaPlana;
    public DialogoMinijuego dialogoClaro;
    public DialogoMinijuego dialogoQuieto;
    public DialogoMinijuego dialogoRoto;
    public GameObject ojosEcho;
    public GameObject blackPanel;

    private PulsoManager pulsoManager;

    void Start()
    {
        pulsoManager = FindObjectOfType<PulsoManager>();
        StartCoroutine(IniciarFinal());
    }

    IEnumerator IniciarFinal()
    {
        
        // Fade de entrada del audio
        monitorAudio.volume = 0f;

        int claro = pulsoManager.claro;
        int quieto = pulsoManager.quieto;
        int roto = pulsoManager.roto;

        if (roto > claro && roto > quieto)
            monitorAudio.clip = lineaPlana;
        else
            monitorAudio.clip = pitidoRitmico;

        monitorAudio.loop = true;
        monitorAudio.Play();

        // Fade in del audio
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 0.5f;
            monitorAudio.volume = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);

        DialogoMinijuego dialogoElegido;
        if (roto > claro && roto > quieto)
            dialogoElegido = dialogoRoto;
        else if (claro >= quieto)
            dialogoElegido = dialogoClaro;
        else
            dialogoElegido = dialogoQuieto;

        if (ojosEcho != null)
        {
            if (roto > claro && roto > quieto)
            {
                ojosEcho.SetActive(false); // Roto — ojos cerrados, no abre
            }
            else
            {
                ojosEcho.SetActive(true); // Claro/Quieto — ojos cerrados al inicio
                StartCoroutine(AbrirOjos());
            }
        }

        foreach (string texto in dialogoElegido.textosIniciales)
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
        yield return new WaitForSeconds(2f);
        DialogueController.Instance.CloseDialogue();
        yield return new WaitForSeconds(2f);

        // Fade a negro
        UnityEngine.UI.Image fondo = blackPanel.GetComponent<UnityEngine.UI.Image>();
        Color color = fondo.color;
        color.a = 0f;
        fondo.color = color;
        blackPanel.SetActive(true);

        while (color.a < 1f)
        {
            color.a += Time.deltaTime * 0.8f;
            fondo.color = color;
            monitorAudio.volume = Mathf.Lerp(1f, 0f, color.a);
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene("Creditos");
    }

    IEnumerator AbrirOjos()
    {
        yield return new WaitForSeconds(5f);
        if (ojosEcho != null) ojosEcho.SetActive(false); // desactiva — abre los ojos
    }
}