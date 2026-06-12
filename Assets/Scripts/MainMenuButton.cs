using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    public void IrAlMenuPrincipal()
    {
        Time.timeScale = 1f;
        PauseController.SetPause(false);
        SceneManager.LoadScene(0);
    }
}