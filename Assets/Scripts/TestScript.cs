using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] private Transform tf;
    private float timer;

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;

        if (timer % 2f < 1f)
        {
            tf.position += Vector3.right * 0.1f;
        } else
        {
            tf.position += Vector3.left * 0.1f;
        }
    }
}
