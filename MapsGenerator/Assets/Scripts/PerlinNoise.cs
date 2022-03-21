using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    void Update()
    {
        //By zmieniæ teksturê musimy uzyskaæ dostep rendera, materia³u i dopiero do tekstury
        Renderer renderer = GetComponent<Renderer>();
        //Zmiana tekstury na t¹ która wygenerujemy
        renderer.material.mainTexture = GenerateTexture();

    }

    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(Manager.Instance.width, Manager.Instance.height);
        
        //Generowanie mapy szumów dla tekstury
        //Aby wygenerowaæ mape szumów Perlina trzeba przejœæ przez wszystkie piksele w naszej teksturze
        //U¿yjemy wiêc kilku pêtli by przejœæ przez wszystkie wspó³rzêdne 

        for(int x = 0; x < Manager.Instance.width; x++) //tyle razy ile pikseli szerokoœci
        {
            for(int y = 0; y < Manager.Instance.height; y++) //tyle razy ile pikseli wysokoœci
            {
                float sample = Manager.Instance.CalculateHeight(x, y);
                //Kolor jest wartoœcia generowan¹ przez nasz¹ funkcjê szumu
                Color color = Manager.Instance.CalculateColor(sample);
                //Dla ka¿dego przypadku ustawiamy piksel, równy kolorowi okreœlonemu przez szum
                texture.SetPixel(x, y, color);
            }
        }
        //Musimy zastosowaæ dane koloru tekstury po zmianie 
        texture.Apply();

        return texture;
    }


}
