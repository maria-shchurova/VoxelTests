using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
	public int size;

	public HexCell cellPrefab;

	HexCell[] cells;

	public Dictionary<Vector3, HexCell> cellsByCoordinates;
	public Dictionary<AxialCoordinate, HexCell> cellsByAxialCoordinates;

	HexMesh hexMesh;
	MeshCollider meshCollider;

	void Awake()
	{
		hexMesh = GetComponentInChildren<HexMesh>();
		meshCollider = gameObject.AddComponent<MeshCollider>();

		cellsByCoordinates = new Dictionary<Vector3, HexCell>();
		cellsByAxialCoordinates = new Dictionary<AxialCoordinate, HexCell>();

		cells = new HexCell[size * size * size];

		for (int z = 0, i = 0; z < size; z++)
		{
			for (int y = 0; y < size; y++)
			{
				for (int x = 0; x < size; x++)
				{
					CreateCell(x, y, z, i++);
				}
			}
		}
	}

    private void Start()
    {
		hexMesh.Triangulate(cells);
		meshCollider.sharedMesh = hexMesh.GetComponent<MeshFilter>().mesh;
	}


	private void CreateCell(int x, int y,int z, int i)
	{
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		position.y = y * HexMetrics.height;
		position.z = z * (HexMetrics.outerRadius * 1.5f);

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.gameObject.name = $"cell {i} {x}_{y}_{z}";
		cell.isActive = true;

		AssignNeighbors(cell, x, y, z, size, i);


		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;

		cellsByCoordinates.Add(cell.transform.localPosition, cell);

		cell.coordinates = AxialCoordinate.FromWorldPosition(position);
		cellsByAxialCoordinates.Add(cell.coordinates, cell);
	}

    private void AssignNeighbors(HexCell cell, int x, int y, int z, int size, int index)
    {
		if (x > 0)
		{
			cell.SetNeighbor(HexDirection.W, cells[index - 1]);
		}
		if (z > 0)
		{
			if ((z & 1) == 0)
			{
				cell.SetNeighbor(HexDirection.SE, cells[index - size * size]);
				if (x > 0)
				{
					cell.SetNeighbor(HexDirection.SW, cells[index - size * size - 1]);
				}
			}
			else
			{
				cell.SetNeighbor(HexDirection.SW, cells[index - size * size]);
				if (x < size - 1)
				{
					cell.SetNeighbor(HexDirection.SE, cells[index - size * size + 1]);
				}
			}
		}
	}
}
