Shader "ImageEffects/Stuff"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Noise("Noise tex", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
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
			sampler2D _Noise;
			float _Strength;
			float _UTime;

			fixed4 frag (v2f i) : SV_Target
			{
				float noise = tex2D(_Noise, float2(_UTime * 2 * 1.5718932749812703, i.uv.y));
				fixed4 col = tex2D(_MainTex,i.uv + float2((noise * 2 - 1) * 0.015 * _Strength, 0));
				return col;
			}
			ENDCG
		}
	}
}
