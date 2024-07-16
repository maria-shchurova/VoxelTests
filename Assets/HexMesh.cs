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
		for (int i = 0; i < 6; i++)
		{
				AddTriangle(
				center,
				center + HexMetrics.corners[i],
				center + HexMetrics.corners[i + 1]
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

	public void Triangulate3D(HexCell cell)
    {

        // Bottom hexagon
        for (int i = 0; i < 6; i++)
        {
            float angle = Mathf.PI / 3 * i;
            vertices[i] = new Vector3(HexMetrics.outerRadius * Mathf.Cos(angle), 0, HexMetrics.outerRadius * Mathf.Sin(angle));
        }

        // Top hexagon
        for (int i = 0; i < 6; i++)
        {
            vertices[i + 6] = vertices[i] + Vector3.up * HexMetrics.height;
        }

        // Center points
        vertices[12] = Vector3.zero;
        vertices[13] = Vector3.up * HexMetrics.height;

        // Bottom face
        for (int i = 0; i < 6; i++)
        {
            triangles[3 * i] = 12;
            triangles[3 * i + 1] = i;
            triangles[3 * i + 2] = (i + 1) % 6;
        }

        // Top face
        for (int i = 0; i < 6; i++)
        {
            triangles[18 + 3 * i] = 13;
            triangles[18 + 3 * i + 1] = 6 + (i + 1) % 6;
            triangles[18 + 3 * i + 2] = 6 + i;
        }

        // Side faces
        for (int i = 0; i < 6; i++)
        {
            int next = (i + 1) % 6;

            // First triangle
            triangles[36 + 6 * i] = i;
            triangles[36 + 6 * i + 1] = 6 + i;
            triangles[36 + 6 * i + 2] = next;

            // Second triangle
            triangles[36 + 6 * i + 3] = next;
            triangles[36 + 6 * i + 4] = 6 + i;
            triangles[36 + 6 * i + 5] = 6 + next;
        }

        hexMesh.vertices = vertices.ToArray();
        hexMesh.triangles = triangles.ToArray();
        hexMesh.RecalculateNormals();

    }

}