// Creates and manages all the units on the board
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitHandler : MonoBehaviour
{
    //
    public static int selected { get; private set; }
    private static Unit[] units;
    // Unit graphics stuff
    private Hashtable meshes;
    private Material unitMat;
    // UI elements
    public static GameObject uiPanel;
    public Text unitText;
    public Text hpText;
    // yeah
    public int numUnits;
    // Input
    public Button moveButton;
    public Button marchButton;
    public Button doneButton;
    // To keep track of how to interpret mouse clicks
    private enum inputState
    {
        READY,
        MOVE
    };
    private static inputState state;

    // Initialize some constants, set up the board
    void Awake()
    {
        meshes = new Hashtable();
        meshes.Add("sword", Resources.Load("Models/sword", typeof(Mesh)));
        meshes.Add("bow", Resources.Load("Models/arrow", typeof(Mesh)));
        meshes.Add("spear", Resources.Load("Models/spear",typeof(Mesh)));
        unitMat = (Material)Resources.Load("Materials/UnitMaterial");
        units = new Unit[numUnits];
        // Add units
        initUnit("sword", new HexCoordinates(0, 0), 0);
        initUnit("bow", new HexCoordinates(1, 0), 1);
        initUnit("spear", new HexCoordinates(2, 2), 2);
        selected = -1;
        // Prepare buttons
        moveButton.onClick.AddListener(moveCheck);
        moveButton.gameObject.SetActive(false);
        marchButton.onClick.AddListener(march);
        marchButton.gameObject.SetActive(false);
        /*doneButton.onClick.AddListener(endTurn);
        doneButton.gameObject.SetActive(false);*/
        // initialize input
        state = inputState.READY;
    }
    /*
    public void giveButtons(Button move, Button marchb, Button done)
    {
        moveButton = move;
        marchButton = marchb;
        doneButton = done;
        moveButton.onClick.AddListener(moveCheck);
        moveButton.gameObject.SetActive(false);
        marchButton.onClick.AddListener(march);
        marchButton.gameObject.SetActive(false);
        doneButton.onClick.AddListener(endTurn);
        doneButton.gameObject.SetActive(false);
    }*/

    // Create a new Unit of a certain type at the desired coordinates
    public void initUnit(string kind, HexCoordinates c, int index)
    {
        Unit newUnit;
        switch(kind)
        {
            case "sword":
                newUnit = ScriptableObject.CreateInstance<Sword>();
                break;
            case "bow":
                newUnit = ScriptableObject.CreateInstance<Bow>();
                break;
            case "spear":
                newUnit = ScriptableObject.CreateInstance<Spear>();
                break;
            default:
                return;
        }
        // Create instance of unit class and set its info
        newUnit.setCoords(c);
        newUnit.id = index;
        newUnit.meshHeight = ((Mesh)meshes[kind]).bounds.size.y;

        // Create GameObject for unit and set up its components
        GameObject o = new GameObject { name = kind + "Unit" + index };
        MeshFilter f = o.AddComponent<MeshFilter>();
        f.mesh = (Mesh)meshes[kind];
        MeshRenderer r = o.AddComponent<MeshRenderer>();
        r.material = unitMat;
        o.AddComponent<MeshCollider>();
        o.gameObject.layer = LayerMask.NameToLayer("Units");

        // Attach GameObject to Sword instance
        units[index] = newUnit;
        newUnit.setObject(o);
        newUnit.setPos();
    }

    // Wait for user's input and move unit
    void moveCheck()
    {
        if(selected != -1 && !units[selected].moved)
        {
            state = inputState.MOVE;
        }
    }
    /*
    void endTurn()
    {
        TurnHandler.playerEndTurn();
    }*/

    // Move a unit to the target coordinates
    public static void moveUnit(HexCoordinates dest)
    {
        if(state == inputState.MOVE && selected != -1)
        {
            state = inputState.READY;
            units[selected].setCoords(dest);
            units[selected].setPos();
            units[selected].moved = true;
        }
    }

    // I know, I know
    private void march()
    {
        StartCoroutine(actuallyMarch());
    }

    // Move the unit in a circle, pausing between steps
    private IEnumerator actuallyMarch()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        for(int i = 0; i < 6; i++)
        {
            HexCoordinates next = HexCoordinates.translate(units[selected].getCoords(), i);
            units[selected].setCoords(next);
            units[selected].setPos();
            yield return wait;
        }
    }

    private void Update()
    {
        playerInput();
    }

    // Check for input relevant to units
    public void playerInput()
    {
        // Check for a click
        if(Input.GetMouseButtonDown(0) && state == inputState.READY)
        {
            //Debug.Log(state);
            handleClick();
        }
    }
    /*
    // Activate or deactivate the UI
    public static void setUIvis(bool active)
    {
        uiPanel.SetActive(active);
    }*/

    // Check to see if the user's click was on an object, and select that object if so
    void handleClick()
    {
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
            moveButton.gameObject.SetActive(true);
            marchButton.gameObject.SetActive(true);
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
            moveButton.gameObject.SetActive(false);
            marchButton.gameObject.SetActive(false);
            unitText.text = "No unit selected";
            hpText.text = "";
        }
    }
}