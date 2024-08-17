using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public class HexMesh : MonoBehaviour
{
    private NativeArray<float3> vertices;
    private NativeArray<int> triangles; 

    private int vertexIndex = 0;
    private int triangleIndex = 0;

    [SerializeField]
    private MeshFilter hexMeshFilter;

    public void Initialize()
    {
        int chunkSize = World.Instance.chunkSize;

        hexMeshFilter.mesh = new Mesh();
        hexMeshFilter.mesh.name = "Hex Mesh";

        // Predefine sizes (estimate based on grid size)
        int maxVertices = chunkSize * chunkSize * chunkSize * 12; 
        int maxTriangles = maxVertices * 3; // Estimate based on the number of faces

        vertices = new NativeArray<float3>(maxVertices, Allocator.Persistent);
        triangles = new NativeArray<int>(maxTriangles, Allocator.Persistent);
    }

    public void Clear()
    {
        hexMeshFilter.mesh.Clear(false);

        // Reset indices and clear mesh data
        vertexIndex = 0;
        triangleIndex = 0;
        // Reset the arrays
        vertices = new NativeArray<float3>(vertices.Length, Allocator.Persistent);
        triangles = new NativeArray<int>(triangles.Length, Allocator.Persistent);
    }

    public void Triangulate(HexCell[] cells)
    {
        Clear();

        for (int i = 0; i < cells.Length; i++)
        {
            if (cells[i].isActive)
            {
                Triangulate(cells[i]);
            }
        }


        hexMeshFilter.mesh.vertices = vertices.ToVector3Array();
        hexMeshFilter.mesh.triangles = triangles.ToIntArray();
        hexMeshFilter.mesh.RecalculateNormals();
    }

    private void Triangulate(HexCell hexCell)
    {
        Vector3 center = hexCell.position;
        Vector3 centerTop = hexCell.position + new Vector3(0, HexMetrics.height, 0);

        for (int i = 0; i < 6; i++)
        {
            if (hexCell.neighborUp == null || !hexCell.neighborUp.isActive)
            {
                // Top face
                AddTriangle(
                    centerTop,
                    centerTop + HexMetrics.corners[i],
                    centerTop + HexMetrics.corners[i + 1]
                );
            }

            if (hexCell.neighborDown == null || !hexCell.neighborDown.isActive)
            {
                // Bottom face
                AddTriangle(
                    center + HexMetrics.corners[i + 1],
                    center + HexMetrics.corners[i],
                    center
                );
            }

            if (hexCell.GetNeighbor(i) == null || !hexCell.GetNeighbor(i).isActive)
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
        // Check to ensure we don't exceed array limits
        if (vertexIndex + 3 >= vertices.Length || triangleIndex + 3 >= triangles.Length)
        {
            Debug.LogError("HexMesh: Exceeded predefined array size. Increase the initial size estimate.");
            return;
        }

        vertices[vertexIndex] = v1;
        vertices[vertexIndex + 1] = v2;
        vertices[vertexIndex + 2] = v3;

        triangles[triangleIndex] = vertexIndex;
        triangles[triangleIndex + 1] = vertexIndex + 1;
        triangles[triangleIndex + 2] = vertexIndex + 2;

        vertexIndex += 3;
        triangleIndex += 3;
    }

    public void OnDestroy()
    {
        vertices.Dispose();
        triangles.Dispose();
    }
}
