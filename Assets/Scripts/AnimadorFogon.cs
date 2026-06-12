using UnityEngine;
using UnityEngine.UI;

public class AnimadorFogon : MonoBehaviour
{
    public Sprite[] frames;
    public float fps = 8f;
    private Image imagen;
    private float timer;
    private int frameActual;

    void Start()
    {
        imagen = GetComponent<Image>();
        imagen.preserveAspect = true;
    }

    void Update()
    {
        if (frames.Length == 0) return;

        timer += Time.unscaledDeltaTime;

        if (timer >= 1f / fps)
        {
            timer = 0f;
            frameActual = (frameActual + 1) % frames.Length;
            imagen.sprite = frames[frameActual];
        }
    }
}