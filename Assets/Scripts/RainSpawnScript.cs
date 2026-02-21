using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RainSpawnScript : MonoBehaviour
{
    [SerializeField] private float spawnRate;
    [SerializeField] private float spawnRateRange;

    public GameObject raindrop;

    private float minX;
    private float maxX;
    private float nextSpawnTime;

    private void Start()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        minX = collider.bounds.min.x; 
        maxX = collider.bounds.max.x; 
        nextSpawnTime = Random.Range(spawnRate - spawnRateRange, 
            spawnRate + spawnRateRange);
    }

    private void Update()
    {
        if (Time.time >= nextSpawnTime) 
        {
            nextSpawnTime = Time.time + Random.Range(spawnRate - spawnRateRange, 
                spawnRate + spawnRateRange);
            SpawnRaindrop();
        }
    }

    private void SpawnRaindrop()
    {
        Instantiate(raindrop, new Vector2(Random.Range(minX, maxX), transform.position.y), 
            transform.rotation);
    }
}
