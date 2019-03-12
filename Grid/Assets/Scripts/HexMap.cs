using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap : MonoBehaviour
{
    public Transform tile;
    public int size;
    private const float innerRadius = 1;
    // The outer radius of a hexagon with inner radius 1
    private static readonly float outerRadius = Mathf.Sqrt(3f) / 2;

    // Instantiate a grid with the given dimensions as parameters
    void Start()
    {
        // Create first half of grid
        float startOffset = 0;
        for(int x = size; x > 0; x--)
        {
            for(int z = 0; z < x+(size-1); z++)
            {
                Instantiate(tile, new Vector3((float)x/2+x*outerRadius, 0, z*2*outerRadius-startOffset), Quaternion.Euler(0, 30, 0), transform);
            }
            startOffset -= outerRadius;
        }
        // Create smaller "half" of grid
        startOffset += outerRadius;
        for (int x = size-1; x > 0; x--)
        {
            for (int z = 0; z < (size-x) + (size-1); z++)
            {
                Instantiate(tile, new Vector3((float)(x + size) / 2 + (x + size) * outerRadius, 0, z * 2 * outerRadius - startOffset), Quaternion.Euler(0, 30, 0), transform);
            }
            startOffset += outerRadius;
        }
    }
}
