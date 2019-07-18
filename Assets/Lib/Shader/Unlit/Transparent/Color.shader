Shader "Custom/Unlit/Transparent/Color"
{
    Properties
    {
        _Color ("Color", color) = (0, 0, 0, 0)
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
            float4 vertex : POSITION;
        };

        struct v2f
        {
            float4 vertex : SV_POSITION;
        };

        float4 _Color;

        v2f vert (appdata v)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            return o;
        }

        fixed4 frag_color (v2f i) : SV_Target
        {
            return _Color;
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
            #pragma fragment frag_color
            ENDCG
        }
    }
}
