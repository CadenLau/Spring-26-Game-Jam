using UnityEngine;

public class LoadingScript : MonoBehaviour
{
    [SerializeField] private PlayerScript playerScript;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private float loadingTime = 4f;

    // private void Awake()
    // {
    //     playerScript.enabled = false;
    // }
    
    private void Update()
    {
        loadingTime -= Time.deltaTime;
        if (loadingTime <= 0)
        {
            playerScript.enabled = true;
            loadingScreen.SetActive(false);
            this.enabled = false;
        }
    }
}
