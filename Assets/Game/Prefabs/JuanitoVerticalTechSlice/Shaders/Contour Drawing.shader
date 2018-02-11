// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "NPR Contour Drawing/Contour Drawing" {
    Properties {
		_ContourColor ("Contour Color", Color) = (0, 0, 0, 0.5)
		_ContourWidth ("Contour Width", Float) = 0.01
		_ContourTex ("Contour Texture", 2D) = "black" {}
		_Amplitude ("Amplitude", Float) = 0.01
		_Speed ("Speed", Float) = 6.0
	}
	CGINCLUDE
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile _ NPR_CONTOUR_TEX
		#include "UnityCG.cginc"
		float hash (float2 seed)
		{
			return frac(sin(dot(seed.xy, float2(12.9898, 78.233))) * 43758.5453);
		}
		struct v2f
		{
			float4 pos : SV_POSITION;
			float4 scrpos : TEXCOORD0;
		};
		fixed4 _ContourColor;
		half _ContourWidth, _Speed, _Amplitude;
		sampler2D _ContourTex;
		float4 _OutlineTex_ST;
		fixed4 frag (v2f IN) : COLOR
		{
#ifdef NPR_CONTOUR_TEX
			float2 sp = IN.scrpos.xy / IN.scrpos.w;
   			sp.xy = (sp.xy + 1) * 0.5;
			sp.xy *= _OutlineTex_ST.xy;
			return tex2D(_ContourTex, sp);
#else
			return _ContourColor;
#endif
		}
	ENDCG
	SubShader {
		Tags { "Queue" = "Overlay" "IgnoreProjector" = "True" }
		Pass {
			Cull Back
			Blend Zero One
			Lighting Off
			CGPROGRAM
				v2f vert (appdata_base v)
				{
					v2f o;
					float4 os = float4(v.normal, 0) * (_Amplitude * hash(v.texcoord.xx + floor(_Time.y * _Speed)));
					o.pos = UnityObjectToClipPos(v.vertex - os);
					o.scrpos = o.pos;
					return o;
				}
			ENDCG
		}
		Pass {
			Cull Front
			Lighting Off
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
				v2f vert (appdata_base v)
				{
					v2f o;
					float4 os = float4(v.normal, 0) * (_ContourWidth + _Amplitude * (hash(v.texcoord.xy + floor(_Time.y * _Speed)) - 0.5));
					o.pos = UnityObjectToClipPos(v.vertex + os);
					o.scrpos = o.pos;
					return o;
				}
			ENDCG
		}
		Pass {
			Cull Front
			Lighting Off
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
				v2f vert (appdata_base v)
				{
					v2f o;
					float4 os = float4(v.normal, 0) * (_ContourWidth + _Amplitude * (hash(v.texcoord.xz + floor(_Time.y * _Speed)) - 0.5));
					o.pos = UnityObjectToClipPos(v.vertex + os);
					o.scrpos = o.pos;
					return o;
				}
			ENDCG
		}
		Pass {
			Cull Front
			Lighting Off
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
				v2f vert (appdata_base v)
				{
					v2f o;
					float4 os = float4(v.normal, 0) * (_ContourWidth + _Amplitude * (hash(v.texcoord.yy + floor(_Time.y * _Speed)) - 0.5));
					o.pos = UnityObjectToClipPos(v.vertex + os);
					o.scrpos = o.pos;
					return o;
				}
			ENDCG
		}
		Pass {
			Cull Front
			Lighting Off
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
				v2f vert (appdata_base v)
				{
					v2f o;
					float4 os = float4(v.normal, 0) * (_ContourWidth + _Amplitude * (hash(v.texcoord.yz + floor(_Time.y * _Speed)) - 0.5));
					o.pos = UnityObjectToClipPos(v.vertex + os);
					o.scrpos = o.pos;
					return o;
				}
			ENDCG
		}
	}
	Fallback "Diffuse"
}
