using UnityEngine;
using UnityEngine.UI;

public class ControladorVolumen : MonoBehaviour
{
    private Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();
        slider.minValue = 0f;
        slider.maxValue = 1f;

        // carga el volumen guardado, si no hay ninguno usa 1 por defecto
        float volumenGuardado = PlayerPrefs.GetFloat("Volumen", 1f);
        slider.value = volumenGuardado;
        AudioListener.volume = volumenGuardado;

        slider.onValueChanged.AddListener(CambiarVolumen);
    }

    public void CambiarVolumen(float valor)
    {
        AudioListener.volume = valor;
        PlayerPrefs.SetFloat("Volumen", valor); // guarda automáticamente
        PlayerPrefs.Save();
    }
}