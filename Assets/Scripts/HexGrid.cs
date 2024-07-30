using SimplexNoise;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HexGrid : MonoBehaviour
{
	public int size;

	public HexCell cellPrefab;

	public HexCell[] cells;

	public Dictionary<Vector3, HexCell> cellsByCoordinates;

	[Inject] 
	private HexMesh hexMesh;

	[Inject] private DiContainer _container;
	void Awake()
	{

		cellsByCoordinates = new Dictionary<Vector3, HexCell>();

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
	}


	private void CreateCell(int x, int y,int z, int i)
	{
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		position.y = y * HexMetrics.height;
		position.z = z * (HexMetrics.outerRadius * 1.5f);
		
		HexCell.CellType type = DetermineCellType(position.x, position.y, position.z);
		HexCell cell = cells[i] = _container.InstantiatePrefabForComponent<HexCell>(cellPrefab);
		
		cell.isActive = type != HexCell.CellType.Air;

		AssignNeighbors(cell, x, y, z, size, i);


		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;

		cellsByCoordinates.Add(cell.transform.localPosition, cell);

		cell.gameObject.name = $"cell {position.x}_{position.y}_{position.z}";

	}

    private HexCell.CellType DetermineCellType(float x, float y, float z)
    {
		float noiseValue = Noise.CalcPixel3D((int)x, (int)y, (int)z, 0.1f);

		float threshold = 125f; // The threshold for determining solid/air

		//Debug.Log(noiseValue);

		if (noiseValue > threshold)
			return HexCell.CellType.Grass; // Solid voxel
		else
			return HexCell.CellType.Air; // Air voxel
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
