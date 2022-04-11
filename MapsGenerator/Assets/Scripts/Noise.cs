using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Odpowiada tylko za generowanie dwuwymiarowej tablicy z wartościami szumu
*/
public static class Noise
{
    public static float[,] GenerateNoiseMap(
        int mapWidth,
        int mapHeight,
        int seed,
        float scale,
        int octaves,
        float persistence,
        float lacunarity,
        Vector2 offset)
    {
        // Utworzenie dwuwymiarowej tablicy szumów, która będzie zwracana
        float[,] noiseMap = new float[mapWidth, mapHeight];

        // Generator liczb pseudolosowych na podstawie podanego seed
        System.Random prng = new System.Random(seed);

        // Otrzymywanie przesunięcia w szumie dla każdej oktawy na podstawie liczb generowanych przez seed
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            // -100000 do 100000 by nie podawać zbyt wysokich wartości do szumu perlina, ponieważ zwraca nieoczekiwane rezultaty
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        // Skala nie może być równa zero, ponieważ występuje dzielenie przez zero
        if (scale <= 0) scale = 0.0001f;

        // max i min noiseHeight służą do śledzenie maksymalnej i minimalnej otrzymanej wartości szumu.
        // Wykorzystywane jest potem do normalizacji mapy, czyli do powrotu do zakresu wartości 0-1
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        // Połowa szerokości i wysokości mapy. Potrzebna do uzyskania efektu skalowania mapy do środka zamiast prawego-górnego rogu
        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;


        // Pętla dla każdego piksela szumu
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                // Modyfikator wysokości terenu dla konkretnej warstwy, zaczynamy od 1, czyli nie modyfikuj
                float amplitude = 1;
                // Modyfikator częstotliwości szumu danej warstwy
                float frequency = 1;
                // Wartość szumu dla konkretnego koordynatu pętli. Zaczynamy od 0 i dodajemy do tego
                float noiseHeight = 0;

                // Pętla przez wszystkie oktawy (warstwy terenu)
                for (int i = 0; i < octaves; i++)
                {
                    // Modyfikacja koordynatów szumu o dodatkowe czynniki jak skala, czy częstotliwość
                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x * frequency;
                    float sampleY = (y - halfHeight) / scale * frequency - octaveOffsets[i].y * frequency;

                    // Wartość szumu perlina w punkcie wyznaczonym przez sample
                    // wartość * 2 - 1, oznacza że otrzymany szum to wartości od -1 do 1
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;

                    // Dodawaj wartości szumu dla konkretnych warstw, by uzyskać ostateczną wartość dla danego piksela
                    noiseHeight += perlinValue * amplitude;

                    // persistence to wartości 0-1, więc z każdą oktawą wpływ szumu na teren zanika z powodu
                    // zmniejszającej się zmiennej amplitude
                    amplitude *= persistence;

                    // lacunarity powinno być większe od 1, więc frequency (częstotliwość) zwiększa się z każdą kolejną oktawą
                    frequency *= lacunarity;
                }

                // Zapisanie największej i najmniejszej otrzymanej wartości mapy szumu
                if (noiseHeight > maxNoiseHeight) maxNoiseHeight = noiseHeight;
                if (noiseHeight < minNoiseHeight) minNoiseHeight = noiseHeight;

                // Przypisz otrzymaną wartość wysokości mapy w pikselu do końcowej mapy szumów
                noiseMap[x, y] = noiseHeight;
            }
        }

        // Pętla przez całą mapę szumów
        // Normalizacja mapy tak, by każda wartość znajdowała się w zakresie 0-1
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                // InverseLepr zwraca wartość pomiędzy 0 a 1 bazując na minNoiseHeight jako 0 i maxNoiseHeight jako 1
                // dla wartości mapy szumów na podanych koordynatach
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        // Po ustawieniu każdej wartości na mapie i normalizacji, zwróć mapę
        return noiseMap;
    }
}
