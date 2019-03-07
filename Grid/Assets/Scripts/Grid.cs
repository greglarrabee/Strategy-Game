using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap : MonoBehaviour
{
    public Transform tile;
    public int width;
    public int length;

    private GameObject[][] grid;

    // Instantiate a grid with the given dimensions as parameters
    void Start()
    {
        for(int z = 0; z < width; z++)
        {
            for(int x = 0; x < length; x++)
            {
                GameObject g = Instantiate(tile, new Vector3(x*Mathf.Sqrt(3)/2, 0, (z*3)+(x%2)*1.5f), Quaternion.identity).gameObject;
                g.transform.parent = transform;
                grid[z + x & 2][x - (z + 1) % 2] = g;
            }
        }
    }
}
