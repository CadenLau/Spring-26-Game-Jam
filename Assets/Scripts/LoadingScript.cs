using UnityEngine;
using UnityEngine.UI;

public class LoadingScript : MonoBehaviour
{
    [SerializeField] private PlayerScript playerScript;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image loadingPlayerImage;
    [SerializeField] private float loadingTime = 4f;
    private float timer = 0f;

    // private void Awake()
    // {
    //     playerScript.enabled = false;
    // }
    
    private void Update()
    {
        timer += Time.deltaTime;
        loadingPlayerImage.fillAmount = timer / loadingTime * 1.2f; // fill a little past 100% for visual effect
        
        if (timer >= loadingTime)
        {
            playerScript.enabled = true;
            loadingScreen.SetActive(false);
            this.enabled = false;
        }
    }
}
