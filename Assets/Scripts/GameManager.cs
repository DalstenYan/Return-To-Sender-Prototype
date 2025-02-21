using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    public UnityEvent gameOverEvent, gameWonEvent;

    private void Awake()
    {
        gm = this;
    }

    public void PauseGame() 
    {
        ToggleTimeAndCursor();
        UIManager.Instance.TogglePauseScreen();
    }

    public void ToggleTimeAndCursor() 
    {
        Time.timeScale = Time.timeScale == 1 ? 0 : 1;
        Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = !Cursor.visible;
    }

    public void RestartGame() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu() 
    {
        if(Time.timeScale == 0)
            ToggleTimeAndCursor();

        SceneManager.LoadScene(0);
    }

    public void GameWon() 
    {
        StartCoroutine(GameWonCoroutine());
    }

    public IEnumerator GameWonCoroutine() 
    {
        yield return new WaitForSeconds(5);
        MainMenu();
    }

    public void QuitGame() => Application.Quit();

    public void GameOver() => gameOverEvent.Invoke();


    public void DestroyAllBullets()
    {
        foreach (var obj in GameObject.FindGameObjectsWithTag("Bullet")) { Destroy(obj); }
    }
}
