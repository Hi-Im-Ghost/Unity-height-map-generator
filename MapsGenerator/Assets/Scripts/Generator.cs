using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    private Terrain terrain;

    private void Awake()
    {
        terrain = GetComponent<Terrain>();
    }

    void Update()
    {
        //Ustawiamy w zmiennej dane terenu, kt�re zostan� wygenerowane 
        terrain.terrainData = GenerateTerrain(terrain.terrainData);

    }

    //Metoda do generowanie danych terenu 
    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        //Ustawienie rozdzielczo�ci dla mapy terenu
        terrainData.heightmapResolution = Manager.Instance.width + 1;
        //Ustawianie wymiar�w terenu
        terrainData.size = new Vector3(Manager.Instance.width, Manager.Instance.depth, Manager.Instance.height);

        //Ustawiamy tablice mapy wysoko�ci. Zakres 0-1
        terrainData.SetHeights(0, 0, GenerateHeights());

        return terrainData;
    }

    //Generowanie wysoko�ci za pomoc� szumu
    static float[,] GenerateHeights()
    {
        //utworzenie dwuwymiarowej tablicy float
        float[,] heights = new float[Manager.Instance.width, Manager.Instance.height];
        //przechodzimy p�tlami przez ka�dy punkt by ustawi� warto�� z szumu
        for (int x = 0; x < Manager.Instance.width; x++)
        {
            for (int y = 0; y < Manager.Instance.height; y++)
            {
                //przypisanie tablicy warto�ci szumu
                heights[x, y] = Manager.Instance.CalculateHeight(x, y);
            }
        }
        return heights;
    }

    static float[,] GenerateNoise(int width, int height)
    {
        float[,] heights = new float[Manager.Instance.width, Manager.Instance.height];
        
        for (int y=0; y < height; y++) {
            for (int x=0; x < width; x++) {
                
                float atX = x;
                float atY = y;
                //float noiseHeight = 0;

                float perlinValue = Mathf.PerlinNoise(atX, atY) * 2 - 1;
                //noiseHeight += perlinValue * amplitude;
                heights[x,y] = perlinValue;
            }

        }
        return heights;
    }

    public static Color CalculateColor(float sample)
    {
        //Otrzymamy czarny, bia�y b�d� r�ne odcienie szarosci
        return new Color(sample, sample, sample);
    }
}
