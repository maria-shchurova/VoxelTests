using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseFromTexture 
{
    public static float[,] Convert(Texture2D heightMap)
    {
        float[,] heights = new float[heightMap.width, heightMap.height];
        Color[] pixels = heightMap.GetPixels();
        for (int x = 0; x < heightMap.width; x++)
        {
            for (int y = 0; y < heightMap.height; y++)
            {
                int index = x + y * heightMap.width;
                heights[x, y] = pixels[index].grayscale;
            }
        }
        return heights;
    }
}
