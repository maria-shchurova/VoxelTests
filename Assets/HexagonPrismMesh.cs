using UnityEngine;

public class HexagonPrismMesh : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Mesh mesh;

    void Awake()
    {
        meshFilter = gameObject.GetComponent<MeshFilter>();
        mesh = new Mesh();
        meshFilter.mesh = mesh;
        //CreateHexagonPrismMesh();
    }

    //void CreateHexagonPrismMesh()
    //{
    //    float height = 1f;

    //    Vector3[] vertices = new Vector3[14];

    //    // Bottom hexagon
    //    for (int i = 0; i < 6; i++)
    //    {
    //        float angle = Mathf.PI / 3 * i;
    //        vertices[i] = new Vector3(HexMetrics.outerRadius * Mathf.Cos(angle), 0, HexMetrics.outerRadius * Mathf.Sin(angle));
    //    }

    //    // Top hexagon
    //    for (int i = 0; i < 6; i++)
    //    {
    //        vertices[i + 6] = vertices[i] + Vector3.up * height;
    //    }

    //    // Center points
    //    vertices[12] = Vector3.zero;
    //    vertices[13] = Vector3.up * height;

    //    int[] triangles = new int[72];

    //    // Bottom face
    //    for (int i = 0; i < 6; i++)
    //    {
    //        triangles[3 * i] = 12;
    //        triangles[3 * i + 1] = i;
    //        triangles[3 * i + 2] = (i + 1) % 6;
    //    }

    //    // Top face
    //    for (int i = 0; i < 6; i++)
    //    {
    //        triangles[18 + 3 * i] = 13;
    //        triangles[18 + 3 * i + 1] = 6 + (i + 1) % 6;
    //        triangles[18 + 3 * i + 2] = 6 + i;
    //    }

    //    // Side faces
    //    for (int i = 0; i < 6; i++)
    //    {
    //        int next = (i + 1) % 6;

    //        // First triangle
    //        triangles[36 + 6 * i] = i;
    //        triangles[36 + 6 * i + 1] = 6 + i;
    //        triangles[36 + 6 * i + 2] = next;

    //        // Second triangle
    //        triangles[36 + 6 * i + 3] = next;
    //        triangles[36 + 6 * i + 4] = 6 + i;
    //        triangles[36 + 6 * i + 5] = 6 + next;
    //    }

    //    // UVs
    //    Vector2[] uvs = new Vector2[14];

    //    // Bottom face UVs
    //    for (int i = 0; i < 6; i++)
    //    {
    //        float angle = Mathf.PI / 3 * i;
    //        uvs[i] = new Vector2(0.5f + Mathf.Cos(angle) * 0.5f, 0.5f + Mathf.Sin(angle) * 0.5f);
    //    }
    //    uvs[12] = new Vector2(0.5f, 0.5f); // Center of bottom face

    //    // Top face UVs
    //    for (int i = 0; i < 6; i++)
    //    {
    //        float angle = Mathf.PI / 3 * i;
    //        uvs[i + 6] = new Vector2(0.5f + Mathf.Cos(angle) * 0.5f, 0.5f + Mathf.Sin(angle) * 0.5f);
    //    }
    //    uvs[13] = new Vector2(0.5f, 0.5f); // Center of top face

    //    // Assigning data to the mesh
    //    mesh.vertices = vertices;
    //    mesh.triangles = triangles;
    //    mesh.uv = uvs;
    //    mesh.RecalculateNormals();
    //}
}
