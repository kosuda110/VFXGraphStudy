// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Unlit/Transparent/StencilOutLine"
{
	Properties
	{
		_MainColor("MainColor", Color) = (1, 1, 1, 1)
		_OutLine("OutLine", float) = 0.1
		_OutLineColor("OutLineColor", Color) = (1, 1, 1, 1)
	}
	SubShader
	{
		Tags
		{
			"RenderType" = "Transparent"
			"Queue" = "Transparent"
		}
		LOD 100
		CGINCLUDE
		#include "UnityCG.cginc"

		struct appdata
		{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
			float3 normal : NORMAL;
		};

		struct v2f
		{
			float2 uv : TEXCOORD0;
			UNITY_FOG_COORDS(1)
			float4 vertex : SV_POSITION;
		};

		struct v2f_outline
		{
			float4 vertex : SV_POSITION;
		};

		sampler2D _MainTex;
		float4 _MainTex_ST;
		float4 _MainColor;
		float _OutLine;
		float4 _OutLineColor;

		v2f vert(appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			UNITY_TRANSFER_FOG(o,o.vertex);
			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			// sample the texture
			fixed4 col = _MainColor;
			// apply fog
			UNITY_APPLY_FOG(i.fogCoord, col);
			return col;
		}

		v2f_outline vert_outline(appdata v)
		{
			v2f_outline o;
			float4 vert = v.vertex;
			//vert.xyz += v.normal * _OutLine;
			// boxの場合以下のパターン使用する必要あり
			vert.xyz *= (1 + _OutLine);
			o.vertex = UnityObjectToClipPos(vert);
			return o;
		}

		fixed4 frag_outline(v2f_outline i) : SV_Target
		{
			return _OutLineColor;
		}

		ENDCG

		Pass
		{
			Stencil
			{
				Ref 1
				Comp Always
				Pass Replace
			}
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			ENDCG
		}
		Pass
		{
			Stencil
			{
				Ref 1
				Comp NotEqual
			}
			Cull Off
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert_outline
			#pragma fragment frag_outline
			ENDCG
		}
	}
}
