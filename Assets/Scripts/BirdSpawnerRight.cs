using UnityEngine;

public class BirdSpawnerRight : MonoBehaviour
{
    [SerializeField] private float spawnX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    [SerializeField] private float spawnRate;
    [SerializeField] private float spawnRateRange;

    public GameObject bird;

    private float nextSpawnTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            nextSpawnTime = Time.time + Random.Range(spawnRate - (spawnRateRange / 2), 
                spawnRate + (spawnRateRange / 2));
            SpawnBird();
        }
    }

    void SpawnBird()
    {
        Instantiate(bird, new Vector2(spawnX, Random.Range(minY, maxY)), 
            Quaternion.Euler(0, 0, 0));
    }
}
