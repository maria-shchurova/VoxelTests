public static class GlobalNoise
{
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