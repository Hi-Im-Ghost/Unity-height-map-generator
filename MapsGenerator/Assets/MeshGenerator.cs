using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve)
    {
        // Pobranie szerokości i wysokości terenu
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        // Pobranie górnej-lewej wartości terenu dla X i Z w celu wyśrodkowania terenu na ekranie przy generacji
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        // Utworzenie obiektu generującego dane potrzebne do utworzenia modelu terenu
        MeshData meshData = new MeshData(width, height);
        // Indeks obecnego werteksa na jakim pracuje pętla
        int vertexIndex = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Przypisywanie wartości szumu do odpowiednich werteksów terenu, razem z wyśrodkowaniem
                //meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, heightMap[x, y], topLeftZ - y);
                meshData.vertices[vertexIndex] = new Vector3(
                    topLeftX + x,
                    heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier,
                    topLeftZ - y);
                // Przypisanie odpowiedniej znormalizowanej pozycji wektora to tablicy UV
                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                // Ten if jest potrzebny by omijać skrajne werteksy przy tworzeniu trójkątów.
                // Za jednym zamachem tworzymy 2 trójkąty, które razem tworzą kwadrat po prawej-dolnej stronie obecnego werteksa.
                // To znaczy, że ignorujemy wszystkie werteksy po skrajnej prawej i na samym dole, by nie próbować tworzyć
                // kwadratów poza terenem
                if (x < width - 1 && y < height - 1)
                {
                    meshData.AddTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                    meshData.AddTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                    //meshData.AddTriangle(vertexIndex, vertexIndex + width, vertexIndex + width + 1);
                    //meshData.AddTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }
        return meshData;
    }
}

// Klasa przechowująca dane terenu tworzonego z trójkątów
public class MeshData
{
    // Tablica werteksów terenu
    public Vector3[] vertices;
    // Tablica int trójkątów. Każde 3 kolejne wartości w tej tablicy wskazują na odpowiednie werteksy tworzące jeden trójkąt.
    public int[] triangles;

    // Dane UV mapy potrzebne do nałożenia tekstury na teren.
    // Określa pozycję dla każdego werteksa w odniesieniu do całej mapy w skali 0-1
    // Werteksy w lewym-dolnym rogu mają (0,0), na środku (0.5,0.5) w prawym-górnym (1,1), itd.
    public Vector2[] uvs;

    // Obecny indeks tablicy trójkątów
    int triangleIndex;

    // Konstruktor
    public MeshData(int meshWidth, int meshHeight)
    {
        // Inicjalizacja tablicy werteksów do wielkości pola powierzchni terenu
        vertices = new Vector3[meshWidth * meshHeight];
        // Jeden wektor dla jednego werteksa na mapie
        uvs = new Vector2[meshWidth * meshHeight];
        // (szerokosc - 1) * (wysokosc - 1) oblicza ile kwadratów zostanie utworzonych w obrębie terenu
        // wynik * 6, ponieważ każdy kwadrat to 2 trójkąty, a każdy trójkąt to 3 werteksy co daje 6 werteksów na każdy kwadrat
        triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];

    }

    public void AddTriangle(int a, int b, int c)
    {
        // Przypisanie trzech kolejnych werteksów tworzących trójkąt
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;

        // Przesunięcie indeksu o 3, umożliwiając przypisanie wartości dla nowego trójkąta
        triangleIndex += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        // Żeby oświetlenie się nie bugowało
        mesh.RecalculateNormals();

        return mesh;
    }
}
