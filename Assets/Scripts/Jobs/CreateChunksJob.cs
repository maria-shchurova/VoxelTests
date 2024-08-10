using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile]
public struct CreateChunksJob : IJob
{
    public NativeArray<float3> HexChunkPosiitonsArray;
    [ReadOnly] public int WorldSize;
    [ReadOnly] public int ChunkSize;

    public void Execute()
    {
        int index = 0;
        for (int x = 0; x < WorldSize; x++)
        {
            for (int y = 0; y < WorldSize; y++)
            {
                for (int z = 0; z < WorldSize; z++)
                {
                    float3 chunkPosition = new float3(x * (HexMetrics.innerRadius * ChunkSize * 2), y * ChunkSize * HexMetrics.height, z * (HexMetrics.outerRadius * ChunkSize * 1.5f));
                    HexChunkPosiitonsArray[index] = chunkPosition;
                    index++;
                }
            }
        }
    }
}
