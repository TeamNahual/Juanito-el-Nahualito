Shader "Custom/ButterflyShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite On ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha

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
			
			float _AnimOffset;
			
			v2f vert (appdata v)
			{
				v2f o;
				
				float animOff = (_AnimOffset)? _AnimOffset: 0.0;
				float scale = 1.00;
				float time = (_Time.y + animOff) * 3 + v.uv.y * 0.75;
				float sintime = (sin(time) * sin(time)) * 2 - 1;
				float dy = abs(0.5 - v.uv.x) * -10 * sintime;
				float dx = (abs(dy) / 5) * 3 * ((v.uv.x > 0.5)? 1: -1);
				v.vertex.x += dx * scale;
				v.vertex.y += dy * scale;
				
				o.uv = v.uv;
				o.vertex = UnityObjectToClipPos(v.vertex);
				
				return o;
			}
			
			sampler2D _MainTex;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				// just invert the colors
				//col = 1 - col;
				return col;
			}
			ENDCG
		}
	}
}
