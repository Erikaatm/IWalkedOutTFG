using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    public void IrAlMenuPrincipal()
    {
        Time.timeScale = 1f;
        PauseController.SetPause(false);
        if (MenuController.Instance != null)
            MenuController.Instance.menuCanvas.SetActive(false);
        SceneManager.LoadScene(0);
    }
}