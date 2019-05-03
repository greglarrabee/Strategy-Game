// The object that populates the grid
using UnityEngine;

public class HexCell : MonoBehaviour
{
    public HexCoordinates coordinates;

    public Color color;

    public HexGrid.Terrain terrain;
    public HexGrid.Status status;

    public bool found;

    public int[] path;
    public bool occupied;

    int elevation;

    public int Elevation
    {
        get
        {
            return (elevation);
        }
        set
        {
            elevation = value;
            Vector3 position = transform.localPosition;
            position.y = value * HexMetrics.elevationStep;
            transform.localPosition = position;
        }
    }
}
