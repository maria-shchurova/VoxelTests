using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class HexChunk : MonoBehaviour
{
	public int size;
	public HexChunk[] neighbors = new HexChunk[6];
	public HexCell[] cells;
	public Dictionary<Vector3, HexCell> cellsByCoordinates;
	public NativeArray<float3> hexCellPositionsAray;
	public NativeArray<int3> hexCellGridPosition;
	public Vector3 Bounds;

	[SerializeField]
	private HexMesh mesh;

    private void OnDestroy()
    {
		hexCellPositionsAray.Dispose();
		hexCellGridPosition.Dispose();

	}
	public void Initialize()
    //void Start()
	{
		this.size = World.Instance.chunkSize;

		cellsByCoordinates = new Dictionary<Vector3, HexCell>();
		hexCellPositionsAray = new NativeArray<float3>(size * size * size, Allocator.Persistent);
		hexCellGridPosition = new NativeArray<int3>(size *size * size, Allocator.Persistent);
		cells = new HexCell[size * size * size];

		CreateCellsJob createJob = new CreateCellsJob
        {
			HexCellGridPosition = hexCellGridPosition,
			HexCellPositionsAray = hexCellPositionsAray,
			Size = size
		};

		JobHandle createHandle = createJob.Schedule();
		createHandle.Complete();

		for (int i = 0; i < hexCellPositionsAray.Length; i++)
		{
			Vector3 position = new Vector3(hexCellPositionsAray[i].x, hexCellPositionsAray[i].y, hexCellPositionsAray[i].z);
			CreateCell(position, i);
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


	private void CreateCell(Vector3 position, int i)
	{
		Vector3 worldPos = transform.position + position;

		HexCell.CellType type = DetermineCellType(worldPos.x, worldPos.y, worldPos.z);

		cells[i] = new HexCell(type, type != HexCell.CellType.Air);
		//cells[i] = new HexCell(type, true);

		cells[i].position = position;
		cellsByCoordinates.Add(position, cells[i]);

		AssignNeighbors(
			cells[i], 
			hexCellGridPosition[i].x,
			hexCellGridPosition[i].y,
			hexCellGridPosition[i].z,
			size, i);

	}

    private HexCell.CellType DetermineCellType(float x, float y, float z)
    {
		float noiseValue = GlobalNoise.GetGlobalNoiseValue(x, z, World.Instance.noiseArray);
		//float normalizedNoiseValue = (noiseValue + 1) / 2;
		float maxHeight = noiseValue * World.Instance.maxHeight;

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

	public void SetNeighbor(int direction, HexChunk neighbor)
	{
		neighbors[direction] = neighbor;
	}

	/* neighbors:
	  0: +x (right)
	  1: -x (left)
	  2: +y (up)
	  3: -y (down)
	  4: +z (forward)
	  5: -z (backward)
	 */
}
