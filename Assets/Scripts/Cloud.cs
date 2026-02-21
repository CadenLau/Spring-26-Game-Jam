using DigitalRuby.LightningBolt;
using UnityEngine;

public class Cloud : MonoBehaviour
{   
    [SerializeField] private float rainSpawnRate;
    [SerializeField] private float rainSpawnRateRange;
    [SerializeField] private float lightningSpawnRate;
    [SerializeField] private float lightningSpawnRateRange;
    [SerializeField] private float lightningEndY;
    [SerializeField] private float warningWidth;
    [SerializeField] private float warningDelay;


    // Reference to the raindrop
    public GameObject raindrop;

    // Reference to the lightning
    public GameObject lightning;

    public GameObject warningBox;

    private float minX;
    private float maxX;
    private float nextRainSpawnTime;
    private float nextLightningSpawnTime;

    private void Start()
    {
        BoxCollider2D cloudCollider = GetComponent<BoxCollider2D>();
        minX = transform.position.x - cloudCollider.bounds.size.x / 2f; 
        maxX = transform.position.x + cloudCollider.bounds.size.x / 2f; 
        nextRainSpawnTime = Random.Range(rainSpawnRate - (rainSpawnRateRange / 2), 
            rainSpawnRate + (rainSpawnRateRange / 2));
    }

    private void Update()
    {
        if (Time.time >= nextRainSpawnTime) 
        {
            nextRainSpawnTime = Time.time + Random.Range(rainSpawnRate - (rainSpawnRateRange / 2), 
                rainSpawnRate + (rainSpawnRateRange / 2));
            SpawnRaindrop();
        }
        if (Time.time >= nextLightningSpawnTime) 
        {
            nextLightningSpawnTime = Time.time + Random.Range(lightningSpawnRate - 
            (lightningSpawnRateRange / 2), 
                lightningSpawnRate + (lightningSpawnRateRange / 2));
            SpawnLightning();
        }
    }

    private void SpawnRaindrop()
    {
        // Instantiate the object at the spawner's position and rotation
        Instantiate(raindrop, new Vector2(Random.Range(minX, maxX), transform.position.y), 
            transform.rotation);
    }

    private async void SpawnLightning()
    {   
        GameObject indicator = Instantiate(warningBox, Vector2.zero, transform.rotation);
        float minLightningX = Random.Range(minX, maxX);
        float maxLightningX = minLightningX;

        DrawBox indicatorScript = indicator.GetComponent<DrawBox>();
        indicatorScript.minX = minLightningX - warningWidth / 2;
        indicatorScript.maxX = minLightningX + warningWidth / 2;
        indicatorScript.minY = lightningEndY;
        indicatorScript.maxY = transform.position.y;

        await Awaitable.WaitForSecondsAsync(warningDelay);

        if (this == null) return; // object was destroyed

        // Instantiate the object at the spawner's position and rotation
        GameObject newLightning = Instantiate(lightning, Vector2.zero, transform.rotation);
        LightningBoltScript lightningScript = newLightning.GetComponent<LightningBoltScript>();
  
        lightningScript.start = new Vector2(minLightningX, transform.position.y);
        lightningScript.end = new Vector2(maxLightningX, lightningEndY);
    }
}