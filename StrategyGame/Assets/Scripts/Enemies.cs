using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    private Mesh pillarMesh;
    private Material unitMat;

    private void Awake()
    {
        unitMat = (Material)Resources.Load("Materials/UnitMaterial");

        pillarMesh = (Mesh)Resources.Load<Mesh>("Models/pillar");
        Unit enemy = ScriptableObject.CreateInstance<Pillar>();
        enemy.setCoords(new HexCoordinates(0, 5));
        enemy.id = 4;
        enemy.meshHeight = ((Mesh)pillarMesh).bounds.size.y;

        // Create GameObject for unit and set up its components
        GameObject o = new GameObject { name = "Pillar" };
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
    }
}
