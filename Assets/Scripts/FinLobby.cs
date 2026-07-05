using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class FinLobby : MonoBehaviour
{
    public DialogoMinijuego dialogoFinal;
    public GameObject blackPanel;
    private bool activado = false;

    void Update()
    {
        if (activado) return;
        if (MinijuegoCocina.Instance.EstaResuelto() &&
        MinijuegoPiano.Instance.EstaResuelto() &&
        MinijuegoCocina.Instance.DialogoTerminado() &&
        MinijuegoPiano.Instance.DialogoTerminado())

        if (MinijuegoCocina.Instance.EstaResuelto() &&
            MinijuegoPiano.Instance.EstaResuelto() &&
            MinijuegoPiano.Instance.DialogoTerminado())
        {
            activado = true;
            StartCoroutine(IrAlFinal());
        }
    }

    IEnumerator IrAlFinal()
    {
        yield return new WaitForSeconds(1f);

        foreach (string texto in dialogoFinal.textosIniciales)
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
        yield return new WaitForSeconds(0.5f);

        // Fade a negro
        UnityEngine.UI.Image fondo = blackPanel.GetComponent<Image>();
        Color color = fondo.color;
        color.a = 0f;
        fondo.color = color;
        blackPanel.SetActive(true);

        while (color.a < 1f)
        {
            color.a += Time.deltaTime * 0.8f;
            fondo.color = color;
            yield return null;
        }

        SceneManager.LoadScene("EscenaFinal");
    }
}