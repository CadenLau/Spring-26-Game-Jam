using UnityEngine;

public class StartScreenScript : MonoBehaviour
{
    [SerializeField] private PlayerScript playerScript;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject startScreen;
    // [SerializeField] private Cloud cloudScript;
    [SerializeField] private float timeBeforeStart = 5f;
    private float timer = 0f;

    private void Awake()
    {
        playerScript.enabled = false;
        loadingScreen.GetComponent<LoadingScript>().enabled = false;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > timeBeforeStart || playerScript.Input.actions["Start"].WasPressedThisFrame())
        {
            loadingScreen.SetActive(true);
            loadingScreen.GetComponent<LoadingScript>().enabled = true;
            startScreen.SetActive(false);
            this.enabled = false;
        } 
        // else if (playerScript.Input.actions["Unbalanced"].WasPressedThisFrame())
        // {
        //     cloudScript.SetUnbalancedRain();
        //     loadingScreen.SetActive(true);
        //     loadingScreen.GetComponent<LoadingScript>().enabled = true;
        //     startScreen.SetActive(false);
        //     this.enabled = false;
        // }
    }
}
