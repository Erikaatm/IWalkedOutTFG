using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    private string saveLocation;
    public GameObject blackPanel;

    void Start()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
    }

    public void JugarNuevo()
    {
        StartCoroutine(FadeYJugar(true));
    }

    public void Continuar()
    {
        StartCoroutine(FadeYJugar(false));
    }

    IEnumerator FadeYJugar(bool esNuevo)
    {
        UnityEngine.UI.Image fondo = blackPanel.GetComponent<UnityEngine.UI.Image>();
        Color color = fondo.color;
        color.a = 0f;
        fondo.color = color;
        blackPanel.SetActive(true);

        while (color.a < 1f)
        {
            color.a += Time.deltaTime * 1.5f;
            fondo.color = color;
            yield return null;
        }

        if (esNuevo)
        {
            SaveData dataReset = new SaveData
            {
                escenaActual = 1,
                playerPosition = new Vector3(0.0399f, -8.4663f, 0.0f),
                hotbarSaveData = new List<InventorySaveData>(),
                objetosCogidos = new List<int>(),
                objetosColocados = new List<ObjetoColocadoData>(),
                zonasUsadas = new List<int>(),
                pulsoRoto = 0,
                pulsoQuieto = 0,
                pulsoClaro = 0,
                introVista = false,
                dialogoLobbyVisto = false,
                escenaAnterior = "MenuPrincipal",
                interrogacionesDesactivadas = new List<int>(),
            };
            File.WriteAllText(saveLocation, JsonUtility.ToJson(dataReset, true));
            SceneManager.LoadScene(1);
        }
        else
        {
            if (File.Exists(saveLocation))
            {
                Time.timeScale = 1f;
                SaveData data = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));
                SceneManager.LoadScene(data.escenaActual);
            }
            else
            {
                SceneManager.LoadScene(1);
            }
        }
    }

    public void Salir()
    {
        Application.Quit();
    }

    public void IrACreditos()
    {
        SceneManager.LoadScene("Creditos");
    }
}