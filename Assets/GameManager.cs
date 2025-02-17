using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    private void Awake()
    {
        gm = this;
    }

    public void PauseGame(bool showPauseScreen = false) 
    {
        Time.timeScale = Time.timeScale == 1 ? 0 : 1;
        Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = !Cursor.visible;
        if(showPauseScreen)
            UIManager.Instance.TogglePauseScreen();
    }

    public void RestartGame() 
    {
        PauseGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu() 
    {
        PauseGame();
        SceneManager.LoadScene(0);
    }

    public void QuitGame() 
    {
        Application.Quit();
    }
}
