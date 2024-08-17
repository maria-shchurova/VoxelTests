using UnityEngine;
using System.Collections.Generic;

public class HexMesh : MonoBehaviour
{
	List<Vector3> vertices;
	List<int> triangles;

	private HexChunk hexGrid;
	[SerializeField]
	private MeshFilter hexMeshFilter;
	//private MeshCollider collider;

	public void Initialize(HexChunk grid)
	{
		hexGrid = grid;
		//collider = GetComponent<MeshCollider>();

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

        //if (hexMeshFilter.mesh.vertices.Length > 0)
        //{
        //    hexMeshFilter.mesh.UploadMeshData(true);
        //    hexMeshFilter.mesh.RecalculateNormals();
        //}
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
		//collider.sharedMesh = GetComponent<MeshFilter>().mesh;
	}

	private void Triangulate(HexCell hexCell)
    {
		Vector3 center = hexCell.position;
		Vector3 centerTop = hexCell.position + new Vector3(0, HexMetrics.height, 0);

		for (int i = 0; i < 6; i++)
		{

			if((hexCell.neighborsBitmask & GetDirectionBitmask(6)) == 0) 
            {
				// Top face
				AddTriangle(
					centerTop,
					centerTop + HexMetrics.corners[i],
					centerTop + HexMetrics.corners[i + 1]
				);
			}

			if ((hexCell.neighborsBitmask & GetDirectionBitmask(7)) == 0) 
            {
				// Bottom face
				AddTriangle(
				center + HexMetrics.corners[i + 1],
				center + HexMetrics.corners[i],
				center
			);
			}

			// Check if the neighbor in this direction is inactive or absent using the bitmask
			if ((hexCell.neighborsBitmask & GetDirectionBitmask(i)) == 0)
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

	private BitmaskNeighbors GetDirectionBitmask(int i)
	{
		switch (i)
		{
			case 0: return BitmaskNeighbors.NE;
			case 1: return BitmaskNeighbors.E;
			case 2: return BitmaskNeighbors.SE;
			case 3: return BitmaskNeighbors.SW;
			case 4: return BitmaskNeighbors.W;
			case 5: return BitmaskNeighbors.NW;
			case 6: return BitmaskNeighbors.TOP;
			case 7: return BitmaskNeighbors.BOTTOM;
			default: return BitmaskNeighbors.None; // Shouldn't reach here
		}
	}

}
