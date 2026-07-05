using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public GameObject blackPanel;

    public void JugarNuevo()
    {
        if (MinijuegoCocina.Instance != null)
            MinijuegoCocina.Instance.ResetMinijuego();
        if (MinijuegoPiano.Instance != null)
            MinijuegoPiano.Instance.ResetMinijuego();
        if (MenuController.Instance != null)
            MenuController.Instance.menuCanvas.SetActive(false);
        SaveController save = FindObjectOfType<SaveController>();
        if (save != null)
            save.dialogoLobbyVisto = false;
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
                objetosCogidos = new List<int>(),
                pulsoRoto = 0,
                pulsoQuieto = 0,
                pulsoClaro = 0,
                introVista = false,
                dialogoLobbyVisto = false,
                escenaAnterior = "MenuPrincipal",
                interrogacionesDesactivadas = new List<int>(),
            };
            PlayerPrefs.SetString("saveData", JsonUtility.ToJson(dataReset));
            PlayerPrefs.Save();
            SceneManager.LoadScene(1);
        }
        else
        {
            if (PlayerPrefs.HasKey("saveData"))
            {
                Time.timeScale = 1f;
                SaveData data = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("saveData"));
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
        gameObject.SetActive(false);
        Application.Quit();
    }

    public void IrACreditos()
    {
        SceneManager.LoadScene("Creditos");
    }
}