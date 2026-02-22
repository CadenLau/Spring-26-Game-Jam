using UnityEngine;

public class BackgroundRainScript : MonoBehaviour
{
    [SerializeField] private float fallRate;
    [SerializeField] private float xDestroyBound = -30f;
    [SerializeField] private float yDestroyBound = -5f;
    
    private void Update()
    {
        // Destroy the raindrop if it goes off-screen to the left or in ground
        if (transform.position.x < xDestroyBound || transform.position.y < yDestroyBound)
        {
            // Destroy(gameObject);
            gameObject.SetActive(false);
        }

        transform.Translate(fallRate * Time.deltaTime * new Vector3(-0.1f, -1, 0));
    }
}
