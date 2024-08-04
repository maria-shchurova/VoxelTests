using SimplexNoise;
using UnityEngine;

public static class GlobalNoise
{
    public static float[,] GetNoise()
    {
        Noise.Seed = World.Instance.noiseSeed;
        // The number of points to generate in the 1st and 2nd dimension
        int width = World.Instance.chunkSize * World.Instance.worldSize;
        int height = World.Instance.chunkSize * World.Instance.worldSize;
        // The scale of the noise. The greater the scale, the denser the noise gets
        float scale = World.Instance.noiseScale;
        float[,] noise = Noise.Calc2D(width, height, scale); // Returns an array containing 2D Simplex noise

        return noise;
    }

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