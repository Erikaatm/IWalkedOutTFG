using UnityEngine;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance;
    public GameObject menuCanvas;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        menuCanvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!menuCanvas.activeSelf && PauseController.IsGamePaused)
                return;
            menuCanvas.SetActive(!menuCanvas.activeSelf);
            PauseController.SetPause(menuCanvas.activeSelf);
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void Salir()
    {
        menuCanvas.SetActive(false);
        Time.timeScale = 1f;
        PauseController.SetPause(false);
    }
}