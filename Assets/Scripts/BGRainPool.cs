using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Pool;
using System;

public class BGRainPool : MonoBehaviour
{

    [SerializeField] GameObject raindropPrefab;
    public List<GameObject> bgRaindrops;

    void Start()
    {
        bgRaindrops = new List<GameObject>();

    }
    private GameObject spawnObjFromPool(GameObject prefab, List<GameObject> pool, Vector2 position)
    {
        for (int i = 0; i < bgRaindrops.Count; i++)
        {
            if (bgRaindrops[i].activeInHierarchy == false)
            {
                bgRaindrops[i].transform.position = position;
                bgRaindrops[i].gameObject.SetActive(true);
                return bgRaindrops[i];
            }
        }
    
        GameObject r = Instantiate(raindropPrefab, position, Quaternion.identity);
        bgRaindrops.Add(r);
        return r;
    }

    public GameObject spawnFromPool(Vector2 position)
    {
        return spawnObjFromPool(raindropPrefab, bgRaindrops, position);
    }
}   
