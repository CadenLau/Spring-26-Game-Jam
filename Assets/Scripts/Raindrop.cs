using UnityEngine;

public class Raindrop : MonoBehaviour
{
    [SerializeField] private string groundLayerTag; 

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(groundLayerTag))
        {
            Destroy(gameObject);
        }
    }

}
