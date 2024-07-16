using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
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

    private void Triangulate(HexCell hexCell)
    {
		Vector3 center = hexCell.transform.localPosition;
		Vector3 centerTop = hexCell.transform.localPosition + new Vector3(0, HexMetrics.height, 0);
		for (int i = 0; i < 6; i++)
		{
			// Bottom face
			AddTriangle(
				center + HexMetrics.corners[i + 1],
				center + HexMetrics.corners[i],
				center
			);

            // Top face
            AddTriangle(
                centerTop,
                centerTop + HexMetrics.cornersTOP[i],
                centerTop + HexMetrics.cornersTOP[i + 1]
            );

            // Side face 1
            AddTriangle(
                centerTop + HexMetrics.cornersTOP[i + 1],
                centerTop + HexMetrics.cornersTOP[i],
                center + HexMetrics.corners[i]
            );

            // Side face 2
            AddTriangle(
                center + HexMetrics.corners[i + 1],
                centerTop + HexMetrics.cornersTOP[i + 1],
                center + HexMetrics.corners[i]
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