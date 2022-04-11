using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Klasa za której pomocą można generować szum, oraz kontrolować jego parametry
*/
public class MapGenerator : MonoBehaviour
{
    // Enumerator określający czy rysować na ekranie monochromatyczną mapę szumu, czy pokolorowaną regionami, czy w formie modelu
    public enum DrawMode
    {
        NoiseMap,
        ColorMap,
        Mesh,
        FalloffMap
    }
    public DrawMode drawMode;

    // Wysokość mapy
    [Min(1)]
    public int mapWidth = 10;

    // Szerokość mapy
    [Min(1)]
    public int mapHeight = 10;

    // Skala szumu
    public float noiseScale = 0.3f;


    // Liczba różnych warstw szumu nałożonych na siebie, by uzyskać końcowy wynik
    [Min(0)]
    public int octaves = 1;

    // Persistance określa wpływ oktawy na kształt terenu. Pierwsze oktawy, które formują góry mają większy wpływ na teren
    // niż te wyższe, formujące nierówności na skalę małych skał.
    // Amplitude = persistence ^ 0 dla oktawy 1, Amplitude = persistence ^ 1 dla oktawy 2, itd.
    [Range(0, 1)]
    public float persistence = 0.5f;


    // Lacunarity - zwiększa detale generowane przez kolejne oktawy.
    // w przypadku 3 oktaw frequency = lacunarity ^ 0 dla 1 oktawy. Główny kształt gór.
    // frequency = lacunarity ^ 1 dla oktawy 2. Ta oktawa zwiększa trochę detal i generuje nierówności na skali skał, itd.
    [Min(1)]
    public float lacunarity = 2;

    public int seed;
    // Przesunięcie mapy zdefiniowane przez nas
    public Vector2 offset;
    // Używa, bądź nie używa opdu wartości przy przegu mapy
    public bool useFalloff;
    // Mnożnik wysokości modelu terenu
    public float meshHeightMultiplier = 1f;
    // Krzywa określająca na jakie wartości meshHeightMultiplier ma mieć wpływ bardziej, lub mniej
    public AnimationCurve meshHightCurve;

    // Automatycznie generuje mapę po zmianie wartości generatora
    public bool autoUpdate;

    // Tablica typów terenu używanych przez generator
    public TerrainType[] regions;

    // Funkcja wywoływana w celu generacji terenu
    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistence, lacunarity, offset);

        // Utwórz tablicę kolorów w celu przypisania kolorów do wartości wysokości mapy
        Color[] colorMap = new Color[mapWidth * mapHeight];
        // Mapa wartości dla opadu brzegowego
        float[,] falloffMap = FalloffGenerator.GenerateFalloffMap(mapWidth, mapHeight);

        // Pętla przez wszystkie uzyskane wartości mapy
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                if (useFalloff)
                {
                    // Zaaplikuj efekt opadu brzegowego
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);
                }

                // Pobierz obecną wartość mapy
                float currentHeight = noiseMap[x, y];

                // Sprawdź czy wartość mieści się w jakimś z regionów
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        // Przypisz odpowiedni kolor, gdy zostanie napotkany pierwszy sektor spełniający wymagania wysokości
                        colorMap[y * mapWidth + x] = regions[i].color;
                        break;
                    }
                }
            }
        }

        // Znajdź na scenie obiekt posiadający skrypt MapDisplay
        MapDisplay mapDisplay = FindObjectOfType<MapDisplay>();

        switch (drawMode)
        {
            case DrawMode.NoiseMap:
                // Rysuj surową mapę wysokości
                mapDisplay.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
                break;
            case DrawMode.ColorMap:
                // Rysuj mapę wysokości pokolorowaną regionami
                mapDisplay.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
                break;
            case DrawMode.Mesh:
                // Rysuj mapę w formie modelu
                mapDisplay.DrawMesh(
                    MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHightCurve),
                    TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight)
                    );
                break;
            case DrawMode.FalloffMap:
                // Rysuj mapę opdadu brzegowego
                mapDisplay.DrawTexture(
                    TextureGenerator.TextureFromHeightMap(
                        FalloffGenerator.GenerateFalloffMap(mapWidth, mapHeight)
                        )
                    );
                break;
        }
    }


    // Struktura reprezentująca część generowanego terenu
    [System.Serializable]
    public struct TerrainType
    {
        // Nazwa części terenu, jak góry, czy woda
        public string name;
        // Wysokość od jakiej teren ma być wyznaczany
        public float height;
        // Kolor na jaki kolorowany będzie teren zaczynając od wysokości
        public Color color;
    }
}
