using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    public void IrAlMenuPrincipal()
    {
        Time.timeScale = 1f;
        PauseController.SetPause(false);
        if (MenuController.Instance != null)
        {
            MenuController.Instance.menuCanvas.SetActive(false);
            Destroy(MenuController.Instance.gameObject);
        }
        SceneManager.LoadScene(0);
    }
}