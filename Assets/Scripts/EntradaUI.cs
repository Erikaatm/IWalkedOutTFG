using UnityEngine;
using UnityEngine.UI;

public class EntradaUI : MonoBehaviour
{
    public Image imagen;
    private EntradaDiario datos;
    public Sprite spriteNormal;
    public Sprite spritePista;

    public void Configurar(EntradaDiario entrada)
    {
        datos = entrada;
        if (entrada.imagen != null)
        {
            imagen.sprite = entrada.imagen;
            imagen.preserveAspect = true;
        }

        Image marco = GetComponentInChildren<Image>();
        if (marco != null)
            marco.sprite = entrada.esPista ? spritePista : spriteNormal;

        GetComponent<Button>().onClick.AddListener(() =>
            PopupRecuerdo.Instance.Mostrar(datos.titulo, datos.descripcion, datos.imagen));
    }
}