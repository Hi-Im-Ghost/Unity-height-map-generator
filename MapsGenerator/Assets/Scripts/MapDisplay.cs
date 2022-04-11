using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Odpowiada za wyświetlanie wygenerowanej mapy za pomocą obiektu Plane
* Konweruje mapę szumu na teksturę, którą aplikuje do obiektu Plane
*/
public class MapDisplay : MonoBehaviour
{
    // Odwołanie do renderera obiektu plane. Potrzebny do aplikowania tekstury
    public Renderer textureRenderer;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    // Zamień podaną tablicę wartości na teksturę i zaaplikuj
    public void DrawTexture(Texture2D texture)
    {
        // textureRenderer.material działa tylko po naciśnięciu start
        // Efekty sharedMaterial widać już w edytorze
        textureRenderer.sharedMaterial.mainTexture = texture;

        // Ustaw wielkość obiektu Plane do wielkości tekstury
        textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public void DrawMesh(MeshData meshData, Texture2D texture)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();
        meshRenderer.sharedMaterial.mainTexture = texture;
    }
}
