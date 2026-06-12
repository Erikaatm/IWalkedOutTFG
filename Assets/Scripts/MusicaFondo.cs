using UnityEngine;
using System.Collections;

public class MusicaFondo : MonoBehaviour
{
    public static MusicaFondo Instance;
    public AudioClip musicaFondo;
    [Range(0f, 1f)]
    public float volumen = 0.5f;
    public float duracionFade = 1.5f;
    private AudioSource audioSource;

    void Awake()
    {
        MusicaFondo[] musicas = FindObjectsOfType<MusicaFondo>();
        foreach (MusicaFondo m in musicas)
        {
            if (m != this)
            {
                m.GetComponent<AudioSource>().Stop();
                Destroy(m.gameObject);
            }
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = musicaFondo;
        audioSource.loop = true;
        audioSource.volume = 0f;
        audioSource.Play();
        StartCoroutine(FadeIn());
    }

    public void Pausar()
    {
        StartCoroutine(FadeOut());
    }

    public void Reanudar()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        audioSource.UnPause();
        float t = audioSource.volume;
        while (t < volumen)
        {
            t += Time.unscaledDeltaTime / duracionFade;
            audioSource.volume = Mathf.Min(t, volumen);
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        float t = audioSource.volume;
        while (t > 0f)
        {
            t -= Time.unscaledDeltaTime / duracionFade;
            audioSource.volume = Mathf.Max(t, 0f);
            yield return null;
        }
        audioSource.Pause();
    }
}