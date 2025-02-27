using GLTFast.Schema;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class PerlinCircle : MonoBehaviour
{
    public int seed = 0;
    public int width = 50;
    public int height = 50;

    void Start()
    {
        seed = Random.Range(0, int.MaxValue);
        Random.InitState(seed);
        Texture2D texture = GenerateOvalTexture(500, height, width,new Vector2(250, 250));
        GetEdge(texture);
        ApplyTexture(texture);
    }

    Texture2D GenerateCircleTexture(int size)
    {
        Texture2D texture = new Texture2D(size, size);
        Color white = Color.white;
        Color black = Color.black;
        Vector2 center = new Vector2(size / 2, size / 2);
        float radius = 200 / 2;
        float noiseOffsetX = Random.value * 1000;
        float noiseOffsetY = Random.value * 1000;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float xCoord = noiseOffsetX + (float)x / 256 * 5;
                float yCoord = noiseOffsetY + (float)y / 256 * 5;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                float bumpyRadius = radius + (sample - 0.5f) * 20; // Adjusting bumpiness

                float distance = Vector2.Distance(new Vector2(x, y), center);
                texture.SetPixel(x, y, distance <= bumpyRadius ? black : white);
            }
        }

        texture.Apply();
        return texture;
    }

    Texture2D GenerateOvalTexture(int size, float a, float b, Vector2 position)
    {
        Texture2D texture = new Texture2D(size, size);
        Color white = Color.white;
        Color black = Color.black;

        float noiseOffsetX = Random.value * 1000;
        float noiseOffsetY = Random.value * 1000;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float nx = (x - position.x) / a;
                float ny = (y - position.y) / b;

                float xCoord = noiseOffsetX + (float)x / 256 * 5;
                float yCoord = noiseOffsetY + (float)y / 256 * 5;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);

                float noiseFactor = (sample - 0.5f) * 0.75f; // Small variation to avoid over-distortion
                float ovalEquation = (nx * nx) + (ny * ny) - noiseFactor;

                if (ovalEquation <= 1)
                    texture.SetPixel(x, y, black);
                else
                    texture.SetPixel(x, y, white);
            }
        }

        texture.Apply();
        return texture;
    }

    void GetEdge(Texture2D texture2D)
    {
        for (int y = 0; y < texture2D.height; y++)
        {
            for (int x = 0; x < texture2D.width; x++)
            {
                if (texture2D.GetPixel(x, y) == Color.white)
                {
                    bool isEdge = false;

                    // Check right pixel
                    if (x + 1 < texture2D.width && texture2D.GetPixel(x + 1, y) == Color.black)
                        isEdge = true;

                    // Check left pixel
                    if (x - 1 >= 0 && texture2D.GetPixel(x - 1, y) == Color.black)
                        isEdge = true;

                    // Check bottom pixel
                    if (y + 1 < texture2D.height && texture2D.GetPixel(x, y + 1) == Color.black)
                        isEdge = true;

                    // Check top pixel
                    if (y - 1 >= 0 && texture2D.GetPixel(x, y - 1) == Color.black)
                        isEdge = true;

                    if (isEdge)
                    {
                        texture2D.SetPixel(x, y, Color.green);
                    }
                }
            }
        }
        texture2D.Apply(); // Apply changes to the texture
    }



    void ApplyTexture(Texture2D texture)
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.mainTexture = texture;
        }
    }
}
