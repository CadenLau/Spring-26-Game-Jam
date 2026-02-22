using UnityEngine;

public class StartScreenScript : MonoBehaviour
{
    [SerializeField] private PlayerScript playerScript;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject startScreen;

    private void Awake()
    {
        playerScript.enabled = false;
        loadingScreen.GetComponent<LoadingScript>().enabled = false;
    }

    void Update()
    {
        if (playerScript.Input.actions["Start"].IsPressed())
        {
            loadingScreen.SetActive(true);
            loadingScreen.GetComponent<LoadingScript>().enabled = true;
            startScreen.SetActive(false);
            this.enabled = false;
        }
    }
}
