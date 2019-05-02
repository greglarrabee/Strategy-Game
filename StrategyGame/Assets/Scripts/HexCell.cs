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
}
