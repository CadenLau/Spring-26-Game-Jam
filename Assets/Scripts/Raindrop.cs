using UnityEngine;

public class Raindrop : MonoBehaviour
{
    [SerializeField] private string groundLayerTag; 

    [SerializeField] private float fallSpeed;

    private void Update()
    {
        transform.Translate(fallSpeed * Time.deltaTime * Vector3.down);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(groundLayerTag))
        {
            Destroy(gameObject);
        }
    }

}
