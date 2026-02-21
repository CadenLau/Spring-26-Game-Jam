using UnityEngine;

public class BackgroundRainScript : MonoBehaviour
{
    [SerializeField] private float fallRate;
    
    private void FixedUpdate()
    {
        // Destroy the raindrop if it goes off-screen to the left or in ground
        if (transform.position.x < -30f || transform.position.y < -5f)
        {
            Destroy(gameObject);
        }

        transform.Translate(fallRate * Time.fixedDeltaTime * new Vector3(-0.1f, -1, 0));
    }
}
