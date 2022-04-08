using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer textureRenderer;

    public void DrawNoiseMap(float[,] noiseMap) {
        // Pobierz szerokośc szumu
        int width = noiseMap.GetLength(0);
        // Pobierz wysokość szumu
        int height = noiseMap.GetLength(1);

        // Utworzenie tekstury o wysokości i szerokości szumu
        Texture2D texture = new Texture2D(width, height);

        // Tablica kolorów przechowująca kolor każdego piksela w teksturze
        Color[] colorMap = new Color[width * height];

        // Przypisz tablicy kolorów wartości z szumu: 0-czarny 1-biały
        for (int y=0; y < height; y++)
            for (int x=0; x < width; x++)
            {
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x,y]);
            }
        
        // Pokoloruj teksturę wartościami z tablicy kolorów
        texture.SetPixels(colorMap);
        texture.Apply();

        // SharedMaterial, bo nie trzeba uruchamiać programu żeby zobaczyć wynik
        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(width, 1, height);
    }
}
