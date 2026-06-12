using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int escenaActual;
    public Vector3 playerPosition;
    public List<InventorySaveData> hotbarSaveData;
    public List<int> objetosCogidos;
    public List<ObjetoColocadoData> objetosColocados;
    public List<int> zonasUsadas;
    public int pulsoRoto;
    public int pulsoQuieto;
    public int pulsoClaro;
    public bool introVista;
    public bool dialogoLobbyVisto;
    public string escenaAnterior;
    public List<int> interrogacionesDesactivadas;
}