﻿// Contains the whole hex grid 
using UnityEngine;
using UnityEngine.UI;


public class HexGrid : MonoBehaviour
{
    public enum Terrain
    {
        FIELD, WATER, MUD
    }

    public enum Status
    {
        EMPTY, ALLY, ENEMY
    }

    public static int width = 6;
    public static int height = 6;

    public HexCell cellPrefab;
    static HexCell[] cells;

    public Text cellLabelPrefab;
    Canvas gridCanvas;

    HexMesh hexMesh;

    public static bool gridReady = false;

    public Color defaultColor = Color.white;

    HexCoordinates touched;


    private void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();

        cells = new HexCell[height * width];

        for(int z = 0, i = 0; z < height; z++)
        {
            for(int x = 0; x < width; x++)
            {
                CreateCell(x, z, i++);
            }
        }
        gridReady = true;
    }

    private void Start()
    {
        hexMesh.Triangulate(cells);
    }

    void CreateCell (int x, int z, int i)
    {
        Vector3 position;
        position.x = (x + z * .5f - z/2) * (HexMetrics.innerRadius * 2f);
        position.y = 0;
        position.z = z * (HexMetrics.outerRadius * 1.5f);

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.color = defaultColor;

        if(x > 0)
        {
            cell.SetNeighbor(HexDirection.W, cells[i - 1]);
        }
        if(z > 0)
        {
            if ((z & 1) == 0)
            {
                cell.SetNeighbor(HexDirection.SE, cells[i - width]);
                if (x > 0)
                {
                    cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]);
                }
            }
            else
            {
                cell.SetNeighbor(HexDirection.SW, cells[i - width]);
                if (x < width - 1)
                {
                    cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]);
                }
            }
        }

        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();
    }
    
    // Touching Cells
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            HandleInput();
        }
    }

    void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(inputRay, out hit))
        {
            TouchCell(hit.point, Color.white);
        }
    }

    // Finds a HexCell object from its HexCoordinates
    public static HexCell cellFromHC(HexCoordinates coords)
    {
        int index = coords.X + coords.Z * width + coords.Z / 2;
        return cells[index];
    }
    
    public void TouchCell(Vector3 position, Color color)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coords = HexCoordinates.FromPosition(position);
        HexCell cell = cellFromHC(coords);
        cell.color = color;
        hexMesh.Triangulate(cells);

        //Debug.Log("touched at " + coords.ToString());

        UnitHandler.moveUnit(coords);
    }

    public HexCell getCell(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        return cells[index];
    }


    public void refresh()
    {
        hexMesh.Triangulate(cells);
    }

    // Unmark the grid for a pathfind
    public static void unmarkGrid()
    {
        for(int i = 0; i < cells.Length; i++)
        {
            cells[i].found = false;
        }
    }
}
