using UnityEngine;
using UnityEngine.UI;

public class ToggleSprite : MonoBehaviour
{
    [SerializeField] Toggle toggle;
    [SerializeField] Image imagen;
    [SerializeField] Sprite spriteOn;
    [SerializeField] Sprite spriteOff;

    void Start()
    {
        toggle.onValueChanged.AddListener(CambiarSprite);
        CambiarSprite(toggle.isOn); // aplicar al inicio
    }

    void CambiarSprite(bool isOn)
    {
        imagen.sprite = isOn ? spriteOn : spriteOff;
    }
}