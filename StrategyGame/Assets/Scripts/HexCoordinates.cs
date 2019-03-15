// A data structure for axial coordinates on the hexagonal grid
// Hexagons sit edge to edge on each axis
// All coordinates (X, Y, Z) sum to 0

using UnityEngine;

[System.Serializable]
public struct HexCoordinates
{
    [SerializeField]
    private int x, z;

    public int X
    {
        get
        { return x; }
    }

    public int Z
    {
        get
        { return z; }
    }

    // Only two coordinates needed to establish unique position on grid
    public HexCoordinates(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public static HexCoordinates FromOffsetCoordinates(int x, int z)
    {
        return new HexCoordinates(x - z / 2, z);
    }

    public static HexCoordinates FromPosition(Vector3 position)
    {
        // X-coordinate on grid depends on width of hexagons (inner radius)
        float x = position.x / (HexMetrics.innerRadius * 2f);
        // X up = Y down
        float y = -x;
        // height of click determines Z-coord
        float offset = position.z / (HexMetrics.outerRadius * 3f);
        // Z up = X & Y down
        x -= offset;
        y -= offset;

        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(y);
        int iZ = Mathf.RoundToInt(-x - y);

        // Floating-point rounding errors can happen:

        return new HexCoordinates(iX, iZ);
    }

    public int Y
    {
        get { return -X - Z; }
    }

    public override string ToString()
    {
        return "(" + X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
    }

    public string ToStringOnSeparateLines()
    {
        return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
    }
}