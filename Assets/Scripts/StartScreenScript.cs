using UnityEngine;

public class StartScreenScript : MonoBehaviour
{
    [SerializeField] private PlayerScript playerScript;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject startScreen;
    [SerializeField] private Cloud cloudScript;

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
        } else if (playerScript.Input.actions["Unbalanced"].IsPressed())
        {
            cloudScript.SetUnbalancedRain();
            loadingScreen.SetActive(true);
            loadingScreen.GetComponent<LoadingScript>().enabled = true;
            startScreen.SetActive(false);
            this.enabled = false;
        }
    }
}
