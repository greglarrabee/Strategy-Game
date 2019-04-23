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

    public int Y
    {
        get { return -X - Z; }
    }

    // Only two coordinates needed to establish unique position on grid
    public HexCoordinates(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    // To find hex coordinates upon grid initialization
    public static HexCoordinates FromOffsetCoordinates(int x, int z)
    {
        return new HexCoordinates(x - z / 2, z);
    }

    // Finds hex coordinates from a world position
    public static HexCoordinates FromPosition(Vector3 position)
    {
        // X-coordinate on grid depends on width of hexagons (inner radius)
        float x = position.x / (HexMetrics.innerRadius * 2f);
        // X up = Y down
        float y = -x;
        // "height" of click determines Z-coord
        float offset = position.z / (HexMetrics.outerRadius * 3f);
        // Z up = X & Y down
        x -= offset;
        y -= offset;

        // coordinates will eventually need to be integers
        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(y);
        int iZ = Mathf.RoundToInt(-x - y);

        // Floating-point rounding errors can happen:
        if (iX + iY + iZ != 0)
        {
            float dX = Mathf.Abs(x - iX);
            float dY = Mathf.Abs(y - iY);
            float dZ = Mathf.Abs(-x - y - iZ);

            // Reconstruct least accurate coordinate
            if (dX > dY && dX > dZ)
            {
                iX = -iY - iZ;
            }
            else if(dZ > dY)
            {
                iZ = -iX - iY;
            }
        }

        return new HexCoordinates(iX, iZ);
    }

    // Converts HexCoordinates to actual 2D positions
    public static Vector2 fromHexCoordinates(HexCoordinates coords)
    {
        float cX = coords.X;
        float cZ = coords.Z;
        float x = (cX * 2 + cZ) * HexMetrics.innerRadius;
        float z = cZ * (1.5f * HexMetrics.outerRadius);
        return new Vector2(x, z);
    }

    // Returns a new HexCoordinates one tile away in the specified direction
    // Directions start with due east and go clockwise
    public static  HexCoordinates translate(HexCoordinates start, int dir)
    {
        HexCoordinates dXZ;
        switch(dir)
        {
            case 0:
                dXZ = new HexCoordinates(1, 0);
                break;
            case 1:
                dXZ = new HexCoordinates(1, -1);
                break;
            case 2:
                dXZ = new HexCoordinates(0, -1);
                break;
            case 3:
                dXZ = new HexCoordinates(-1, 0);
                break;
            case 4:
                dXZ = new HexCoordinates(-1, 1);
                break;
            case 5:
                dXZ = new HexCoordinates(0, 1);
                break;
            default:
                return start;
        }
        return new HexCoordinates(start.X + dXZ.X, start.Z + dXZ.Z);
    }

    // For internal debugging
    public override string ToString()
    {
        return "(" + X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
    }

    public string ToStringOnSeparateLines()
    {
        return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
    }
}