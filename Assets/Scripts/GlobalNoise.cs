using System.Collections;
using SimplexNoise;
using UnityEngine;
using Zenject;

public static class GlobalNoise
{
    public static float[,] GetNoise()
    {
        Noise.Seed = HexGrid.Instance.noiseSeed;
        // The number of points to generate in the 1st and 2nd dimension
        int width = HexGrid.Instance.size * HexGrid.Instance.size;
        int height = HexGrid.Instance.size * HexGrid.Instance.size;
        // The scale of the noise. The greater the scale, the denser the noise gets
        float scale = HexGrid.Instance.noiseScale;
        float[,] noise = Noise.Calc2D(width, height, scale); // Returns an array containing 2D Simplex noise

        return noise;
    }

    public static float GetGlobalNoiseValue(float globalX, float globalZ, float[,] globalNoiseMap)
    {
        // Convert global coordinates to noise map coordinates
        int noiseMapX = (int)globalX % globalNoiseMap.GetLength(0);
        int noiseMapZ = (int)globalZ % globalNoiseMap.GetLength(1);

        // Ensure that the coordinates are within the bounds of the noise map
        if (
            noiseMapX >= 0 && noiseMapX < globalNoiseMap.GetLength(0) &&
            noiseMapZ >= 0 && noiseMapZ < globalNoiseMap.GetLength(1))
        {
            return globalNoiseMap[noiseMapX, noiseMapZ];
        }
        else
        {
            return 0; // Default value if out of bounds
        }
    }
}