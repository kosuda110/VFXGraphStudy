Shader "Hidden/DistortionImageEffect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
        _Frequency ("frequency", Range(0, 200)) = 0
        _Amplitude ("Amplitude", Range(0, 0.03)) = 0
        _Phase ("Phase", float) = 0
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
            float4 screenPos : TEXCOORD1;
        };

        v2f vert (appdata v)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = v.uv;
            o.screenPos = ComputeScreenPos(o.vertex);
            return o;
        }

        sampler2D _MainTex;
        float _Frequency;
        float _Amplitude;
        float _Phase;
        sampler2D _CameraDepthTexture;

        fixed4 frag_distortion (v2f i) : SV_Target
        {
            //float depthValue = Linear01Depth (tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)).r);
            //fixed depthValue = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, i.uv));
            float distortion = sin(i.uv.y * _Frequency + _Phase) * _Amplitude;
            fixed4 col = tex2D(_MainTex, float2(i.uv.x + distortion, i.uv.y));
            return col;
            //return 1 - depthValue;
        }

        ENDCG

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag_distortion
			ENDCG
		}
	}
}
