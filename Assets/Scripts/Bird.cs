using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float leftBound;
    [SerializeField] private float rightBound;

    private Vector3 moveDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < leftBound  || transform.position.x > rightBound)
        {
            Destroy(gameObject);
        }
        transform.position += moveDirection * speed * Time.deltaTime;
    }
}
