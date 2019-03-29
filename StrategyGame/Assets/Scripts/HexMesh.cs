// Creates the mesh for the entire grid based on the HexCell objects
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]


public class HexMesh : MonoBehaviour
{
    Mesh hexMesh;
    MeshCollider meshCollider;
    List<Vector3> vertices;
    List<int> triangles;

    
    void Awake()
    {
        // Create mesh and meshCollider for grid
        GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
        meshCollider = gameObject.AddComponent<MeshCollider>();
        hexMesh.name = "Hex Mesh";
        vertices = new List<Vector3>();
        triangles = new List<int>();
    }

    // Triangulate some number of HexCells
    public void Triangulate(HexCell[] cells)
    {
        // Clear old data (mesh might have already been triangulated)
        hexMesh.Clear();
        vertices.Clear();
        triangles.Clear();
        // Triangulate each cell
        for(int i = 0; i < cells.Length; i++)
        {
            Triangulate(cells[i]);
        }
        // Put vertices into HexMesh, make sure normals are okay
        hexMesh.vertices = vertices.ToArray();
        hexMesh.triangles = triangles.ToArray();
        hexMesh.RecalculateNormals();
        // Attach grid's collider to the mesh
        meshCollider.sharedMesh = hexMesh;
    }


    // Triangulate a specific cell
    public void Triangulate(HexCell cell)
    {
        Vector3 center = cell.transform.localPosition;
        for (int i = 0; i < 6; i++)
        {
            AddTriangle(center, center + HexMetrics.corners[i], center + HexMetrics.corners[i+1]);
        }
    }


    // Add a single triangle to the vertex and triangle arrays
    public void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
    }
}
