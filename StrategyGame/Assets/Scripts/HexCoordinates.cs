﻿// A data structure for axial coordinates on the hexagonal grid
// Hexagons sit edge to edge on each axis
// All coordinates (X, Y, Z) sum to 0

using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public struct HexCoordinates
{
    [SerializeField]
    private int x, z;

    public int X
    {
        get
        { return x; }
    }

    public int Z
    {
        get
        { return z; }
    }

    public int Y
    {
        get { return -X - Z; }
    }

    // Only two coordinates needed to establish unique position on grid
    public HexCoordinates(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    // To find hex coordinates upon grid initialization
    public static HexCoordinates FromOffsetCoordinates(int x, int z)
    {
        return new HexCoordinates(x - z / 2, z);
    }

    // Finds hex coordinates from a world position
    public static HexCoordinates FromPosition(Vector3 position)
    {
        // X-coordinate on grid depends on width of hexagons (inner radius)
        float x = position.x / (HexMetrics.innerRadius * 2f);
        // X up = Y down
        float y = -x;
        // "height" of click determines Z-coord
        float offset = position.z / (HexMetrics.outerRadius * 3f);
        // Z up = X & Y down
        x -= offset;
        y -= offset;

        // coordinates will eventually need to be integers
        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(y);
        int iZ = Mathf.RoundToInt(-x - y);

        // Floating-point rounding errors can happen:
        if (iX + iY + iZ != 0)
        {
            float dX = Mathf.Abs(x - iX);
            float dY = Mathf.Abs(y - iY);
            float dZ = Mathf.Abs(-x - y - iZ);

            // Reconstruct least accurate coordinate
            if (dX > dY && dX > dZ)
            {
                iX = -iY - iZ;
            }
            else if (dZ > dY)
            {
                iZ = -iX - iY;
            }
        }

        return new HexCoordinates(iX, iZ);
    }

    // Converts HexCoordinates to actual 2D positions
    public static Vector2 fromHexCoordinates(HexCoordinates coords)
    {
        float cX = coords.X;
        float cZ = coords.Z;
        float x = (cX * 2 + cZ) * HexMetrics.innerRadius;
        float z = cZ * (1.5f * HexMetrics.outerRadius);
        return new Vector2(x, z);
    }

    // Returns a new HexCoordinates one tile away in the specified direction
    // Directions start with due east and go clockwise
    public static HexCoordinates translate(HexCoordinates start, int dir)
    {
        HexCoordinates dXZ;
        switch (dir)
        {
            case 0:
                dXZ = new HexCoordinates(1, 0);
                break;
            case 1:
                dXZ = new HexCoordinates(1, -1);
                break;
            case 2:
                dXZ = new HexCoordinates(0, -1);
                break;
            case 3:
                dXZ = new HexCoordinates(-1, 0);
                break;
            case 4:
                dXZ = new HexCoordinates(-1, 1);
                break;
            case 5:
                dXZ = new HexCoordinates(0, 1);
                break;
            default:
                return start;
        }
        return new HexCoordinates(start.X + dXZ.X, start.Z + dXZ.Z);
    }

    // Finds the distance between two HexCoordinates
    public static int distance(HexCoordinates a, HexCoordinates b)
    {
        int dx = Math.Abs(a.X - b.X);
        int dy = Math.Abs(a.Y - b.Y);
        int dz = Math.Abs(a.Z - b.Z);

        // Distance is absolute value of greatest distance on any axis
        int distance = 0;
        if (dx > dy)
            distance = dx;
        else
            distance = dy;
        if (dz > distance)
            distance = dz;

        return distance;
    }

    // Finds cells that a unit on origin can move to
    public static List<HexCoordinates> cellSearch(int range, HexCoordinates origin, int searchStyle)
    {
        // searchStyle defines what search algorithm should look for
        // 0: cells ally can move to, 1: cells enemy can move to, 2: cells ally can attack, 3: cells enemy can attack
        HexGrid.unmarkGrid();
        HexCell orCell = HexGrid.cellFromHC(origin);
        orCell.found = true;

        Queue<HexCell> pathCells = new Queue<HexCell>();
        List<HexCoordinates> resultCells = new List<HexCoordinates>();
        HexCell nextCell;
        HexCell curCell;
        int stepsTaken = 1;

        // Based on BFS algorithm from Wikipedia
        pathCells.Enqueue(orCell);
        while(pathCells.Count > 0)
        {
            //Debug.Log("started new layer");
            curCell = pathCells.Dequeue();
            // Stop searching once you reach the allowed range
            stepsTaken = HexCoordinates.distance(curCell.coordinates, origin) + 1;
            //Debug.Log("cell: " + curCell.coordinates + " steps: " + stepsTaken);
            //Debug.Log(stepsTaken);
            if (stepsTaken > range)
            {
                //Debug.Log("hit range");
                return resultCells;
            }
            for(int i = 0; i < 6; i++)
            {
                //Debug.Log("checking new tile from " + curCell.coordinates.ToString());
                
                nextCell = curCell.getNeighbor((HexDirection)i);

                if(nextCell != null && !nextCell.found)
                {
                    // Inspect this cell
                    // If looking for movement range, add empty cells to set of movable cells
                    if((searchStyle == 0 || searchStyle == 1) && nextCell.status == HexGrid.Status.EMPTY)
                    {
                        resultCells.Add(nextCell.coordinates);
                        //Debug.Log(nextCell.coordinates);
                    }
                    // If looking for movement range, can move through any cell not occupied by other side
                    if(nextCell.status == HexGrid.Status.EMPTY || ((searchStyle == 0 && nextCell.status == HexGrid.Status.ALLY) ||
                                                                   (searchStyle == 1 && nextCell.status == HexGrid.Status.ENEMY)))
                    {
                        // Give this cell a path
                        //Debug.Log("starting queue proc. steps: " + stepsTaken);
                        nextCell.path = new int[stepsTaken];
                        if (stepsTaken == 1)
                        {
                            nextCell.path[0] = i;
                        }
                        else
                        {
                            for(int j = 0; j < stepsTaken-1; j++)
                            {
                                //Debug.Log(j + " " + stepsTaken);
                                //Debug.Log(curCell.path[j]);
                                nextCell.path[j] = curCell.path[j];
                            }
                            nextCell.path[nextCell.path.Length - 1] = i;
                        }
                        pathCells.Enqueue(nextCell);
                        //Debug.Log("Enqueued " + nextCell.coordinates.ToString());
                    }
                    // If looking for attack range, only check cell if it is at the right range
                    //Debug.Log(stepsTaken + ", " + searchStyle + ", " + range);
                    if((searchStyle == 2 || searchStyle == 3) && stepsTaken == range)
                    {
                        //Debug.Log("at range at cell " + nextCell.coordinates);
                        // See if cell has a unit from the other team on it
                        if(searchStyle == 2 && nextCell.status == HexGrid.Status.ENEMY)
                        {
                            resultCells.Add(nextCell.coordinates);
                            Debug.Log(nextCell.coordinates.ToString());
                        }
                        if (searchStyle == 3 && nextCell.status == HexGrid.Status.ALLY)
                        {
                            resultCells.Add(nextCell.coordinates);
                            Debug.Log(nextCell.coordinates.ToString());
                        }
                    }
                    nextCell.found = true;
                }
            }
            //Debug.Log("Count: " + pathCells.Count);
        }
        return resultCells;
    }

    // For internal debugging
    public override string ToString()
    {
        return "(" + X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
    }

    public string ToStringOnSeparateLines()
    {
        return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
    }
}