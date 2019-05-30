// Creates and manages all the units on the board
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitHandler : MonoBehaviour
{
    // whose turn it is
    private bool playerTurn;
    //
    public static int selected { get; private set; }
    private static Unit[] units;
    private static List<HexCoordinates> interactables;
    private static HexCoordinates curDest;
    // Unit graphics stuff
    private Hashtable meshes;
    private Material unitMat;
    // UI elements
    public GameObject uiPanel;
    public Text unitText;
    public Text hpText;
    public Text infoText;
    // yeah
    public int numUnits;
    // Input
    public Button moveButton;
    //public Button marchButton;
    public Button attackButton;
    public Button doneButton;
    // To keep track of how to interpret mouse clicks
    private enum inputState
    {
        READY,
        MOVE,
        ATTACK,
        MOVING
    };
    private static inputState state;

    // Initialize some constants, set up the board
    void Start()
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
        /*marchButton.onClick.AddListener(march);*/
        doneButton.onClick.AddListener(endTurn);
        attackButton.onClick.AddListener(attackCheck);
        setButtonsVis(false);
        // initialize input
        state = inputState.READY;
        playerTurn = true;
    }

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
        newUnit.alignment = true;

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

    void attackCheck()
    {
        if(selected != -1 && !units[selected].acted)
        {
            state = inputState.ATTACK;
            interactables = HexCoordinates.cellSearch(units[selected].atkRange, units[selected].getCoords(), 2);
            // Highlight specific enemy
            int count = interactables.Count;
            for(int i = 0; i < count; i++)
            {
                Enemies.enemyAtCoords(interactables[i]).getObj().layer = LayerMask.NameToLayer("Targetable");
            }
        }
        
    }
    
    // End's the player's turn
    void endTurn()
    {
        selected = -1;
        playerTurn = false;
        setUIvis(false);
        for(int i = 0; i < units.Length; i++)
        {
            units[i].getObj().layer = LayerMask.NameToLayer("Units");
        }
    }

    // Sets things up for player's turn
    void startTurn()
    {
        for(int i = 0; i < numUnits; i++)
        {
            units[i].moved = false;
        }
        playerTurn = true;
        state = inputState.READY;
        setUIvis(true);
    }

    // Move a unit to the target coordinates
    public static void moveUnit(HexCoordinates dest)
    {   
        if(state == inputState.MOVE && selected != -1 && interactables.Contains(dest))
        {
            state = inputState.MOVING;
            curDest = dest;
        }
    }

    // I know, I know
    private void move(int[] steps)
    {
        HexGrid.cellFromHC(units[selected].getCoords()).status = HexGrid.Status.EMPTY;
        StartCoroutine(actuallyMarch(steps));
    }

    // Move the unit in a sequence of steps, pausing between steps
    private IEnumerator actuallyMarch(int[] steps)
    {
        playerTurn = false;
        WaitForSeconds wait = new WaitForSeconds(0.4f);
        for(int i = 0; i < steps.Length; i++)
        {
            HexCoordinates next = HexGrid.cellFromHC(units[selected].getCoords()).getNeighbor((HexDirection)steps[i]).coordinates;
            units[selected].setCoords(next);
            units[selected].setPos();
            yield return wait;
        }
        units[selected].moved = true;
        playerTurn = true;
        if(state == inputState.MOVING)
            state = inputState.READY;
        // All cells moved through will be marked as ALLY thanks to setPos()
        for(int i = 0; i < interactables.Count; i++)
        {
            HexGrid.cellFromHC(interactables[i]).status = HexGrid.Status.EMPTY;
        }
        HexGrid.cellFromHC(curDest).status = HexGrid.Status.ALLY;
    }

    // Currently enemy AI
    private IEnumerator wait()
    {
        WaitForSeconds w = new WaitForSeconds(1f);
        yield return w;
        startTurn();
    }

    private void Update()
    {
        if(playerTurn)
        {
            playerInput();
            if(state == inputState.MOVING)
            {
                move(HexGrid.cellFromHC(curDest).path);
            }
        }
        else
        {
            StartCoroutine(wait());
        }
    }

    // Check for input relevant to units
    public void playerInput()
    {
        // Check for a click
        if(Input.GetMouseButtonDown(0))
        {
            //Debug.Log("click " + state);
            if(state == inputState.READY)
            {
                handleClick();
            }
            if(state == inputState.ATTACK)
            {
                attackClick();
            }
        }
    }
    
    // Activate or deactivate the UI
    private void setUIvis(bool active)
    {
        uiPanel.SetActive(active);
    }

    // Show or hide the unit-specific buttons
    void setButtonsVis(bool active)
    {
        moveButton.gameObject.SetActive(active);
        //marchButton.gameObject.SetActive(active);
        attackButton.gameObject.SetActive(active);
    }

    // Raycasts to see if user's click was on targetable object
    void attackClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Targetable")))
        {
            string name = hit.collider.gameObject.name;
            // Get number of enemy clicked on
            char numChar = name[0];
            int num = numChar - '0';
            Unit enemy = Enemies.enemies[num];
            enemy.getObj().layer = LayerMask.NameToLayer("Units");
            defeat(enemy);
            state = inputState.READY;
        }
    }

    // Check to see if the user's click was on an object, and select that object if so
    void handleClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Units", "Selected")))
        {
            string name = hit.collider.gameObject.name;
            // If object is a player-controlled one
            if(!Char.IsLetter(name[name.ToCharArray().Length-1]))
            {
                // If this unit isn't selected...
                if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Selected"))
                {
                    // Put the old unit back off the selected layer
                    if(selected != -1)
                    {
                        units[selected].getObj().layer = LayerMask.NameToLayer("Units");
                    }
                    // Update the selected index
                    selected = name[name.Length - 1] - '0';
                }
                unitText.text = "Selected " + name;
                int hp = units[selected].health;
                int mHP = units[selected].maxHealth;
                hpText.text = "HP: " + hp + "/" + mHP;
                setButtonsVis(true);
                //Debug.Log(name);
                interactables = HexCoordinates.cellSearch(units[selected].moveRange, units[selected].getCoords(), 0);
                // move unit to highlight layer
                units[selected].getObj().layer = LayerMask.NameToLayer("Selected");
            }
        }
        // If mouse click is on UI section of canvas
        else if(Input.mousePosition.y < 100)
        {
            // do nothing
        }
        else
        {
            if(selected > -1)
                units[selected].getObj().layer = LayerMask.NameToLayer("Units");
            selected = -1;
            setButtonsVis(false);
            unitText.text = "No unit selected";
            hpText.text = "";
        }
    }

    void defeat(Unit defeated)
    {
        Debug.Log("Unit " + defeated.name + " defeated");
        //StartCoroutine(vanish(defeated));
        Destroy(defeated.getObj());
        Destroy(defeated);
    }

    // Makes a unit slowly vanish-- not the right approach currently w/ pointers and things
    IEnumerator vanish(Unit defeated)
    {
        MeshRenderer renderer = defeated.getObj().GetComponent<MeshRenderer>();
        Color col;
        WaitForSeconds wait = new WaitForSeconds(0.002f);
        for(int i = 0; i < 240; i++)
        {
            col = renderer.material.color;
            col.a -= (1.0f / 250.0f);
            renderer.material.color = col;
            Debug.Log(renderer.material.color);
            yield return wait;
        }
        Debug.Log("finished loop");
        Destroy(defeated);
    }
}