using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LuzFuego : MonoBehaviour
{
    private Light2D luz;
    public float intensidadMin = 1f;
    public float intensidadMax = 2f;
    public float velocidad = 3f;

    void Start()
    {
        luz = GetComponent<Light2D>();
    }

    void Update()
    {
        luz.intensity = Mathf.Lerp(intensidadMin, intensidadMax,
            Mathf.PerlinNoise(Time.time * velocidad, 0f));
    }
}