using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour
{
    Mesh hexMesh;
    List<Vector3> vertices;
    List<int> triangles;



    void Awake()
    {
        GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
        hexMesh.name = "Hex Mesh";
        vertices = new List<Vector3>();
        triangles = new List<int>();
    }
    /*

     public void Triangulate(HexCell[] cells)
     {
         hexMesh.Clear();
         vertices.Clear();
         triangles.Clear();
         for (int i = 0; i < cells.Length; i++)
         {
             Triangulate(cells[i]);
         }
         hexMesh.vertices = vertices.ToArray();
         hexMesh.triangles = triangles.ToArray();
         hexMesh.RecalculateNormals();
     }

     void Triangulate(HexCell cell)
     {

     }
     */
}
