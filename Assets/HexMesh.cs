using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour
{

	List<Vector3> vertices;
	List<int> triangles;

	[SerializeField]
	private HexGrid hexGrid;

	[SerializeField]
	private MeshFilter hexMeshFilter;
	[SerializeField]
	private MeshCollider collider;

	void Awake()
	{
		hexMeshFilter.mesh = new Mesh();
		hexMeshFilter.mesh.name = "Hex Mesh";
		vertices = new List<Vector3>();
		triangles = new List<int>();
	}

	public void Clear()
	{
        hexMeshFilter.mesh.Clear(false);
        vertices.Clear();
        triangles.Clear();

        // Ensure all mesh data arrays are emptied
        hexMeshFilter.mesh.vertices = new Vector3[0];
        hexMeshFilter.mesh.triangles = new int[0];
        hexMeshFilter.mesh.normals = new Vector3[0];

        if (hexMeshFilter.mesh.vertices.Length > 0)
        {
            hexMeshFilter.mesh.UploadMeshData(true);
            hexMeshFilter.mesh.RecalculateNormals();
        }
    }

	public void Triangulate(HexCell[] cells)
	{
		Clear();

		for (int i = 0; i < cells.Length; i++)
        {

			if(cells[i].isActive)
            {
				Triangulate(cells[i]);
			}
		}

		hexMeshFilter.mesh.vertices = vertices.ToArray();
		hexMeshFilter.mesh.triangles = triangles.ToArray();
		hexMeshFilter.mesh.RecalculateNormals();
		collider.sharedMesh = GetComponent<MeshFilter>().mesh;
	}

	private void Triangulate(HexCell hexCell)
    {
		Vector3 center = hexCell.transform.localPosition;
		Vector3 centerTop = hexCell.transform.localPosition + new Vector3(0, HexMetrics.height, 0);
		for (int i = 0; i < 6; i++)
		{

			if(!IsFaceVisible(center + Vector3.up)) //if neighboring on above cell is NOT active
            {
				// Top face
				AddTriangle(
					centerTop,
					centerTop + HexMetrics.corners[i],
					centerTop + HexMetrics.corners[i + 1]
				);
			}

			if (!IsFaceVisible(center + Vector3.down)) //if neighboring below cell is NOT active
            {
				// Bottom face
				AddTriangle(
				center + HexMetrics.corners[i + 1],
				center + HexMetrics.corners[i],
				center
			);
			}
			// 0 face is NE
			// 1 face is E
			// 2 face is SE
			// 3 face is SW
			// 4 face is W
			// 5 face is NW

			if (hexCell.GetNeighbor(i) == null || !hexCell.GetNeighbor(i).isActive)
            {
				// Side face 1
				AddTriangle(
					centerTop + HexMetrics.corners[i + 1],
					centerTop + HexMetrics.corners[i],
					center + HexMetrics.corners[i]
				);

				// Side face 2
				AddTriangle(
					center + HexMetrics.corners[i + 1],
					centerTop + HexMetrics.corners[i + 1],
					center + HexMetrics.corners[i]
				);
			}

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

	private bool IsFaceVisible(Vector3 direction)
	{
		HexCell neighbour;
		hexGrid.cellsByCoordinates.TryGetValue(direction, out neighbour);

		if (neighbour != null)
			return neighbour.isActive;
		else
			return false;
	}

}
