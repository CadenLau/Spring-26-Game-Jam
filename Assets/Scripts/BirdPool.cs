using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Pool;

public class BirdPool : MonoBehaviour
{

    [SerializeField] GameObject raindropPrefab;
    public List<GameObject> birds;

    void Start()
    {
        birds = new List<GameObject>();
    }
    private GameObject spawnObjFromPool(GameObject prefab, List<GameObject> pool, UnityEngine.Vector2 position,
    Quaternion rotation)
    {
        for (int i = 0; i < birds.Count; i++)
        {
            if (birds[i].activeInHierarchy == false)
            {
                birds[i].transform.position = position;
                birds[i].gameObject.SetActive(true);
                return birds[i];
            }
        }
    
        GameObject r = Instantiate(raindropPrefab, position, rotation);
        birds.Add(r);
        return r;
    }

    public GameObject spawnFromPool(Vector2 position, Quaternion rotation)
    {
        return spawnObjFromPool(raindropPrefab, birds, position, rotation);
    }
}   
