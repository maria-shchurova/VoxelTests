using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplexNoiseDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    public void Display()
    {
        float[,] noiseMap = World.Instance.noiseArray;
        MapDisplay display = GetComponent<MapDisplay>();

        display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
