using UnityEngine;

public class EndUIScript : MonoBehaviour
{
    [SerializeField] private GameObject endPanel;
    [SerializeField] private PlayerScript playerScript;

    private void Awake()
    {
        endPanel.SetActive(false);
    }

    private void Update()
    {
        if (endPanel.activeSelf && playerScript.enabled == false && playerScript.Input.actions["Start"].IsPressed())
        {
            // Reload the current scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }

    public void ShowEndScreen()
    {
        endPanel.SetActive(true);
    }
}
