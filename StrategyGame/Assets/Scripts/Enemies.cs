using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    private Mesh pillarMesh;
    private Material unitMat;

    public static Unit[] enemies;

    private void Awake()
    {
        unitMat = (Material)Resources.Load("Materials/UnitMaterial");

        pillarMesh = (Mesh)Resources.Load<Mesh>("Models/pillar");
        Unit enemy = ScriptableObject.CreateInstance<Pillar>();
        enemy.setCoords(new HexCoordinates(0, 5));
        enemy.id = 4;
        enemy.meshHeight = ((Mesh)pillarMesh).bounds.size.y;

        // Create GameObject for unit and set up its components
        GameObject o = new GameObject { name = "0Pillar" };
        MeshFilter f = o.AddComponent<MeshFilter>();
        f.mesh = pillarMesh;
        MeshRenderer r = o.AddComponent<MeshRenderer>();
        r.material = unitMat;
        o.AddComponent<MeshCollider>();
        o.gameObject.layer = LayerMask.NameToLayer("Units");

        enemy.alignment = false;

        // Attach GameObject to enemy
        enemy.setObject(o);
        enemy.setPos();

        // Put enemy units into the array
        enemies = new Unit[1];
        enemies[0] = enemy;
    }

    // Find the enemy at the specified coordinates
    public static Unit enemyAtCoords(HexCoordinates coords)
    {
        int len = enemies.Length;
        for(int i = 0; i < len; i++)
        {
            if (enemies[i].getCoords().Equals(coords))
                return enemies[i];
        }
        return null;
    }
}
