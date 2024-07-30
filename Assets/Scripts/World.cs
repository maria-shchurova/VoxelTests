using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public int worldSize = 5; 
    public int chunkSize = 16;

    private Dictionary<Vector3, HexChunk> chunks;

    public int noiseSeed = 1234;
    public float maxHeight = 0.2f;
    public float noiseScale = 0.015f;
    public float[,] noiseArray;

    public static World Instance { get; private set; }

    public Material baseMaterial;

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
    }

    private void GenerateWorld()
    {
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

                    newChunk.Initialize(chunkSize, chunkMesh);
                    chunks.Add(chunkPosition, newChunk);
                }
            }
        }
    }
}
