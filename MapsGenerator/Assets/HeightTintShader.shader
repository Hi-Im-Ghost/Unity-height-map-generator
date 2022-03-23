Shader "Custom/HeightTintShader"
{
    // Właściwości materiału wybierane w inspektorze
   Properties 
   {
     _MainTex ("Base (RGB)", 2D) = "white" {}
     _HeightMin ("Height Min", Float) = -1
     _HeightMax ("Height Max", Float) = 1
     _Color0 ("Tint Color 0", Color) = (0,0,0,1)
     _Color1 ("Tint Color 1", Color) = (1,1,1,1)
     _Color2 ("Tint Color 2", Color) = (1,1,1,1)
   }
  
   SubShader
   {
       // Opaque zaznacza, że materiał używający tego shadera będzie nieprzezroczysty
     Tags { "RenderType"="Opaque" }
  
     CGPROGRAM
     #pragma surface surf Lambert
  
    // Deklaracja zmiennych na podstawie powyższych właściwości
     sampler2D _MainTex;
     fixed4 _Color0;
     fixed4 _Color1;
     fixed4 _Color2;
     float _HeightMin;
     float _HeightMax;
  
     struct Input
     {
       float2 uv_MainTex;
       float3 worldPos;
     };
  
     void surf (Input IN, inout SurfaceOutput o) 
     {
         // Pobranie tekstury obiektu używającego tego shadera
       half4 c = tex2D (_MainTex, IN.uv_MainTex);
       // Obliczanie wysokości działania koloru na podstawie pozycji obiektu
       // i wysokości minimalnej i maksymalej ustawionej w inspektorze
       float h = (_HeightMax-IN.worldPos.y) / (_HeightMax-_HeightMin);
       // Gradient z koloru0 do koloru1 od wysokości maksymalnej w dół.
       // h/0.3f oznacza zmianę koloru w 1/3 odległości od szczytu
       fixed4 tintColor = lerp(_Color0.rgba,_Color1.rgba, h/.3f);
       // Gradient z wcześniej powstałego koloru do koloru2 który ciągnie się do wysokości minimalnej
       tintColor = lerp(tintColor.rgba, _Color2.rgba, h/.5f);
       // Ustawienie otrzymanego gradientu jako kolor nakładany na teksturę obiektu
       o.Albedo = c.rgb * tintColor.rgb;
       o.Alpha = c.a * tintColor.a;
     }
     ENDCG
   } 
   Fallback "Diffuse"
 }
