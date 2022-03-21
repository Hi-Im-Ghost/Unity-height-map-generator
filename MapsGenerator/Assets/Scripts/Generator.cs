using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    void Update()
    {
        //Tworzymy zmienn� by m�c zmienia� dane terenu 
        Terrain terrain = GetComponent<Terrain>();
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
    float[,] GenerateHeights()
    {
        //utworzenie dwuwymiarowej tablicy float
        float [,] heights = new float[Manager.Instance.width, Manager.Instance.height];
        //przechodzimy p�tlami przez ka�dy punkt by ustawi� warto�� z szumu
        for(int x=0; x<Manager.Instance.width; x++)
        {
            for(int y=0; y<Manager.Instance.height; y++)
            {
                //przypisanie tablicy warto�ci szumu
                heights[x, y] = Manager.Instance.CalculateHeight(x,y);
            }
        }
        return heights;
    }
}
