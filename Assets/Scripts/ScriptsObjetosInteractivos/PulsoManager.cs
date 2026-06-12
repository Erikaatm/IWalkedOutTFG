using UnityEngine;

public class PulsoManager : MonoBehaviour
{
    public static PulsoManager Instance;

    [Header("Valores de Pulso")]
    public int claro = 0;
    public int quieto = 0;
    public int roto = 0;

    void Awake()
    {
        Instance = this;
    }

    public void ModificarPulso(EstadoPulso estado)
    {
        if (estado == EstadoPulso.Claro) claro++;
        else if (estado == EstadoPulso.Quieto) quieto++;
        else if (estado == EstadoPulso.Roto) roto++;
    }

    public EstadoPulso GetEstadoFinal()
    {
        if (claro >= quieto && claro >= roto) return EstadoPulso.Claro;
        else if (quieto >= claro && quieto >= roto) return EstadoPulso.Quieto;
        else return EstadoPulso.Roto;
    }
}