using UnityEngine;

public class HexMesh : MonoBehaviour
{
    private Vector3[] vertices;
    private int[] triangles;

    private int vertexIndex = 0;
    private int triangleIndex = 0;

    private HexChunk hexGrid;
    [SerializeField]
    private MeshFilter hexMeshFilter;

    public void Initialize(HexChunk grid)
    {
        hexGrid = grid;

        hexMeshFilter.mesh = new Mesh();
        hexMeshFilter.mesh.name = "Hex Mesh";

        // Predefine sizes (estimate based on grid size)
        int maxVertices = grid.size * grid.size * grid.size * 6 * 4; // Estimate based on the number of hex cells
        int maxTriangles = maxVertices * 3; // Estimate based on the number of faces

        vertices = new Vector3[maxVertices];
        triangles = new int[maxTriangles];
    }

    public void Clear()
    {
        hexMeshFilter.mesh.Clear(false);

        // Reset indices and clear mesh data
        vertexIndex = 0;
        triangleIndex = 0;
        vertices = new Vector3[vertices.Length]; // Reset the arrays
        triangles = new int[triangles.Length];
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

        // Trim the arrays to only include used data
        Vector3[] trimmedVertices = new Vector3[vertexIndex];
        int[] trimmedTriangles = new int[triangleIndex];

        System.Array.Copy(vertices, trimmedVertices, vertexIndex);
        System.Array.Copy(triangles, trimmedTriangles, triangleIndex);

        hexMeshFilter.mesh.vertices = trimmedVertices;
        hexMeshFilter.mesh.triangles = trimmedTriangles;
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
}
