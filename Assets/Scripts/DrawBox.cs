using UnityEngine;

public class DrawBox : MonoBehaviour
{
    private LineRenderer lineRenderer;

    [SerializeField] private float lifetime;
    public float minX;
    public float maxX;
    public float maxY;
    public float minY;

    void Start()
    {
        Destroy(gameObject, lifetime);
        lineRenderer = GetComponent<LineRenderer>();
        // Set the number of points for the box
        lineRenderer.positionCount = 4; 
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        
        // Define the corner points of the box
        Vector3 corner1 = new Vector3(minX, minY, 0);
        Vector3 corner2 = new Vector3(minX, maxY, 0);
        Vector3 corner3 = new Vector3(maxX, maxY, 0);
        Vector3 corner4 = new Vector3(maxX, minY, 0);

        // Assign the points to the Line Renderer
        lineRenderer.SetPosition(0, corner1);
        lineRenderer.SetPosition(1, corner2);
        lineRenderer.SetPosition(2, corner3);
        lineRenderer.SetPosition(3, corner4);
    }
}

