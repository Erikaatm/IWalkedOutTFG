using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Image blackPanel;
    public TextMeshProUGUI narratorText;
    public Image irisPanel;

    [Header("Player")]
    public GameObject playerCharacter;

    [Header("Texto Narrador")]
    [TextArea]
    public string narratorMessage = "¿Dónde estoy...?\n¿A dónde voy...?";
    public float typingSpeed = 0.05f;
    public float textHoldTime = 2.0f;

    private Material irisMat;

    void Start()
    {
        irisMat = Instantiate(irisPanel.material);
        irisPanel.material = irisMat;

        // Estado inicial
        irisPanel.gameObject.SetActive(false);
        narratorText.text = "";
        narratorText.alpha = 1f;
        SetAlpha(blackPanel, 1f);
        playerCharacter.SetActive(false);

        StartCoroutine(PlayIntro());
    }

    IEnumerator PlayIntro()
    {
        // FASE 1: texto narrador
        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine(TypeText(narratorMessage));
        yield return new WaitForSeconds(textHoldTime);

        // FASE 2: texto desaparece
        yield return StartCoroutine(FadeOutText());

        // FASE 3: negro durante 1 segundo
        yield return new WaitForSeconds(1f);

        // FASE 4: preparar iris invisible
        playerCharacter.SetActive(true);

        SetIris(0.15f, 0f);              
        irisPanel.gameObject.SetActive(true);

        // Fade in
        yield return StartCoroutine(FadeInIris());

        // FASE 5: se queda 1 segundo
        yield return new WaitForSeconds(1f);

        // FASE 6: fade out
        yield return StartCoroutine(RevealGame());

        FinishIntro();
    }


    IEnumerator TypeText(string fullText)
    {
        narratorText.text = "";
        foreach (char letter in fullText)
        {
            narratorText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    IEnumerator FadeOutText()
    {
        float elapsed = 0f;
        while (elapsed < 0.5f)
        {
            elapsed += Time.deltaTime;
            narratorText.alpha = 1f - Mathf.Clamp01(elapsed / 0.5f);
            yield return null;
        }
        narratorText.alpha = 0f;
        narratorText.gameObject.SetActive(false);
    }

    IEnumerator RevealGame()
    {
        // PASO 1: círculo blanco se vuelve transparente
        float fadeDuration = 0.8f;
        float elapsed = 0f;
        Color panelColor = blackPanel.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            SetIris(0.15f, 1f - t);

            panelColor.a = 1f - t;
            blackPanel.color = panelColor;

            yield return null;
        }

        SetIris(0.15f, 0f);

        // PASO 2: expansión inmediata, sin pausa, blackPanel se va en el mismo frame
        blackPanel.gameObject.SetActive(false);

        float expandDuration = 2.0f;
        elapsed = 0f;

        while (elapsed < expandDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / expandDuration));
            float radius = Mathf.Lerp(0.15f, 1.5f, t);

            irisMat.SetFloat("_Radius", radius);
            irisMat.SetFloat("_WhiteAlpha", 0f);
            irisMat.SetFloat("_FadeAlpha", 1f - t);
            yield return null;
        }

        irisMat.SetFloat("_Radius", 1.5f);
        irisMat.SetFloat("_WhiteAlpha", 0f);
        irisMat.SetFloat("_FadeAlpha", 0f);
    }


    void FinishIntro()
    {
        irisPanel.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }

    void SetIris(float radius, float whiteAlpha)
    {
        irisMat.SetFloat("_Radius", radius);
        irisMat.SetFloat("_FadeAlpha", 1f);
        irisMat.SetFloat("_WhiteAlpha", whiteAlpha);
    }

    void SetAlpha(Image image, float alpha)
    {
        Color c = image.color;
        c.a = alpha;
        image.color = c;
    }

    IEnumerator FadeInIris()
    {
        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float radius = Mathf.Lerp(0f, 0.15f, t);

            irisMat.SetFloat("_Radius", radius);
            irisMat.SetFloat("_FadeAlpha", 1f);
            irisMat.SetFloat("_WhiteAlpha", 1f);

            yield return null;
        }

        SetIris(0.15f, 1f);
    }

    void Update()
    {
        if (irisMat != null)
        {
            float aspect = (float)Screen.width / (float)Screen.height;
            irisMat.SetFloat("_AspectRatio", aspect);
        }
    }


}