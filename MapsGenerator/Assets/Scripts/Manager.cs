using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager Instance { get; private set; }
    public int width = 256;
    public int height = 256;
    public float intensity = 20f;
    public float offsetX = 100f;
    public float offsetY = 100f;
    //Wysokoœæ na osi Y terenu
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
        //animowanie przesuwania co klatke
        offsetX += Time.deltaTime * 5f;
        //offsetY += Time.deltaTime * 2f;
    }

    //Funkcja do generowanie wysokoœci szumu Perlina
    public float CalculateHeight(int x, int y)
    {
        //Ka¿emy przejœæ od 0 do 1 ni¿ jakby mia³o przechodziæ od 0 do 256
        //Im mniejszy x tym bardziej zbli¿amy siê do 0 a im wiêkszy tym bli¿ej do 1
        float xCoord = (float)x / width * intensity + offsetX; //Wspó³rzêdne dla Perlinga
        float yCoord = (float)y / height * intensity + offsetY; //Wspó³rzêdne dla Perlinga

        //Matematyczny szum Perlina
        return Mathf.PerlinNoise(xCoord, yCoord); //U¿ywamy miejsca dziesiêtnego


    }

    //Funkcja do generowanie koloru szumu Perlina
    public Color CalculateColor(float sample)
    {
        //Otrzymamy czarny, bia³y b¹dŸ ró¿ne odcienie szarosci
        return new Color(sample, sample, sample);
    }
}
