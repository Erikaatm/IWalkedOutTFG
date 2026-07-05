using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveController : MonoBehaviour
{
    public List<int> objetosCogidos = new List<int>();
    public bool dialogoLobbyVisto = false;
    public string escenaAnterior = "";
    public List<int> interrogacionesDesactivadas = new List<int>();

    private PulsoManager pulsoManager;


    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        pulsoManager = FindObjectOfType<PulsoManager>();
        Time.timeScale = 1f;
        LoadGame();
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            escenaActual = SceneManager.GetActiveScene().buildIndex,
            playerPosition = SceneManager.GetActiveScene().name == "Lobby"
                ? GameObject.FindGameObjectWithTag("Player").transform.position
                : CargarPosicionGuardada(),
            objetosCogidos = objetosCogidos,
            pulsoRoto = pulsoManager.roto,
            pulsoQuieto = pulsoManager.quieto,
            pulsoClaro = pulsoManager.claro,
            dialogoLobbyVisto = dialogoLobbyVisto,
            escenaAnterior = escenaAnterior,
            interrogacionesDesactivadas = interrogacionesDesactivadas
        };
        PlayerPrefs.SetString("saveData", JsonUtility.ToJson(saveData));
        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("saveData"))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("saveData"));

            if (SceneManager.GetActiveScene().name == "Lobby")
            {
                Debug.Log("Escena anterior: " + saveData.escenaAnterior);
                Debug.Log("Posición guardada: " + saveData.playerPosition);

                if (saveData.escenaAnterior == "EscenaIntro" || saveData.escenaAnterior == "MenuPrincipal")
                    GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(0.0399f, -8.4663f, 0f);
                else
                    GameObject.FindGameObjectWithTag("Player").transform.position = saveData.playerPosition;
            }

            objetosCogidos = saveData.objetosCogidos ?? new List<int>();
            interrogacionesDesactivadas = saveData.interrogacionesDesactivadas ?? new List<int>();

            ItemWorld[] itemsEnMapa = FindObjectsOfType<ItemWorld>();
            foreach (ItemWorld item in itemsEnMapa)
            {
                if (objetosCogidos.Contains(item.worldID))
                    Destroy(item.gameObject);
            }

            // Desactiva interrogaciones ya vistas
            MonoBehaviour[] todosLosInteractuables = FindObjectsOfType<MonoBehaviour>();
            foreach (MonoBehaviour mb in todosLosInteractuables)
            {
                int id = -1;
                if (mb is PuertaInteractuable p) id = p.interactuableID;
                else if (mb is ObjetoRecuerdo o) id = o.interactuableID;
                else if (mb is AbrirMinijuego a) id = a.interactuableID;

                if (id >= 0 && interrogacionesDesactivadas.Contains(id))
                {
                    Transform interrogacion = mb.transform.Find("interrogacion");
                    if (interrogacion != null) interrogacion.gameObject.SetActive(false);
                }
            }

            if (pulsoManager != null)
            {
                pulsoManager.roto = saveData.pulsoRoto;
                pulsoManager.quieto = saveData.pulsoQuieto;
                pulsoManager.claro = saveData.pulsoClaro;
            }

            dialogoLobbyVisto = saveData.dialogoLobbyVisto;
            escenaAnterior = saveData.escenaAnterior;
        }
        else
        {
            SaveGame();
        }
    }

    private Vector3 CargarPosicionGuardada()
    {
        if (PlayerPrefs.HasKey("saveData"))
        {
            SaveData data = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("saveData"));
            return data.playerPosition;
        }
        return Vector3.zero;
    }

    public void MarcarObjetoCogido(int worldID)
    {
        if (!objetosCogidos.Contains(worldID))
            objetosCogidos.Add(worldID);
    }

    public void MarcarInterrogacionDesactivada(int id)
    {
        if (!interrogacionesDesactivadas.Contains(id))
            interrogacionesDesactivadas.Add(id);
    }
}