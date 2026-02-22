using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Pool;

public class RainPool : MonoBehaviour
{

    [SerializeField] GameObject raindropPrefab;
    public List<GameObject> raindrops;

    void Start()
    {
        raindrops = new List<GameObject>();
    }
    private GameObject spawnObjFromPool(GameObject prefab, List<GameObject> pool, Vector2 position)
    {
        for (int i = 0; i < raindrops.Count; i++)
        {
            if (raindrops[i].activeInHierarchy == false)
            {
                raindrops[i].transform.position = position;
                raindrops[i].gameObject.SetActive(true);
                return raindrops[i];
            }
        }
    
        GameObject r = Instantiate(raindropPrefab, position, Quaternion.identity);
        raindrops.Add(r);
        return r;
    }

    public GameObject spawnFromPool(Vector2 position)
    {
        return spawnObjFromPool(raindropPrefab, raindrops, position);
    }
}   
