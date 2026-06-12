using System.Collections.Generic;
using UnityEngine;

public class DiarioManager : MonoBehaviour
{
    public static DiarioManager Instance { get; private set; }

    private List<EntradaDiario> entradas = new List<EntradaDiario>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void AgregarEntrada(EntradaDiario entrada)
    {
        if (!entradas.Exists(e => e.titulo == entrada.titulo))
            entradas.Add(entrada);
    }

    public List<EntradaDiario> GetEntradasPorEscena(string escena)
    {
        return entradas.FindAll(e => e.escena == escena);
    }

    public List<EntradaDiario> GetTodasEntradas()
    {
        return entradas;
    }
}