Shader "Custom/SpiritSurface" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos : TEXCOORD0;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END
		
		uniform sampler2D _MK_FOG_STYLISTIC;
		uniform sampler2D _MK_FOG_DESATURATE;

		uniform int       _MK_FOG_SPIRIT_MODE_ENABLED;
		uniform sampler2D _MK_FOG_SPIRIT_MODE_GRADIENT;
		uniform float     _MK_FOG_SPIRIT_MODE_RADIUS;
		uniform float3    _MK_FOG_SPIRIT_MODE_ORIGIN;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			
			// Fog shenanigans
			float alpha = c.a;
			if (_MK_FOG_SPIRIT_MODE_ENABLED) {
				// Spirit Mode Shader
				float spiritScale = min(0.99, distance(_MK_FOG_SPIRIT_MODE_ORIGIN, IN.worldPos) / _MK_FOG_SPIRIT_MODE_RADIUS);
				float gs = (c.r + c.g + c.b) / 3;
				float gz = (1 - spiritScale * spiritScale) * -2;// + ( sin(_Time.y) * sin(_Time.y) * 10);
				float realWorld = 0.1 * sin(_Time.y / 2) * sin(_Time.y / 2) + 0.5;
				c.r = c.r * realWorld + ((c.r * gz) + (gs * (1 - gz))) * (1 - realWorld);
				c.g = c.g * realWorld + ((c.g * gz) + (gs * (1 - gz))) * (1 - realWorld);
				c.b = c.b * realWorld + ((c.b * gz) + (gs * (1 - gz))) * (1 - realWorld);
				c = lerp(c, tex2D(_MK_FOG_SPIRIT_MODE_GRADIENT, float2(spiritScale, 0)), spiritScale);
				c.a = alpha;
			} else {
				// Stylistic and Desaturated fog effects
				float fogScale = min(0.99, distance(_WorldSpaceCameraPos, IN.worldPos) / 1000);
				
				// Stylistic
				fixed4 sCol = tex2D(_MK_FOG_STYLISTIC, float2(fogScale, 0));
				c = lerp(c, sCol, sCol.a);
				c.a = alpha;
				
				// Desaturation
				float gs = (c.r + c.g + c.b) / 3;
				float gz = tex2D(_MK_FOG_DESATURATE, float2(fogScale, 0)).a;
				c = lerp(c, float4(gs, gs, gs, gz), gz);
				c.a = alpha;
			}
			
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
