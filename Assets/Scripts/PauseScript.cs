using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PauseScript : MonoBehaviour
{    
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private PlayerScript playerScript;
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject loadingScreen;

    public static bool IsPaused = false;

    private void Start()
    {
        pauseMenuUI.SetActive(false);
        IsPaused = false;
    }

    private void Update()
    {
        if (startScreen.activeSelf || loadingScreen.activeSelf) return;

        if (playerScript.Input.actions["Pause"].WasPressedThisFrame())
        {
            if (IsPaused) Resume();
            else Pause();
        } else if (IsPaused && playerScript.Input.actions["Quit"].WasPressedThisFrame())
        {
            QuitGame();
        }
    }

    private void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
        playerScript.enabled = true;
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
        playerScript.enabled = false;
    }

    private void QuitGame()
    {
        Time.timeScale = 1f;
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}

