using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FalloffGenerator
{
    // Generuj mapę wartości 1 blisko krawędzi przechodzących w wartości 0 do środka, co mieszając z mapą szumu
    // pozwala utworzyć wodę wokół mapy
    public static float[,] GenerateFalloffMap(int width, int height)
    {
        float[,] map = new float[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                // "i" i "j" to koordynaty na naszej mapie i chcemy te koordynaty przedstawić w skali -1 do 1
                float x = i / (float)width * 2 - 1;
                float y = j / (float)height * 2 - 1;

                // Znajdujemy który punkt jest bliżej krawędzi mapy
                float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
                map[i, j] = Evaluate(value);
            }
        }

        return map;
    }

    static float Evaluate(float value)
    {
        float a = 1.5f;
        float b = 5f;

        return Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b * value, a));
    }
}
