using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TestOneCell : MonoBehaviour
{
    public GameObject prefab;
    Vector3 topCenter;
    Vector3 bottomCenter;

    List<Vector3> vertices;
    List<int> triangles;

    Mesh myMesh;

    void Awake()
    {
        GetComponent<MeshFilter>().mesh = myMesh = new Mesh();
        myMesh.name = "Hex Mesh";
        vertices = new List<Vector3>();
        triangles = new List<int>();
    }

    void Start()
    {
        for (int i = 0; i < HexMetrics.corners.Length; i++)
        {
            GameObject obj = Instantiate(prefab, HexMetrics.corners[i], Quaternion.identity);
            obj.name = i.ToString();
            GameObject obj2 = Instantiate(prefab, HexMetrics.cornersTOP[i], Quaternion.identity);
            obj2.name = "top " + i.ToString();
        }
        GameObject bC = Instantiate(prefab, transform.position, Quaternion.identity);
        bC.name = "bottomCenter";
        bottomCenter = bC.transform.position;

        GameObject tC = Instantiate(prefab, transform.position + Vector3.up * HexMetrics.height, Quaternion.identity);
        tC.name = "topCenter";
        topCenter = tC.transform.position;

        Triangulate();
        myMesh.vertices = vertices.ToArray();
        myMesh.triangles = triangles.ToArray();
        myMesh.RecalculateNormals();
    }

    void Triangulate()
    {
        for (int i = 0; i < 6; i++)
        {
            // Bottom face
            AddTriangle(
                bottomCenter,
                HexMetrics.corners[i],
                HexMetrics.corners[i + 1]
            );

            // Top face
            AddTriangle(
                topCenter,
                HexMetrics.cornersTOP[i],
                HexMetrics.cornersTOP[i + 1]
            );

            // Side face 1
            AddTriangle(
                HexMetrics.cornersTOP[i + 1],
                HexMetrics.cornersTOP[i],
                HexMetrics.corners[i]
            );

            // Side face 2
            AddTriangle(
                HexMetrics.corners[i + 1],
                HexMetrics.cornersTOP[i + 1],
                HexMetrics.corners[i]
            );
        }
    }

    void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
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

