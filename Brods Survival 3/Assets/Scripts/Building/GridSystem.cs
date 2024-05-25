using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public float gridSize = 1f;

    // Snap a position to the grid
    public Vector3 SnapToGrid(Vector3 position)
    {
        float snappedX = Mathf.Round(position.x / gridSize) * gridSize;
        float snappedY = Mathf.Round(position.y / gridSize) * gridSize;
        float snappedZ = Mathf.Round(position.z / gridSize) * gridSize;

        return new Vector3(snappedX, snappedY, snappedZ);
    }
}
