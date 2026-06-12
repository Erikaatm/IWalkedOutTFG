using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeEntrada : MonoBehaviour
{
    public Image blackPanel;
    public AudioSource musicaFondo;
    public float duracionFade = 1.5f;
    public DialogoMinijuego dialogoEntrada;

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        blackPanel.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        Color color = blackPanel.color;
        color.a = 1f;
        blackPanel.color = color;

        if (musicaFondo != null)
            musicaFondo.volume = 0f;

        yield return null;
        yield return null;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 0.8f / duracionFade;
            color.a = Mathf.Lerp(1f, 0f, t);
            blackPanel.color = color;

            if (musicaFondo != null)
                musicaFondo.volume = Mathf.Lerp(0f, 0.5f, t);

            yield return null;
        }

        blackPanel.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        SaveController save = FindObjectOfType<SaveController>();
        if (save != null && !save.dialogoLobbyVisto && dialogoEntrada != null)
        {
            save.dialogoLobbyVisto = true;
            save.SaveGame();

            DialogueController.Instance.ShowText(dialogoEntrada.textosIniciales[0]);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            if (DialogueController.Instance.escribiendo)
            {
                DialogueController.Instance.CompletarOAvanzar();
                yield return null;
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            }
            yield return null;
            DialogueController.Instance.CloseDialogue();
        }
    }
}