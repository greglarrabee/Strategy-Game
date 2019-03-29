// Creates and manages all the units on the board
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitHandler : MonoBehaviour
{
    private enum inputState
    {
        READY,
        MOVE
    };
    private static inputState state;
    private static int selected;
    private static Unit[] units;
    private Mesh swordMesh;
    public Text unitText;
    public Text hpText;
    public int numUnits;
    public Button moveButton;

    // Initialize some constants, set up the board
    void Awake()
    {
        swordMesh = (Mesh)Resources.Load("Models/sword",typeof(Mesh));
        units = new Unit[numUnits];
        // Add units
        for(int i = 0; i < numUnits; i++)
        {
            initS(new HexCoordinates(i, 0), swordMesh, i);
        }
        selected = -1;
        // Prepare buttons
        moveButton.onClick.AddListener(moveCheck);
        // initialize input
        state = inputState.READY;
    }

    // Create a new Sword object at the desired hex coordinates
    public void initS(HexCoordinates c, Mesh m, int index)
    {
        // Create instance of Sword class and set its info
        Sword s = ScriptableObject.CreateInstance<Sword>();
        s.setCoords(c);
        s.id = index;
        s.meshHeight = swordMesh.bounds.size.y;

        // Create GameObject for Sword unit and set up its components
        GameObject o = new GameObject { name = "SwordUnit" + index };
        MeshFilter f = o.AddComponent<MeshFilter>();
        f.mesh = swordMesh;
        MeshRenderer r = o.AddComponent<MeshRenderer>();
        r.material = new Material(Shader.Find("Diffuse"));
        o.AddComponent<MeshCollider>();
        o.gameObject.layer = LayerMask.NameToLayer("Units");

        // Attach GameObject to Sword instance
        units[index] = s;
        s.setObject(o);
    }
    
    public static int getSelectedId()
    {
        return selected;
    }

    // Wait for user's input and move unit
    void moveCheck()
    {
        state = inputState.MOVE;
    }

    // Move a unit to the target coordinates
    public static void moveUnit(HexCoordinates dest)
    {
        if(state == inputState.MOVE && selected != -1)
        {
            state = inputState.READY;
            units[selected].setCoords(dest);
        }
    }

    // Check for input relevant to units
    void Update()
    {
        // Check for a click
        if(Input.GetMouseButtonDown(0) && state == inputState.READY)
        {
            //Debug.Log(state);
            handleClick();
        }
        // Update unit positions
        for(int i = 0; i < numUnits; i++)
        {
            units[i].setPos();
        }
    }

    // Check to see if the user's click was on an object, and select that object if so
    void handleClick()
    {
        Debug.Log(Input.mousePosition);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Units")))
        {
            string name = hit.collider.gameObject.name;
            selected = name[name.Length - 1] - '0';
            unitText.text = "Selected " + name;
            int hp = units[selected].health;
            int mHP = units[selected].maxHealth;
            hpText.text = "HP: " + hp + "/" + mHP;
            //Debug.Log(name);
        }
        // If mouse click is on UI section of canvas
        else if(Input.mousePosition.y < 100)
        {
            // do nothing
        }
        else
        {
            selected = -1;
            unitText.text = "No unit selected";
            //Debug.Log("no hit");
            hpText.text = "";
        }
    }
}