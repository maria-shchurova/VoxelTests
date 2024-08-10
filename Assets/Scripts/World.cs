using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class World : MonoBehaviour
{
    public NativeArray<float3> hexChunkPosiitonsArray;

    public int worldSize = 5; 
    public int chunkSize = 16;

    public int mapWidth;
    public int mapHeight;
    public float noiseScale;
    public float maxHeight;

    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public float[,] noiseArray;

    public bool autoUpdate;

    public static World Instance { get; private set; }

    public Material baseMaterial;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            Generate();
        }
    }

    void Generate()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        GenerateWorld();
        //AssignChunkNeighbors();
    }

    public void GenerateWorld()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        noiseArray = SLNoise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);
        hexChunkPosiitonsArray = new NativeArray<float3>(worldSize * worldSize * worldSize, Allocator.Persistent);

        CreateChunksJob createJob = new CreateChunksJob
        {
            WorldSize = worldSize,
            ChunkSize = chunkSize,
            HexChunkPosiitonsArray = hexChunkPosiitonsArray
        };

        JobHandle createHandle = createJob.Schedule();
        createHandle.Complete();

        foreach(float3 position in hexChunkPosiitonsArray)
        {
            GameObject newChunkObject = new GameObject($"Chunk_{position.x}_{position.y}_{position.z}");
            newChunkObject.transform.position = new Vector3(position.x, position.y, position.z);
            newChunkObject.transform.parent = this.transform;
            HexChunk newChunk = newChunkObject.AddComponent<HexChunk>();
            HexMesh chunkMesh = newChunkObject.AddComponent<HexMesh>();
            //newChunkObject.AddComponent<MeshCollider>();
            newChunkObject.AddComponent<MeshFilter>();
            newChunkObject.AddComponent<MeshRenderer>().material = baseMaterial;

            newChunk.Initialize(chunkSize, chunkMesh);
        }

    }

    public void OnDestroy()
    {
        hexChunkPosiitonsArray.Dispose();
    }


    //private void AssignChunkNeighbors()
    //{
    //    for (int x = 0; x < worldSize; x++)
    //    {
    //        for (int y = 0; y < worldSize; y++)
    //        {
    //            for (int z = 0; z < worldSize; z++)
    //            {
    //                HexChunk currentChunk = hexChunkGrid[x, y, z];

    //                // x direction neighbor
    //                if (x + 1 < worldSize)
    //                {
    //                    currentChunk.SetNeighbor(0, hexChunkGrid[x + 1, y, z]);
    //                }
    //                // -x direction neighbor
    //                if (x - 1 >= 0)
    //                {
    //                    currentChunk.SetNeighbor(1, hexChunkGrid[x - 1, y, z]);
    //                }
    //                // y direction neighbor
    //                if (y + 1 < worldSize)
    //                {
    //                    currentChunk.SetNeighbor(2, hexChunkGrid[x, y + 1, z]);
    //                }
    //                // -y direction neighbor
    //                if (y - 1 >= 0)
    //                {
    //                    currentChunk.SetNeighbor(3, hexChunkGrid[x, y - 1, z]);
    //                }
    //                // z direction neighbor
    //                if (z + 1 < worldSize)
    //                {
    //                    currentChunk.SetNeighbor(4, hexChunkGrid[x, y, z + 1]);
    //                }
    //                // -z direction neighbor
    //                if (z - 1 >= 0)
    //                {
    //                    currentChunk.SetNeighbor(5, hexChunkGrid[x, y, z - 1]);
    //                }
    //            }
    //        }
    //    }

    //}
}
