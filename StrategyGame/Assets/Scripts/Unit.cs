// The generic Unit class
using UnityEngine;

public class Unit : ScriptableObject
{
    // The unit's position
    private HexCoordinates coords = new HexCoordinates(0, 0);
    protected Vector2 planePos;
    // Unit's stats
    public int moveRange { get; protected set; }
    public int atkRange { get; protected set; }
    public int maxHealth { get; protected set; }
    // Unit's mesh height (for positioning)
    public float meshHeight;
    // Status
    public int health { get; protected set; }
    public bool moved;
    public bool alignment;
    // The actual GameObject
    GameObject obj;

    public int id;

    // Place unit at correct world place
    public void setPos()
    {
        Vector2 xzPos = HexCoordinates.fromHexCoordinates(coords);
        obj.transform.SetPositionAndRotation(new Vector3(xzPos[0], meshHeight / 2, xzPos[1]), Quaternion.identity);
    }

    // Give unit a GameObject to handle
    public void setObject(GameObject o)
    {
        obj = o;
    }

    public void setCoords(HexCoordinates c)
    {
        coords = c;
    }

    public HexCoordinates getCoords()
    {
        return coords;
    }
}
