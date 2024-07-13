using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;

    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private List<Vector2> uvs = new List<Vector2>();

    private Voxel[,,] voxels;
    private int chunkSize = 16;
    private Color gizmoColor;

    void Start()
    {
        // Initialize Mesh Components
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshCollider = gameObject.AddComponent<MeshCollider>();

        // Call this to generate the chunk mesh
        GenerateMesh();
    }

    private void GenerateMesh()
    {
        IterateVoxels(); // Make sure this processes all voxels

        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();

        mesh.RecalculateNormals(); // Important for lighting

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;

        meshRenderer.material = World.Instance.VoxelMaterial;
    }

    private void InitializeVoxels()
    {
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    voxels[x, y, z] = new Voxel(transform.position + new Vector3(x, y, z), Color.white);
                }
            }
        }
    }
    public void Initialize(int size)
    {
        this.chunkSize = size;
        voxels = new Voxel[size, size, size];
        InitializeVoxels();
    }

    public void IterateVoxels()
    {
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    ProcessVoxel(x, y, z);
                }
            }
        }
    }

    private void ProcessVoxel(int x, int y, int z)
    {
        // Check if the voxels array is initialized and the indices are within bounds
        if (voxels == null || x < 0 || x >= voxels.GetLength(0) ||
            y < 0 || y >= voxels.GetLength(1) || z < 0 || z >= voxels.GetLength(2))
        {
            return; 
        }

        Voxel voxel = voxels[x, y, z];
        if (voxel.isActive)
        {
            // Check each face of the voxel for visibility
            bool[] facesVisible = new bool[6];

            // Check visibility for each face
            facesVisible[0] = IsFaceVisible(x, y + 1, z); // Top
            facesVisible[1] = IsFaceVisible(x, y - 1, z); // Bottom
            facesVisible[2] = IsFaceVisible(x - 1, y, z); // Left
            facesVisible[3] = IsFaceVisible(x + 1, y, z); // Right
            facesVisible[4] = IsFaceVisible(x, y, z + 1); // Front
            facesVisible[5] = IsFaceVisible(x, y, z - 1); // Back

            for (int i = 0; i < facesVisible.Length; i++)
            {
                if (facesVisible[i])
                    AddFaceData(x, y, z, i); // Method to add mesh data for the visible face
            }
        }
    }

    private bool IsFaceVisible(int x, int y, int z)
    {
        // Check if the neighboring voxel in the given direction is inactive or out of bounds
        if (x < 0 || x >= chunkSize || y < 0 || y >= chunkSize || z < 0 || z >= chunkSize)
            return true; // Face is at the boundary of the chunk
        return !voxels[x, y, z].isActive;
    }

    private void AddFaceData(int x, int y, int z, int faceIndex)
    {
        // Based on faceIndex, determine vertices and triangles
        // Add vertices and triangles for the visible face
        // Calculate and add corresponding UVs

        if (faceIndex == 0) // Top Face
        {
            vertices.Add(new Vector3(x, y + 1, z));
            vertices.Add(new Vector3(x, y + 1, z + 1));
            vertices.Add(new Vector3(x + 1, y + 1, z + 1));
            vertices.Add(new Vector3(x + 1, y + 1, z));
            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(1, 0));
            uvs.Add(new Vector2(1, 1));
            uvs.Add(new Vector2(0, 1));
        }

        if (faceIndex == 1) // Bottom Face
        {
            vertices.Add(new Vector3(x, y, z));
            vertices.Add(new Vector3(x + 1, y, z));
            vertices.Add(new Vector3(x + 1, y, z + 1));
            vertices.Add(new Vector3(x, y, z + 1));
            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(0, 1));
            uvs.Add(new Vector2(1, 1));
            uvs.Add(new Vector2(1, 0));
        }

        if (faceIndex == 2) // Left Face
        {
            vertices.Add(new Vector3(x, y, z));
            vertices.Add(new Vector3(x, y, z + 1));
            vertices.Add(new Vector3(x, y + 1, z + 1));
            vertices.Add(new Vector3(x, y + 1, z));
            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(0, 1));
            uvs.Add(new Vector2(0, 1));
        }

        if (faceIndex == 3) // Right Face
        {
            vertices.Add(new Vector3(x + 1, y, z + 1));
            vertices.Add(new Vector3(x + 1, y, z));
            vertices.Add(new Vector3(x + 1, y + 1, z));
            vertices.Add(new Vector3(x + 1, y + 1, z + 1));
            uvs.Add(new Vector2(1, 0));
            uvs.Add(new Vector2(1, 1));
            uvs.Add(new Vector2(1, 1));
            uvs.Add(new Vector2(1, 0));
        }

        if (faceIndex == 4) // Front Face
        {
            vertices.Add(new Vector3(x, y, z + 1));
            vertices.Add(new Vector3(x + 1, y, z + 1));
            vertices.Add(new Vector3(x + 1, y + 1, z + 1));
            vertices.Add(new Vector3(x, y + 1, z + 1));
            uvs.Add(new Vector2(0, 1));
            uvs.Add(new Vector2(0, 1));
            uvs.Add(new Vector2(1, 1));
            uvs.Add(new Vector2(1, 1));
        }

        if (faceIndex == 5) // Back Face
        {
            vertices.Add(new Vector3(x + 1, y, z));
            vertices.Add(new Vector3(x, y, z));
            vertices.Add(new Vector3(x, y + 1, z));
            vertices.Add(new Vector3(x + 1, y + 1, z));
            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(1, 0));
            uvs.Add(new Vector2(1, 0));
            uvs.Add(new Vector2(0, 0));

        }
        AddTriangleIndices();
    }

    private void AddTriangleIndices()
    {
        int vertCount = vertices.Count;

        // First triangle
        triangles.Add(vertCount - 4);
        triangles.Add(vertCount - 3);
        triangles.Add(vertCount - 2);

        // Second triangle
        triangles.Add(vertCount - 4);
        triangles.Add(vertCount - 2);
        triangles.Add(vertCount - 1);
    }

    //void OnDrawGizmos()
    //{
    //    if (voxels != null)
    //    {
    //        for (int x = 0; x < chunkSize; x++)
    //        {
    //            for (int y = 0; y < chunkSize; y++)
    //            {
    //                for (int z = 0; z < chunkSize; z++)
    //                {
    //                    if (voxels[x, y, z].isActive)
    //                    {
    //                        gizmoColor = new Color(Random.value, Random.value, Random.value, 0.4f); // Semi-transparent

    //                        Gizmos.color = gizmoColor; 
    //                        Gizmos.DrawCube(transform.position + new Vector3(x, y, z), Vector3.one);
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}
}