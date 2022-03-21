using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    [SerializeField] private int width = 256;
    [SerializeField] private int height = 256;
    [SerializeField] private float intensity = 20f;
    [SerializeField] private float offsetX = 100f;
    [SerializeField] private float offsetY = 100f;

    void Start()
    {
        //Losowy szum na start
        offsetX = UnityEngine.Random.Range(0f, 99999f);
        offsetY = UnityEngine.Random.Range(0f, 99999f);
    }

    void Update()
    {
        //By zmieniæ teksturê musimy uzyskaæ dostep rendera, materia³u i dopiero do tekstury
        Renderer renderer = GetComponent<Renderer>();
        //Zmiana tekstury na t¹ która wygenerujemy
        renderer.material.mainTexture = GenerateTexture();

    }

    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);
        
        //Generowanie mapy szumów dla tekstury
        //Aby wygenerowaæ mape szumów Perlina trzeba przejœæ przez wszystkie piksele w naszej teksturze
        //U¿yjemy wiêc kilku pêtli by przejœæ przez wszystkie wspó³rzêdne 

        for(int x = 0; x < width; x++) //tyle razy ile pikseli szerokoœci
        {
            for(int y = 0; y < height; y++) //tyle razy ile pikseli wysokoœci
            {
                //Kolor jest wartoœcia generowan¹ przez nasz¹ funkcjê szumu
                Color color = CalculateColor(x,y);
                //Dla ka¿dego przypadku ustawiamy piksel, równy kolorowi okreœlonemu przez szum
                texture.SetPixel(x, y, color);
            }
        }
        //Musimy zastosowaæ dane koloru tekstury po zmianie 
        texture.Apply();

        return texture;
    }

    Color CalculateColor(int x, int y)
    {
        //Ka¿emy przejœæ od 0 do 1 ni¿ jakby mia³o przechodziæ od 0 do 256
        //Im mniejszy x tym bardziej zbli¿amy siê do 0 a im wiêkszy tym bli¿ej do 1
        float xCoord = (float)x/ width * intensity + offsetX; //Wspó³rzêdne dla Perlinga
        float yCoord = (float)y / height * intensity + offsetY; //Wspó³rzêdne dla Perlinga

        //Matematyczny szum Perlina
        float sample = Mathf.PerlinNoise(xCoord,yCoord); //U¿ywamy miejsca dziesiêtnego
        //Otrzymamy czarny, bia³y b¹dŸ ró¿ne odcienie szarosci
        return new Color(sample, sample, sample);
    }
}
