using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasUI : MonoBehaviour
{
    public static CanvasUI Instance;
    public GameObject blackPanel;
    public GameObject interactPrompt;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (interactPrompt != null) interactPrompt.SetActive(false);
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        if (blackPanel == null) yield break;

        UnityEngine.UI.Image fondo = blackPanel.GetComponent<UnityEngine.UI.Image>();
        Color color = fondo.color;
        color.a = 1f;
        fondo.color = color;
        blackPanel.SetActive(true);

        while (color.a > 0f)
        {
            color.a -= Time.deltaTime * 1.5f;
            fondo.color = color;
            yield return null;
        }

        blackPanel.SetActive(false);
    }


}