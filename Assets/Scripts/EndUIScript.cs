using UnityEngine;

public class EndUIScript : MonoBehaviour
{
    [SerializeField] private GameObject endPanel;
    [SerializeField] private PlayerScript playerScript;

    private void Start()
    {
        endPanel.SetActive(false);
    }

    private void Update()
    {
        if (playerScript.enabled == false && playerScript.Input.actions["Jump"].IsPressed())
        {
            // Reload the current scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }

    public void ShowEndScreen()
    {
        endPanel.SetActive(true);

        // Time.timeScale = 0f; // pause game
    }
}
