// Creates and manages all the units on the board
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHandler : MonoBehaviour
{
    public int selected;
    private Unit[] units;
    private GameObject[] unitObjects;
    private Mesh swordMesh;

    // Initialize some constants, set up the board
    void Awake()
    {
        swordMesh = (Mesh)Resources.Load("Models/sword",typeof(Mesh));
        units = new Unit[1];
        unitObjects = new GameObject[1];
        initS(new HexCoordinates(2, 3), swordMesh, 0);
        selected = -1;
    }

    // Create a new Sword object at the desired hex coordinates
    public void initS(HexCoordinates c, Mesh m, int index)
    {
        GameObject o = new GameObject { name = "SwordUnit" + index };
        Sword s = ScriptableObject.CreateInstance<Sword>();
        s.coords = c;
        s.id = index;

        MeshFilter f = o.AddComponent<MeshFilter>();
        f.mesh = swordMesh;

        MeshRenderer r = o.AddComponent<MeshRenderer>();
        r.material = new Material(Shader.Find("Diffuse"));

        o.AddComponent<MeshCollider>();

        float height = swordMesh.bounds.size.y;
        Vector2 coords = HexCoordinates.fromHexCoordinates(s.coords);
        o.transform.SetPositionAndRotation(new Vector3(coords[0], height / 2, coords[1]), Quaternion.identity);

        o.gameObject.layer = LayerMask.NameToLayer("Units");

        units[index] = s;
        unitObjects[index] = o;
    }
    
    
    // Check for input relevant to units
    void Update()
    {
        // Check for a click
        if(Input.GetMouseButtonDown(0))
        {
            handleClick();
        }
    }

    // Check to see if the user's click was on an object, and select that object if so
    void handleClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Units")))
        {
            string name = hit.collider.gameObject.name;
            selected = name[name.Length - 1] - '0';
            //Debug.Log(name);
        }
        else
        {
            selected = -1;
            //Debug.Log("no hit");
        }
    }
}