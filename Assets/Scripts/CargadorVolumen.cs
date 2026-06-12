using UnityEngine;

public class CargadorVolumen : MonoBehaviour
{
    void Awake()
    {
        float volumenGuardado = PlayerPrefs.GetFloat("Volumen", 1f);
        AudioListener.volume = volumenGuardado;

    }
}