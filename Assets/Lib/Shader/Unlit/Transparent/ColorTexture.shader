Shader "Custom/Unlit/Transparent/ColorTexture"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        [HDR]_Color ("Color", color) = (0, 0, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite On

        CGINCLUDE
        #include "UnityCG.cginc"

        struct appdata
        {
            float2 uv : TEXCOORD0;
            float4 vertex : POSITION;
        };

        struct v2f
        {
            float2 uv : TEXCOORD0;
            float4 vertex : SV_POSITION;
        };

        float4 _Color;
        sampler2D _MainTex;
        float4 _MainTex_ST;

        v2f vert (appdata v)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = TRANSFORM_TEX(v.uv, _MainTex);
            return o;
        }

        fixed4 frag (v2f i) : SV_Target
        {
            half4 base = tex2D(_MainTex, i.uv);
            half4 col = base * _Color * base.a;
            return col;
        }
        ENDCG
        Pass
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
}
