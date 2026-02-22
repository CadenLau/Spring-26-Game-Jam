using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RainSpawnScript : MonoBehaviour
{
    [SerializeField] private float spawnRate;
    [SerializeField] private float spawnRateRange;

    public GameObject raindrop;

    public GameObject rainPool;

    private BirdPool poolScript;

    private float minX;
    private float maxX;
    private float spawnTimer;

    private void Start()
    {
        poolScript = rainPool.GetComponent<BirdPool>();
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        minX = collider.bounds.min.x; 
        maxX = collider.bounds.max.x; 
        ResetSpawnTimer();
    }

    private void FixedUpdate()
    {
        spawnTimer -= Time.fixedDeltaTime;

        if (spawnTimer <= 0) 
        {
            ResetSpawnTimer();
            SpawnRaindrop();
        }
    }

    private void ResetSpawnTimer()
    {
        spawnTimer = Random.Range(spawnRate - spawnRateRange, 
            spawnRate + spawnRateRange);
    }

    private void SpawnRaindrop()
    {
        // Instantiate(raindrop, new Vector2(Random.Range(minX, maxX), transform.position.y), 
        //     transform.rotation);
        poolScript.spawnFromPool(new Vector2(Random.Range(minX, maxX), transform.position.y), Quaternion.identity);

    }
}
