// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Unlit/Transparent/BoxBorder"
{
    Properties
    {
        _MainColor("Main Color", Color) = (1,1,1,0)
        _LineColor("Line Color", Color) = (1,1,1,1)
        _LineWidth("Line Width", float) = 0.01
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

        uniform float4 _LineColor;
        uniform float4 _MainColor;
        uniform float _LineWidth;

        // vertex input: position, uv1, uv2
        struct appdata
        {
            float4 vertex : POSITION;
            float4 texcoord : TEXCOORD0;
        };

        struct v2f
        {
            float4 pos : SV_POSITION;
            float2 uv : TEXCOORD0;
        };

        v2f vert(appdata v)
        {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);
            o.uv = v.texcoord.xy;
            return o;
        }

        fixed4 frag(v2f i) : COLOR
        {
            fixed4 res;
            float lx = step(_LineWidth, i.uv.x);
            float ly = step(_LineWidth, i.uv.y);
            float hx = step(i.uv.x, 1 - _LineWidth);
            float hy = step(i.uv.y, 1 - _LineWidth);
            res = lerp(_LineColor, _MainColor, lx*ly*hx*hy);
            //res = lerp(_LineColor, _MainColor, ly);
            return res;
        }
        ENDCG
        pass
        {
            ZWrite On
            ColorMask 0
        }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            ENDCG
        }
    }
    Fallback "Vertex Colored", 1
}
