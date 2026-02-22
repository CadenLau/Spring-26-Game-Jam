using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float leftBound;
    [SerializeField] private float rightBound;

    private Vector3 moveDirection;

    void Start()
    {
        if (transform.rotation.eulerAngles.y == 180)
        {
            moveDirection = new Vector2 (1, 0);
        }
        else
        {
            moveDirection = new Vector2 (-1, 0);
        }
    }

    void Update()
    {
        if (transform.position.x < leftBound  || transform.position.x > rightBound)
        {
            // Destroy(gameObject);
            gameObject.SetActive(false);
        }
        transform.position += moveDirection * speed * Time.deltaTime;
    }
}
