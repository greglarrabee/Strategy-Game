// The generic Unit class
using UnityEngine;

public class Unit : ScriptableObject
{
    // The unit's position
    public HexCoordinates coords = new HexCoordinates(0, 0);
    protected Vector2 planePos;
    // Unit's stats
    public int moveRange { get; protected set; }
    public int atkRangeMin { get; protected set; }
    public int atkRangeMax { get; protected set; }
    public int maxHealth { get; protected set; }
    // Unit's mesh
    public Mesh mesh;
    float meshHeight;
    // Status
    public int health { get; protected set; }
    public bool selected = false;
    // The actual GameObject
    GameObject obj;

    public int id;
}
