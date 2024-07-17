using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
	public int size;

	public HexCell cellPrefab;

	HexCell[] cells;

	public Dictionary<Vector3, HexCell> cellsByCoordinates;

	HexMesh hexMesh;
	MeshCollider meshCollider;

	void Awake()
	{
		hexMesh = GetComponentInChildren<HexMesh>();
		//meshCollider = gameObject.AddComponent<MeshCollider>();

		cellsByCoordinates = new Dictionary<Vector3, HexCell>();
		cells = new HexCell[size * size * size];

		for (int x = 0, i = 0; x < size; x++)
		{
			for (int y = 0; y < size; y++)
			{
				for (int z = 0; z < size; z++)
				{
					CreateCell(x, y, z, i++);
				}
			}
		}
	}

    private void Start()
    {
		hexMesh.Triangulate(cells);
		//meshCollider.sharedMesh = hexMesh.GetComponent<MeshFilter>().mesh;
	}


	void CreateCell(int x, int y,int z, int i)
	{
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		position.y = y * HexMetrics.height;
		position.z = z * (HexMetrics.outerRadius * 1.5f);

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.isActive = true;

		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;

		cellsByCoordinates.Add(cell.transform.localPosition, cell);

	}


}
