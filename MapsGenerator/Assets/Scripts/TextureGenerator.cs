using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerator
{
    public static Texture2D TextureFromColorMap(Color[] colorMap, int width, int height)
    {
        // Utwórz nową teksturę o podanych wymiarach
        Texture2D texture = new Texture2D(width, height);

        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;

        // Zaaplikuj mapę kolorów do utworzonej tekstury
        texture.SetPixels(colorMap);
        texture.Apply();

        // Zwróć teksturę
        return texture;
    }

    public static Texture2D TextureFromHeightMap(float[,] heightMap)
    {
        // Pobierz szerokość mapy przez pobranie rozmiaru tablicy pierwszego wymiaru
        int width = heightMap.GetLength(0);
        // Pobierz wysokość mapy przez pobranie rozmiaru tablicy drugiego wymiaru
        int height = heightMap.GetLength(1);

        // Utwórz nową teksturę o wymiarach mapy szumu
        Texture2D texture = new Texture2D(width, height);

        // Tablica kolorów dla każdego piksela w mapie szumów.
        Color[] colorMap = new Color[width * height];

        // Przejdź przez wszystkie piksele na mapie szumu
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Przypisz odpowiednim elementom mapy kolorów kolor określany przez odpowiadający
                // koordynat na mapie szumów w zakresie 0-1, gdzie 0 - czarny, 1 - biały
                colorMap[width * y + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
            }
        }

        // Zwróć rezultat transformacji otrzymanej mapy kolorów w teksturę
        return TextureFromColorMap(colorMap, width, height);
    }
}
