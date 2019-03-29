﻿// The generic Unit class
using UnityEngine;

public class Unit : ScriptableObject
{
    // The unit's position
    private HexCoordinates coords = new HexCoordinates(0, 0);
    protected Vector2 planePos;
    // Unit's stats
    public int moveRange { get; protected set; }
    public int atkRangeMin { get; protected set; }
    public int atkRangeMax { get; protected set; }
    public int maxHealth { get; protected set; }
    // Unit's mesh height
    public float meshHeight;
    // Status
    public int health { get; protected set; }
    //public bool selected = false;
    // The actual GameObject
    GameObject obj;

    public int id;

    public void setPos()
    {
        Vector2 xzPos = HexCoordinates.fromHexCoordinates(coords);
        obj.transform.SetPositionAndRotation(new Vector3(xzPos[0], meshHeight / 2, xzPos[1]), Quaternion.identity);
    }

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
