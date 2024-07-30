using System.Collections.Generic;
using UnityEngine;

public class HexChunk : MonoBehaviour
{
	public int size;
	public HexCell[] cells;
	public Dictionary<Vector3, HexCell> cellsByCoordinates;

	public Vector3 Bounds;

	public void Initialize(int size, HexMesh mesh)
	{
		this.size = size;

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

		Bounds = new Vector3()
		{
			x = transform.position.x * (HexMetrics.innerRadius * size * 2),
			y = transform.position.y * size * HexMetrics.height,
			z = transform.position.z * (HexMetrics.outerRadius * size * 1.5f)
		};

		mesh.Initialize(this);
		mesh.Triangulate(cells);
	}


	private void CreateCell(int x, int y,int z, int i)
	{
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		position.y = y * HexMetrics.height;
		position.z = z * (HexMetrics.outerRadius * 1.5f);

		Vector3 worldPos = transform.position + new Vector3(x, y, z);

		//HexCell.CellType type = DetermineCellType(worldPos.x, worldPos.y, worldPos.z);

		var cellGO = new GameObject($"cell {position.x}_{position.y}_{position.z}");

		HexCell cell = cells[i] = cellGO.AddComponent<HexCell>();

		//cell.isActive = type != HexCell.CellType.Air;
		cell.isActive = true;

		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
		AssignNeighbors(cell, x, y, z, size, i);

		cellsByCoordinates.Add(cell.transform.localPosition, cell);
	}

    private HexCell.CellType DetermineCellType(float x, float y, float z)
    {
		float noiseValue = GlobalNoise.GetGlobalNoiseValue(x, z, World.Instance.noiseArray);
		float normalizedNoiseValue = (noiseValue + 1) / 2;
		float maxHeight = normalizedNoiseValue * World.Instance.maxHeight;

		if (y <= maxHeight)
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
