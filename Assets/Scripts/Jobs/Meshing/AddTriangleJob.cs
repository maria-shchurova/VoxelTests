using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile]
public struct AddTriangleJob : IJob
{
    public int vertexIndex;
    public int triangleIndex;
    [ReadOnly] public NativeArray<float3> coordinates;
    public NativeArray<float3> vertices;
    public NativeArray<int> triangles;


    public void Execute()
    {
        // Check to ensure we don't exceed array limits
        if (vertexIndex + 3 >= vertices.Length || triangleIndex + 3 >= triangles.Length)
        {
            return;
        }

        vertices[vertexIndex] = coordinates[0];
        vertices[vertexIndex + 1] = coordinates[1];
        vertices[vertexIndex + 2] = coordinates[2];

        triangles[triangleIndex] = vertexIndex;
        triangles[triangleIndex + 1] = vertexIndex + 1;
        triangles[triangleIndex + 2] = vertexIndex + 2;

    }

}
