Shader "Custom/Leaves" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Main Texture", 2D) = "white" {  }
        _Cutoff ("Alpha cutoff", Range(0.25,0.9)) = 0.5
        _BaseLight ("Base Light", Range(0, 1)) = 0.35
        _AO ("Amb. Occlusion", Range(0, 10)) = 2.4
        _Occlusion ("Dir Occlusion", Range(0, 20)) = 7.5

        // These are here only to provide default values
        [HideInInspector] _TreeInstanceColor ("TreeInstanceColor", Vector) = (1,1,1,1)
        [HideInInspector] _TreeInstanceScale ("TreeInstanceScale", Vector) = (1,1,1,1)
        [HideInInspector] _SquashAmount ("Squash", Float) = 1
    }

    SubShader {
        Tags {
            "Queue" = "AlphaTest"
            "IgnoreProjector"="True"
            "RenderType" = "TreeTransparentCutout"
            "DisableBatching"="True"
        }
        Cull Off
        ColorMask RGB

        Pass {
            Lighting On

            CGPROGRAM
			#pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "UnityBuiltin2xTreeLibrary.cginc"

			struct appdata
			{
				float4 vertex : POSITION;       // position
				float4 tangent : TANGENT;       // directional AO
				float3 normal : NORMAL;         // normal
				fixed4 color : COLOR;           // .w = bend factor
				float4 texcoord : TEXCOORD0;    // UV
				float4 posWorld : POSITION;
			};
			
			struct vert2f {
				float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;
				half4 color : TEXCOORD1;
				float4 posWorld : TEXCOORD2;
				UNITY_FOG_COORDS(3)
				UNITY_VERTEX_OUTPUT_STEREO
			};
			
			vert2f vert (appdata v)
			{
				vert2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				TerrainAnimateTree(v.vertex, v.color.w);

				float3 viewpos = UnityObjectToViewPos(v.vertex);
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;

				float4 lightDir = 0;
				float4 lightColor = 0;
				lightDir.w = _AO;

				float4 light = UNITY_LIGHTMODEL_AMBIENT;

				for (int i = 0; i < 4; i++) {
					float atten = 1.0;
					#ifdef USE_CUSTOM_LIGHT_DIR
						lightDir.xyz = _TerrainTreeLightDirections[i];
						lightColor = _TerrainTreeLightColors[i];
					#else
							float3 toLight = unity_LightPosition[i].xyz - viewpos.xyz * unity_LightPosition[i].w;
							toLight.z *= -1.0;
							lightDir.xyz = mul( (float3x3)unity_CameraToWorld, normalize(toLight) );
							float lengthSq = dot(toLight, toLight);
							atten = 1.0 / (1.0 + lengthSq * unity_LightAtten[i].z);

							lightColor.rgb = unity_LightColor[i].rgb;
					#endif

					lightDir.xyz *= _Occlusion;
					float occ =  dot (v.tangent, lightDir);
					occ = max(0, occ);
					occ += _BaseLight;
					light += lightColor * (occ * atten);
				}

				o.color = light * _Color * _TreeInstanceColor;
				o.color.a = 0.5 * _HalfOverCutoff;

				UNITY_TRANSFER_FOG(o,o.pos);
				o.posWorld = mul(unity_ObjectToWorld, v.vertex);
				
				return o;
			}

            sampler2D _MainTex;
            fixed _Cutoff;

			uniform sampler2D _MK_FOG_STYLISTIC;
			uniform sampler2D _MK_FOG_DESATURATE;
			
			uniform int       _MK_FOG_SPIRIT_MODE_ENABLED;
			uniform sampler2D _MK_FOG_SPIRIT_MODE_GRADIENT;
			uniform float     _MK_FOG_SPIRIT_MODE_RADIUS;
			uniform float3    _MK_FOG_SPIRIT_MODE_ORIGIN;

            fixed4 frag(vert2f input) : SV_Target
            {
                fixed4 c = tex2D( _MainTex, input.uv.xy);
                c.rgb *= input.color.rgb;

                clip (c.a - _Cutoff);
                UNITY_APPLY_FOG(input.fogCoord, c);
				
				// Fog shenanigans
				float alpha = c.a;
				if (_MK_FOG_SPIRIT_MODE_ENABLED) {
					// Spirit Mode Shader
					float spiritScale = min(0.99, distance(_MK_FOG_SPIRIT_MODE_ORIGIN, input.posWorld) / _MK_FOG_SPIRIT_MODE_RADIUS);
					float gs = (c.r + c.g + c.b) / 3;
					float gz = (1 - spiritScale * spiritScale) * -2;// + ( sin(_Time.y) * sin(_Time.y) * 10);
					float realWorld = 0.1 * sin(_Time.y / 2) * sin(_Time.y / 2) + 0.5;
					c.r = c.r * realWorld + ((c.r * gz) + (gs * (1 - gz))) * (1 - realWorld);
					c.g = c.g * realWorld + ((c.g * gz) + (gs * (1 - gz))) * (1 - realWorld);
					c.b = c.b * realWorld + ((c.b * gz) + (gs * (1 - gz))) * (1 - realWorld);
					c = lerp(c, tex2D(_MK_FOG_SPIRIT_MODE_GRADIENT, float2(spiritScale, 0)), spiritScale);
				} else {
					// Stylistic and Desaturated fog effects
					float fogScale = min(0.99, distance(_WorldSpaceCameraPos, input.posWorld) / 1000);
					
					// Stylistic
					fixed4 sCol = tex2D(_MK_FOG_STYLISTIC, float2(fogScale, 0));
					c = lerp(c, sCol, sCol.a);
					c.a = alpha;
					
					// Desaturation
					float gs = (c.r + c.g + c.b) / 3;
					float gz = tex2D(_MK_FOG_DESATURATE, float2(fogScale, 0)).a;
					c = lerp(c, float4(gs, gs, gs, gz), gz);	
				}

				c.a = alpha;
				
                return c;
            }
            ENDCG
        }

        Pass {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster
            #include "UnityCG.cginc"
            #include "TerrainEngine.cginc"

            struct v2f {
                V2F_SHADOW_CASTER;
                float2 uv : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                fixed4 color : COLOR;
                float4 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            v2f vert( appdata v )
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                TerrainAnimateTree(v.vertex, v.color.w);
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                o.uv = v.texcoord;
                return o;
            }

            sampler2D _MainTex;
            fixed _Cutoff;

            float4 frag( v2f i ) : SV_Target
            {
                fixed4 texcol = tex2D( _MainTex, i.uv );
                clip( texcol.a - _Cutoff );
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }

    // This subshader is never actually used, but is only kept so
    // that the tree mesh still assumes that normals are needed
    // at build time (due to Lighting On in the pass). The subshader
    // above does not actually use normals, so they are stripped out.
    // We want to keep normals for backwards compatibility with Unity 4.2
    // and earlier.
    SubShader {
        Tags {
            "Queue" = "AlphaTest"
            "IgnoreProjector"="True"
            "RenderType" = "TransparentCutout"
        }
        Cull Off
        ColorMask RGB
        Pass {
            Tags { "LightMode" = "Vertex" }
            AlphaTest GEqual [_Cutoff]
            Lighting On
            Material {
                Diffuse [_Color]
                Ambient [_Color]
            }
            SetTexture [_MainTex] { combine primary * texture DOUBLE, texture }
        }
    }

    Dependency "BillboardShader" = "Hidden/Nature/Tree Soft Occlusion Leaves Rendertex"
    Fallback Off
}
