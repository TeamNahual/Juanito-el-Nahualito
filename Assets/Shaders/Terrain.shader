// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Custom/Terrain" {
    Properties {
        [HideInInspector] _Control ("Control (RGBA)", 2D) = "red" {}
        [HideInInspector] _Splat3 ("Layer 3 (A)", 2D) = "white" {}
        [HideInInspector] _Splat2 ("Layer 2 (B)", 2D) = "white" {}
        [HideInInspector] _Splat1 ("Layer 1 (G)", 2D) = "white" {}
        [HideInInspector] _Splat0 ("Layer 0 (R)", 2D) = "white" {}
        [HideInInspector] _Normal3 ("Normal 3 (A)", 2D) = "bump" {}
        [HideInInspector] _Normal2 ("Normal 2 (B)", 2D) = "bump" {}
        [HideInInspector] _Normal1 ("Normal 1 (G)", 2D) = "bump" {}
        [HideInInspector] _Normal0 ("Normal 0 (R)", 2D) = "bump" {}
        // used in fallback on old cards & base map
        [HideInInspector] _MainTex ("BaseMap (RGB)", 2D) = "white" {}
        [HideInInspector] _Color ("Main Color", Color) = (1,1,1,1)
    }

    CGINCLUDE
        #pragma surface surf Lambert vertex:SplatmapVert finalcolor:SplatmapFinalColor finalprepass:SplatmapFinalPrepass finalgbuffer:SplatmapFinalGBuffer noinstancing
        #pragma multi_compile_fog
        #include "CustomTerrainSplatmapCommon.cginc"
		
		uniform sampler2D _MK_FOG_STYLISTIC;
		uniform sampler2D _MK_FOG_DESATURATE;

		uniform int       _MK_FOG_SPIRIT_MODE_ENABLED;
		uniform sampler2D _MK_FOG_SPIRIT_MODE_GRADIENT;
		uniform float     _MK_FOG_SPIRIT_MODE_RADIUS;
		uniform float3    _MK_FOG_SPIRIT_MODE_ORIGIN;
		
        void surf(Input IN, inout SurfaceOutput o)
        {
            half4 splat_control;
            half weight;
            fixed4 c;
            SplatmapMix(IN, splat_control, weight, c, o.Normal);
			
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
            o.Alpha = weight;
        }
    ENDCG

    Category {
        Tags {
            "Queue" = "Geometry-99"
            "RenderType" = "Opaque"
        }
        // TODO: Seems like "#pragma target 3.0 _TERRAIN_NORMAL_MAP" can't fallback correctly on less capable devices?
        // Use two sub-shaders to simulate different features for different targets and still fallback correctly.
        SubShader { // for sm3.0+ targets
            CGPROGRAM
                #pragma target 3.0
                #pragma multi_compile __ _TERRAIN_NORMAL_MAP
            ENDCG
        }
        SubShader { // for sm2.0 targets
            CGPROGRAM
            ENDCG
        }
    }

    Dependency "AddPassShader" = "Hidden/TerrainEngine/Splatmap/Diffuse-AddPass"
    Dependency "BaseMapShader" = "Diffuse"
    Dependency "Details0"      = "Hidden/TerrainEngine/Details/Vertexlit"
    Dependency "Details1"      = "Hidden/TerrainEngine/Details/WavingDoublePass"
    Dependency "Details2"      = "Hidden/TerrainEngine/Details/BillboardWavingDoublePass"
    Dependency "Tree0"         = "Hidden/TerrainEngine/BillboardTree"

    Fallback "Diffuse"
}
