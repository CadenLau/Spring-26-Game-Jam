using UnityEngine;

public class Cloud : MonoBehaviour
{   
    [SerializeField] private float spawnRate;
    [SerializeField] private float spawnRateRange;

    // Reference to the raindrop
    public GameObject raindrop;

    private float minX;
    private float maxX;
    private float nextSpawnTime;

    private void Start()
    {
        BoxCollider2D cloudCollider = GetComponent<BoxCollider2D>();
        minX = transform.position.x - cloudCollider.bounds.size.x / 2f; 
        maxX = transform.position.x + cloudCollider.bounds.size.x / 2f; 
        nextSpawnTime = Random.Range(spawnRate - (spawnRateRange / 2), 
            spawnRate + (spawnRateRange / 2));
    }

    private void Update()
    {
        if (Time.time >= nextSpawnTime) 
        {
            nextSpawnTime = Time.time + Random.Range(spawnRate - (spawnRateRange / 2), 
                spawnRate + (spawnRateRange / 2));
            SpawnRaindrop();
        }
    }

    private void SpawnRaindrop()
    {
        // Instantiate the object at the spawner's position and rotation
        Instantiate(raindrop, new Vector2(Random.Range(minX, maxX), transform.position.y), 
            transform.rotation);

        // You can store the newly created object in a variable to access its components
        // GameObject spawnedObject = Instantiate(objectToSpawnPrefab, transform.position, transform.rotation);
        // spawnedObject.GetComponent<Rigidbody>().velocity = Vector3.forward * speed;
    }
}