using UnityEngine;

namespace Assets.Scripts
{
    public static class GlobalNoise
    {
        public static float GetGlobalNoiseValue(float globalX, float globalZ, float[,] globalNoiseMap)
        {
            int noiseMapWidth = globalNoiseMap.GetLength(0);
            int noiseMapHeight = globalNoiseMap.GetLength(1);

            // Convert global coordinates to noise map coordinates
            int noiseMapX = Mathf.FloorToInt(globalX) % noiseMapWidth;
            int noiseMapZ = Mathf.FloorToInt(globalZ) % noiseMapHeight;

            // Ensure positive indices for modulo operation
            if (noiseMapX < 0) noiseMapX += noiseMapWidth;
            if (noiseMapZ < 0) noiseMapZ += noiseMapHeight;

            return globalNoiseMap[noiseMapX, noiseMapZ];
        }

    }
}