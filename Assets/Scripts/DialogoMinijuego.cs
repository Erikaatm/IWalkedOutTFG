using UnityEngine;

[System.Serializable]
public class RespuestaDialogo
{
    [TextArea] public string[] textos;
    public EstadoPulso valorPulso;
}

[CreateAssetMenu(fileName = "DialogoMinijuego", menuName = "IWalkedOut/DialogoMinijuego")]
public class DialogoMinijuego : ScriptableObject
{
    [TextArea] public string[] textosIniciales;
    public string[] opciones;
    public RespuestaDialogo[] respuestas;
}