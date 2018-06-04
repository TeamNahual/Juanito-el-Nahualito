Shader "Custom/SpiritBorder"
{
	Properties
	{
		
	}
	SubShader
	{
		// No culling or depth
		Cull Front ZWrite On ZTest LEqual

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
				
				o.uv = v.uv;
				o.vertex = UnityObjectToClipPos(v.vertex);
				
				return o;
			}
			
			uniform int _MK_FOG_SPIRIT_MODE_ENABLED;
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = fixed4(0,0,0,0);
				
				if (_MK_FOG_SPIRIT_MODE_ENABLED) {
					col.a = 1;
				} else {
					col.a = 0;
				}
				return col;
			}
			ENDCG
		}
	}
}
