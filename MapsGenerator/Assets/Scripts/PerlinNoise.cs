using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    private new Renderer renderer;

    private void Awake()
    {
        //By zmieni� tekstur� musimy uzyska� dostep rendera, materia�u i dopiero do tekstury
        renderer = GetComponent<Renderer>();
    }
    void Update()
    {
        if (!Manager.Instance.ShouldStop())
            //Zmiana tekstury na t� kt�ra wygenerujemy
            renderer.material.mainTexture = GenerateTexture();
    }

    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(Manager.Instance.width, Manager.Instance.height);

        //Generowanie mapy szum�w dla tekstury
        //Aby wygenerowa� mape szum�w Perlina trzeba przej�� przez wszystkie piksele w naszej teksturze
        //U�yjemy wi�c kilku p�tli by przej�� przez wszystkie wsp�rz�dne 

        for (int x = 0; x < Manager.Instance.width; x++) //tyle razy ile pikseli szeroko�ci
        {
            for (int y = 0; y < Manager.Instance.height; y++) //tyle razy ile pikseli wysoko�ci
            {
                float sample = Manager.Instance.CalculateHeight(x, y);
                //Kolor jest warto�cia generowan� przez nasz� funkcj� szumu
                Color color = Manager.Instance.CalculateColor(sample);
                //Dla ka�dego przypadku ustawiamy piksel, r�wny kolorowi okre�lonemu przez szum
                texture.SetPixel(x, y, color);
            }
        }
        //Musimy zastosowa� dane koloru tekstury po zmianie 
        texture.Apply();

        return texture;
    }


}
