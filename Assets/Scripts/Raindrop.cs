using System;
using UnityEngine;

public class Raindrop : MonoBehaviour
{
    [SerializeField] private String groundLayerTag; 

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(groundLayerTag))
        {
            Destroy(gameObject);
        }
    }

}
