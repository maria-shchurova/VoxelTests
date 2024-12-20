using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts
{
    public class World : MonoBehaviour
    {
        [SerializeField]
        private GameObject ChunkPrefab;
        private NativeArray<float3> hexChunkPosiitonsArray;
        public bool GenerateFroHeightmap;

        [SerializeField]
        private Texture2D Heightmap;
        [SerializeField]
        private int worldSize = 5;
        public int chunkSize = 16;
        [SerializeField]
        private int mapWidth;
        [SerializeField]
        private int mapHeight;
        [SerializeField]
        private float noiseScale;
        public float maxHeight;
        [SerializeField]
        private int octaves;
        [SerializeField]
        [Range(0, 1)]
        private float persistance;
        [SerializeField]
        private float lacunarity;
        [SerializeField]
        private int seed;
        [SerializeField]
        private Vector2 offset;

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

        private void Generate()
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

            if(GenerateFroHeightmap)
            {
                if (Heightmap == null)
                    return;
                noiseArray = NoiseFromTexture.Convert(Heightmap);
            }
            else
            {
                noiseArray = SLNoise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);
            }

            hexChunkPosiitonsArray = new NativeArray<float3>(worldSize * worldSize * worldSize, Allocator.Persistent);

            CreateChunksJob createJob = new CreateChunksJob
            {
                WorldSize = worldSize,
                ChunkSize = chunkSize,
                HexChunkPosiitonsArray = hexChunkPosiitonsArray
            };

            JobHandle createHandle = createJob.Schedule();
            createHandle.Complete();

            for(int i = 0; i < hexChunkPosiitonsArray.Length; i++)
            {
                GameObject newChunkObject = Instantiate(ChunkPrefab, transform);
                newChunkObject.transform.position = new Vector3(hexChunkPosiitonsArray[i].x, hexChunkPosiitonsArray[i].y, hexChunkPosiitonsArray[i].z);
                newChunkObject.GetComponent<HexChunk>().Initialize();
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
}
