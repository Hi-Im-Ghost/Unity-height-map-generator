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
        //By zmieni� tekstur� musimy uzyska� dostep rendera, materia�u i dopiero do tekstury
        Renderer renderer = GetComponent<Renderer>();
        //Zmiana tekstury na t� kt�ra wygenerujemy
        renderer.material.mainTexture = GenerateTexture();

    }

    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);
        
        //Generowanie mapy szum�w dla tekstury
        //Aby wygenerowa� mape szum�w Perlina trzeba przej�� przez wszystkie piksele w naszej teksturze
        //U�yjemy wi�c kilku p�tli by przej�� przez wszystkie wsp�rz�dne 

        for(int x = 0; x < width; x++) //tyle razy ile pikseli szeroko�ci
        {
            for(int y = 0; y < height; y++) //tyle razy ile pikseli wysoko�ci
            {
                //Kolor jest warto�cia generowan� przez nasz� funkcj� szumu
                Color color = CalculateColor(x,y);
                //Dla ka�dego przypadku ustawiamy piksel, r�wny kolorowi okre�lonemu przez szum
                texture.SetPixel(x, y, color);
            }
        }
        //Musimy zastosowa� dane koloru tekstury po zmianie 
        texture.Apply();

        return texture;
    }

    Color CalculateColor(int x, int y)
    {
        //Ka�emy przej�� od 0 do 1 ni� jakby mia�o przechodzi� od 0 do 256
        //Im mniejszy x tym bardziej zbli�amy si� do 0 a im wi�kszy tym bli�ej do 1
        float xCoord = (float)x/ width * intensity + offsetX; //Wsp�rz�dne dla Perlinga
        float yCoord = (float)y / height * intensity + offsetY; //Wsp�rz�dne dla Perlinga

        //Matematyczny szum Perlina
        float sample = Mathf.PerlinNoise(xCoord,yCoord); //U�ywamy miejsca dziesi�tnego
        //Otrzymamy czarny, bia�y b�d� r�ne odcienie szarosci
        return new Color(sample, sample, sample);
    }
}
