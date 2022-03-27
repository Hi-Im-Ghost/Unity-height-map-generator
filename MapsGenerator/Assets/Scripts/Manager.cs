using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager Instance { get; private set; }
    public bool stop = false;
    public int width = 256;
    public int height = 256;

    public float scrollSpeed = 5f;
    public float intensity = 20f;
    public float offsetX;
    public float offsetY;
    //Wysoko�� na osi Y terenu
    public int depth = 20;
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //Losowy szum na start
        offsetX = Random.Range(0f, 9999f);
        offsetY = Random.Range(0f, 9999f);
    }

    void Update()
    {
        if (!Manager.Instance.ShouldStop())
            //animowanie przesuwania co klatke
            offsetX += Time.deltaTime * scrollSpeed;
        //offsetY += Time.deltaTime * 2f;
    }

    //Funkcja do generowanie wysoko�ci szumu Perlina
    public float CalculateHeight(int x, int y)
    {
        //Ka�emy przej�� od 0 do 1 ni� jakby mia�o przechodzi� od 0 do 256
        //Im mniejszy x tym bardziej zbli�amy si� do 0 a im wi�kszy tym bli�ej do 1
        float xCoord = (float)x / width * intensity + offsetX; //Wsp�rz�dne dla Perlinga
        float yCoord = (float)y / height * intensity + offsetY; //Wsp�rz�dne dla Perlinga

        //Matematyczny szum Perlina
        return Mathf.PerlinNoise(xCoord, yCoord); //U�ywamy miejsca dziesi�tnego


    }

    //Funkcja do generowanie koloru szumu Perlina
    public Color CalculateColor(float sample)
    {
        //Otrzymamy czarny, bia�y b�d� r�ne odcienie szarosci
        return new Color(sample, sample, sample);
    }

    public bool ShouldStop()
    {
        return stop;
    }
}
