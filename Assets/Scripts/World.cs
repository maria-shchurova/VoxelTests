using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public int worldSize = 5; 
    public int chunkSize = 16;

    private HexChunk[,,] hexChunkGrid;
    private Dictionary<Vector3, HexChunk> chunks;

    public int noiseSeed = 1234;
    public float maxHeight = 0.2f;
    public float noiseScale = 0.015f;
    public float[,] noiseArray;

    public static World Instance { get; private set; }

    public Material baseMaterial;
    public bool autoUpdate;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: if you want this to persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }

        noiseArray = GlobalNoise.GetNoise();

        chunks = new Dictionary<Vector3, HexChunk>();

        GenerateWorld();
        AssignChunkNeighbors();
    }

    public void GenerateWorld()
    {
        hexChunkGrid = new HexChunk[worldSize, worldSize, worldSize];

        for (int x = 0; x < worldSize; x++)
        {
            for (int y = 0; y < worldSize; y++)
            {
                for (int z = 0; z < worldSize; z++)
                {
                    Vector3 chunkPosition = new Vector3(x * (HexMetrics.innerRadius * chunkSize * 2), y * chunkSize * HexMetrics.height, z * (HexMetrics.outerRadius * chunkSize * 1.5f));
                    GameObject newChunkObject = new GameObject($"Chunk_{x}_{y}_{z}");
                    newChunkObject.transform.position = chunkPosition;
                    newChunkObject.transform.parent = this.transform;

                    HexChunk newChunk = newChunkObject.AddComponent<HexChunk>();
                    HexMesh chunkMesh = newChunkObject.AddComponent<HexMesh>();
                    newChunkObject.AddComponent<MeshCollider>();
                    newChunkObject.AddComponent<MeshFilter>();
                    newChunkObject.AddComponent<MeshRenderer>().material = baseMaterial;

                    hexChunkGrid[x, y, z] = newChunk;

                    newChunk.Initialize(chunkSize, chunkMesh);
                    chunks.Add(chunkPosition, newChunk);
                }
            }
        }
    }

    private void AssignChunkNeighbors()
    {
        for (int x = 0; x < worldSize; x++)
        {
            for (int y = 0; y < worldSize; y++)
            {
                for (int z = 0; z < worldSize; z++)
                {
                    HexChunk currentChunk = hexChunkGrid[x, y, z];

                    // x direction neighbor
                    if (x + 1 < worldSize)
                    {
                        currentChunk.SetNeighbor(0, hexChunkGrid[x + 1, y, z]);
                    }
                    // -x direction neighbor
                    if (x - 1 >= 0)
                    {
                        currentChunk.SetNeighbor(1, hexChunkGrid[x - 1, y, z]);
                    }
                    // y direction neighbor
                    if (y + 1 < worldSize)
                    {
                        currentChunk.SetNeighbor(2, hexChunkGrid[x, y + 1, z]);
                    }
                    // -y direction neighbor
                    if (y - 1 >= 0)
                    {
                        currentChunk.SetNeighbor(3, hexChunkGrid[x, y - 1, z]);
                    }
                    // z direction neighbor
                    if (z + 1 < worldSize)
                    {
                        currentChunk.SetNeighbor(4, hexChunkGrid[x, y, z + 1]);
                    }
                    // -z direction neighbor
                    if (z - 1 >= 0)
                    {
                        currentChunk.SetNeighbor(5, hexChunkGrid[x, y, z - 1]);
                    }
                }
            }
        }

    }
}
