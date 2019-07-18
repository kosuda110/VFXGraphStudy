// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Unlit/Transparent/Grid"
{
    Properties
    {
        _GridThickness("Grid Thickness", Float) = 0.01
        _GridSpacing("Grid Spacing", Float) = 10.0
        _GridColor("Grid Colour", Color) = (0.5, 1.0, 1.0, 1.0)
        _BaseColor("Base Colour", Color) = (0.0, 0.0, 0.0, 0.0)
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }
        Blend SrcAlpha OneMinusSrcAlpha
        CGINCLUDE
        #include "UnityCG.cginc"

        uniform float _GridThickness;
        uniform float _GridSpacing;
        uniform float4 _GridColor;
        uniform float4 _BaseColor;

        struct appdata
        {
            float4 vertex : POSITION;
        };

        struct v2f
        {
            float4 pos : SV_POSITION;
            float4 worldPos : TEXCOORD0;
        };

        v2f vert(appdata i)
        {
            v2f o;
            o.pos = UnityObjectToClipPos(i.vertex);
            // Calculate the world position coordinates to pass to the fragment shader
            o.worldPos = mul(unity_ObjectToWorld, i.vertex);
            return o;
        }

        // FRAGMENT SHADER
        float4 frag(v2f i) : COLOR
        {
            if (frac(i.worldPos.x / _GridSpacing) < _GridThickness || frac(i.worldPos.z / _GridSpacing) < _GridThickness)
            {
                return _GridColor;
            }

            return _BaseColor;
        }

        ENDCG
        Pass
        {
            CGPROGRAM
            // Define the vertex and fragment shader functions
            #pragma vertex vert
            #pragma fragment frag
            ENDCG
        }
    }
}
