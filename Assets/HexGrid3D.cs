using UnityEngine;

public class HexGrid3D : MonoBehaviour
{
    public GameObject hexPrefab;
    public int gridWidth = 10;
    public int gridHeight = 10;
    public int gridDepth = 10;
    private float hexWidth;
    private float hexHeight;
    private float hexDepth;

    void Start()
    {
        if (hexPrefab == null)
        {
            Debug.LogError("Hex Prefab is not set.");
            return;
        }
        GameObject tempHex = Instantiate(hexPrefab);
        Mesh hexMesh = tempHex.GetComponent<MeshFilter>().sharedMesh;

        hexWidth = hexMesh.bounds.size.x;
        hexHeight = hexMesh.bounds.size.z;
        hexDepth = hexMesh.bounds.size.y;

        Destroy(tempHex);
        GenerateHexGrid3D();
    }

    void GenerateHexGrid3D()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                for (int z = 0; z < gridDepth; z++)
                {
                    float xOffset = (x % 2 == 0) ? 0 : hexWidth * 0.5f;
                    float zOffset = (z % 2 == 0) ? 0 : hexDepth * 0.5f;
                    Vector3 position = new Vector3(x * hexWidth * 0.75f, z * hexDepth * 0.75f, y * hexHeight + xOffset + zOffset);
                    Instantiate(hexPrefab, position, Quaternion.identity, transform);
                }
            }
        }
    }
}