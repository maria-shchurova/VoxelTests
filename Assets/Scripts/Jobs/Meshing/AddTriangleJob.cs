using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile]
public class AddTriangleJob : IJob
{
    public void Execute()
    {
        throw new System.NotImplementedException();
    }

    void AddTriangle(float3 v1, float3 v2, float3 v3)
    {
        //// Check to ensure we don't exceed array limits
        //if (vertexIndex + 3 >= vertices.Length || triangleIndex + 3 >= triangles.Length)
        //{
        //    Debug.LogError("HexMesh: Exceeded predefined array size. Increase the initial size estimate.");
        //    return;
        //}

        //vertices[vertexIndex] = v1;
        //vertices[vertexIndex + 1] = v2;
        //vertices[vertexIndex + 2] = v3;

        //triangles[triangleIndex] = vertexIndex;
        //triangles[triangleIndex + 1] = vertexIndex + 1;
        //triangles[triangleIndex + 2] = vertexIndex + 2;

        //vertexIndex += 3;
        //triangleIndex += 3;
    }
}
