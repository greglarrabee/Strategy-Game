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

    int elevation;

    [SerializeField]
    HexCell[] neighbors;

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

    public HexCell getNeighbor(HexDirection direction)
    {
        return neighbors[(int)direction];
    }

    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }



}
