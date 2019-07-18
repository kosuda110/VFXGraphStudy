Shader "MyShader/ScreenCover"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CoverColor ("CoverColor", Color) = (0,0,0,0)
        _Radius ("Radius", float) = 1
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        CGINCLUDE
        #include "UnityCG.cginc"

        struct appdata
        {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
        };

        struct v2f
        {
            float2 uv : TEXCOORD0;
            float4 vertex : SV_POSITION;
        };

        v2f vert (appdata v)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = v.uv;
            return o;
        }

        sampler2D _MainTex;
        float4 _CoverColor;
        float _Radius;

        fixed4 frag_rad (v2f i) : SV_Target
        {
            float dist = distance(i.vertex.xy, _ScreenParams.xy / 2);

            if (dist < _Radius)
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }

            fixed4 col = tex2D(_MainTex, i.uv) + _CoverColor;
            return col;
        }

        fixed4 frag_col_add(v2f i) : SV_Target
        {
            return tex2D(_MainTex, i.uv) + _CoverColor;
        }

        fixed4 frag_col_mul(v2f i) : SV_Target
        {
            return tex2D(_MainTex, i.uv) * _CoverColor;
        }

        fixed4 frag_col_screen(v2f i) : SV_Target
        {
            fixed4 col = tex2D(_MainTex, i.uv);
            return col + _CoverColor - (col * _CoverColor);
        }

        fixed4 frag_col_overlay(v2f i) : SV_Target
        {
            fixed4 col = tex2D(_MainTex, i.uv);

            if (col.r < 0.5 && col.g < 0.5 && col.b < 0.5)
            {
                return col * _CoverColor * 2;
            }
            else
            {
                return 1 - 2 * (1 - col) * (1 - _CoverColor);
            }
        }
        ENDCG
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag_rad
            ENDCG
        }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag_col_add
            ENDCG
        }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag_col_mul
            ENDCG
        }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag_col_screen
            ENDCG
        }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag_col_overlay
            ENDCG
        }
    }
}
