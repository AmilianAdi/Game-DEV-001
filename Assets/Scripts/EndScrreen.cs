using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScrreen : MonoBehaviour
{
    public static EndScrreen Instance;

    [Header("Panels")]
    public GameObject victoryPanel;
    public GameObject defeatPanel;
    public GameObject hud;
    public GameObject upgradePanel;

    private void Awake()
    {
        Instance = this;

        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        if (defeatPanel != null)
            defeatPanel.SetActive(false);
    }
    public void ShowVictory()
    {
        Time.timeScale = 0f;
        if (hud != null)
            hud.SetActive(false);
        if (upgradePanel != null)
            upgradePanel.SetActive(false);
        if (defeatPanel != null)
            defeatPanel.SetActive(false);
        if (victoryPanel != null)
            victoryPanel.SetActive(true);
    }
    public void ShowDefeat()
    {
        Time.timeScale = 0f;

        if (hud != null)
            hud.SetActive(false);

        if (upgradePanel != null)
            upgradePanel.SetActive(false);

        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        if (defeatPanel != null)
            defeatPanel.SetActive(true);
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
    private void Update()
    {
        if (defeatPanel != null && defeatPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(0))
            {
                RestartGame();
            }
        }
        if (victoryPanel != null && victoryPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
            }
        }
    }
}
