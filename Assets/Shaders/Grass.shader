Shader "Hidden/TerrainEngine/Details/BillboardWavingDoublePass" {
    Properties {
        _WavingTint ("Fade Color", Color) = (.7,.6,.5, 0)
        _MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
        _WaveAndDistance ("Wave and distance", Vector) = (12, 3.6, 1, 1)
        _Cutoff ("Cutoff", float) = 0.5
    }

CGINCLUDE
#include "UnityCG.cginc"
#include "TerrainEngine.cginc"

struct v2f {
    float4 pos : SV_POSITION;
    fixed4 color : COLOR;
    float4 uv : TEXCOORD0;
	float4 posWorld : TEXCOORD1;
    UNITY_VERTEX_OUTPUT_STEREO
};
v2f BillboardVert (appdata_full v) {
    v2f o;
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
    WavingGrassBillboardVert (v);
    o.color = v.color;

    o.color.rgb *= ShadeVertexLights (v.vertex, v.normal);

    o.pos = UnityObjectToClipPos(v.vertex);
    o.uv = v.texcoord;
	o.posWorld = mul(unity_ObjectToWorld, v.vertex);
    return o;
}
ENDCG

    SubShader {
        Tags {
            "Queue" = "Geometry+200"
            "IgnoreProjector"="True"
            "RenderType"="GrassBillboard"
            "DisableBatching"="True"
        }
        Cull Off
        LOD 200
        ColorMask RGB

CGPROGRAM
#pragma surface surf Lambert vertex:WavingGrassBillboardVert addshadow exclude_path:deferred

sampler2D _MainTex;
fixed _Cutoff;

struct Input {
    float2 uv_MainTex;
    fixed4 color : COLOR;
	float3 worldPos : TEXCOORD0;
};

uniform sampler2D _MK_FOG_STYLISTIC;
uniform sampler2D _MK_FOG_DESATURATE;

uniform int       _MK_FOG_SPIRIT_MODE_ENABLED;
uniform sampler2D _MK_FOG_SPIRIT_MODE_GRADIENT;
uniform float     _MK_FOG_SPIRIT_MODE_RADIUS;
uniform float3    _MK_FOG_SPIRIT_MODE_ORIGIN;

void surf (Input IN, inout SurfaceOutput o) {
    fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
	
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
    o.Alpha = c.a;
    clip (o.Alpha - _Cutoff);
    o.Alpha *= IN.color.a;
}

ENDCG
    }

    Fallback Off
}
