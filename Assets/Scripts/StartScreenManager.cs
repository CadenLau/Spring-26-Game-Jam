using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour
{
    // play button
    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    // quit the game button
    public void QuitGame()
    {
        // exit game
        Application.Quit();
        Debug.Log("Quit clicked (won’t close in Editor)");
    }
}
