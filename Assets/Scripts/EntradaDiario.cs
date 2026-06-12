using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntradaDiario
{
    public string titulo;
    public string descripcion;
    public Sprite imagen;
    public string escena; // "Cocina" o "Padre"
    public bool esPista; // true = pista real, false = solo narrativa
}
