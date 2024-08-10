using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile]
public struct CreateCellsJob : IJob
{
    [ReadOnly] public int Size;
    public NativeArray<float3> HexCellPositionsAray;
	public NativeArray<int3> HexCellGridPosition;

    public void Execute()
    {
		for (int z = 0, i = 0; z < Size; z++)
		{
			for (int y = 0; y < Size; y++)
			{
				for (int x = 0; x < Size; x++)
				{
					AddCellPosition(x, y, z, i++);
				}
			}
		}
	}

    private void AddCellPosition(int x, int y, int z, int i)
    {
		float3 position;
		HexCellGridPosition[i] = new int3(x, y, z);

		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		position.y = y * HexMetrics.height;
		position.z = z * (HexMetrics.outerRadius * 1.5f);

		HexCellPositionsAray[i] = position;
		
	}
}
