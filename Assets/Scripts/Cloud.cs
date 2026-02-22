using System;
using System.Collections.Generic;
using DigitalRuby.LightningBolt;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Cloud : MonoBehaviour
{   
    [SerializeField] private float rainSpawnRate;
    [SerializeField] private float rainSpawnRateRange;
    [SerializeField] private float lightningSpawnRate;
    [SerializeField] private float lightningSpawnRateRange;
    [SerializeField] private float lightningEndY;
    [SerializeField] private float warningWidth;
    [SerializeField] private float warningDelay;

    private List<float> sections = new() {0f, 1f, 2f};
    private static readonly List<float> allSections = new() {0f, 1f, 2f};
    // private bool balancedRain = true; // whether to spawn raindrops in a balanced way across the cloud 
    // [SerializeField] private float unbalancedRainSpawnRate = 0.2f;

    [SerializeField] private GameObject raindrop;
    [SerializeField] private GameObject rainPool;
    [SerializeField] private GameObject lightning;
    [SerializeField] private GameObject warningBox;

    private float minX;
    private float maxX;
    private float nextRainSpawnTime;
    private float nextLightningSpawnTime;

    private BirdPool poolScript;

    private void Start()
    {
        poolScript = rainPool.GetComponent<BirdPool>();
        BoxCollider2D cloudCollider = GetComponent<BoxCollider2D>();
        minX = transform.position.x - cloudCollider.bounds.size.x / 2f; 
        maxX = transform.position.x + cloudCollider.bounds.size.x / 2f; 
        nextRainSpawnTime = UnityEngine.Random.Range(rainSpawnRate - (rainSpawnRateRange / 2), 
            rainSpawnRate + (rainSpawnRateRange / 2));
    }

    private void Update()
    {
        if (Time.time >= nextRainSpawnTime) 
        {
            nextRainSpawnTime = Time.time + UnityEngine.Random.Range(rainSpawnRate - (rainSpawnRateRange / 2), 
                rainSpawnRate + (rainSpawnRateRange / 2));
            SpawnRaindrop();
        }
        if (Time.time >= nextLightningSpawnTime) 
        {
            nextLightningSpawnTime = Time.time + UnityEngine.Random.Range(lightningSpawnRate - 
            (lightningSpawnRateRange / 2), 
                lightningSpawnRate + (lightningSpawnRateRange / 2));
            SpawnLightning();
        }
    }

    private void SpawnRaindrop()
    {
        // Debug.Log("Spawn rate: " + rainSpawnRate);
        // if (!balancedRain)
        // {
        //     // Debug.Log("Spawning unbalanced raindrop");
        //     Instantiate(raindrop, new Vector2(UnityEngine.Random.Range(minX, maxX), transform.position.y), transform.rotation);
        //     return;
        // }

        float width = Math.Abs(maxX - minX);
        if (sections.Count == 0) 
        {
            sections = new(allSections);
        }
        int getSection = UnityEngine.Random.Range(0, sections.Count);
        float section = sections[getSection]; // randomly select a section and remove it from the list
        sections.RemoveAt(getSection);
        
        // Instantiate the object at the spawner's position and rotation
        // Instantiate(raindrop, new Vector2(UnityEngine.Random.Range(minX + width * (section / allSections.Count), minX + width * ((section + 1) / allSections.Count)), transform.position.y), 
        //     transform.rotation);
        poolScript.spawnFromPool(new Vector2(UnityEngine.Random.Range(minX + width * (section / allSections.Count), minX + width * ((section + 1) / allSections.Count)), transform.position.y), Quaternion.identity);
    }

    private async void SpawnLightning()
    {   
        GameObject indicator = Instantiate(warningBox, Vector2.zero, transform.rotation);
        float minLightningX = UnityEngine.Random.Range(minX, maxX);
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

    // public void SetUnbalancedRain()
    // {
    //     balancedRain = false;
    //     rainSpawnRate = unbalancedRainSpawnRate;
    // }
}