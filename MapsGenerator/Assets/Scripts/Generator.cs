using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    void Update()
    {
        //Tworzymy zmienn¹ by móc zmieniaæ dane terenu 
        Terrain terrain = GetComponent<Terrain>();
        //Ustawiamy w zmiennej dane terenu, które zostan¹ wygenerowane 
        terrain.terrainData = GenerateTerrain(terrain.terrainData);

    }

    //Metoda do generowanie danych terenu 
    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        //Ustawienie rozdzielczoœci dla mapy terenu
        terrainData.heightmapResolution = Manager.Instance.width + 1;
        //Ustawianie wymiarów terenu
        terrainData.size = new Vector3(Manager.Instance.width, Manager.Instance.depth, Manager.Instance.height);

        //Ustawiamy tablice mapy wysokoœci. Zakres 0-1
        terrainData.SetHeights(0, 0, GenerateHeights());

        return terrainData;
    }

    //Generowanie wysokoœci za pomoc¹ szumu
    float[,] GenerateHeights()
    {
        //utworzenie dwuwymiarowej tablicy float
        float [,] heights = new float[Manager.Instance.width, Manager.Instance.height];
        //przechodzimy pêtlami przez ka¿dy punkt by ustawiæ wartoœæ z szumu
        for(int x=0; x<Manager.Instance.width; x++)
        {
            for(int y=0; y<Manager.Instance.height; y++)
            {
                //przypisanie tablicy wartoœci szumu
                heights[x, y] = Manager.Instance.CalculateHeight(x,y);
            }
        }
        return heights;
    }
}
