// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

//vertex and fragment shader
#ifndef MK_TOON_FORWARD
	#define MK_TOON_FORWARD
	
	/////////////////////////////////////////////////////////////////////////////////////////////
	// VERTEX SHADER
	/////////////////////////////////////////////////////////////////////////////////////////////
	VertexOutputForward vertfwd (VertexInputForward v)
	{
		UNITY_SETUP_INSTANCE_ID(v);
		VertexOutputForward o;
		UNITY_INITIALIZE_OUTPUT(VertexOutputForward, o);
		UNITY_TRANSFER_INSTANCE_ID(v,o);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
		
		float4 vert = v.vertex;
		#ifdef GRASS_SHADER
			float scaleFactor = (v.vertex * v.vertex * v.vertex) / 10;
			float offset = scaleFactor * sin(_Time.y * 3);
			vert.x += offset;
			vert.y -= offset / 2;
		#endif
		
		//vertex positions
		o.posWorld = mul(unity_ObjectToWorld, vert);
		o.pos =  UnityObjectToClipPos(vert);
		
		//texcoords
		o.uv_Main = TRANSFORM_TEX(v.texcoord0, _MainTex);
		
		//normal tangent binormal
		o.tangentWorld = normalize(mul(unity_ObjectToWorld, half4(v.tangent.xyz, 0.0)).xyz);
		o.normalWorld = normalize(mul(half4(v.normal, 0.0), unity_WorldToObject).xyz);
		o.binormalWorld = normalize(cross(o.normalWorld, o.tangentWorld) * v.tangent.w);

		#ifdef MK_TOON_FWD_BASE_PASS
			//lightmaps and ambient
			#ifdef DYNAMICLIGHTMAP_ON
				o.uv_Lm.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
				o.uv_Lm.xy = 1;
			#endif
			#ifdef LIGHTMAP_ON
				o.uv_Lm.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				o.uv_Lm.zw = 1;
			#endif

			#ifdef MK_TOON_FWD_BASE_PASS
				#if UNITY_SHOULD_SAMPLE_SH
				//unity ambient light
					o.aLight = ShadeSH9 (half4(o.normalWorld,1.0));
				#else
					o.aLight = 0.0;
				#endif
				#ifdef VERTEXLIGHT_ON
					//vertexlight
					o.aLight += Shade4PointLights (
					unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
					unity_LightColor[0].rgb, unity_LightColor[1].rgb, unity_LightColor[2].rgb, unity_LightColor[3].rgb,
					unity_4LightAtten0, o.posWorld, o.normalWorld);
				#endif
			#endif
		#endif

		//vertex shadow
		UNITY_TRANSFER_SHADOW(o,v.texcoord0.xy); 

		//vertex fog
		UNITY_TRANSFER_FOG(o,o.pos);
		return o;
	}

	uniform sampler2D _MK_FOG_STYLISTIC;
	uniform sampler2D _MK_FOG_DESATURATE;
	
	uniform int       _MK_FOG_SPIRIT_MODE_ENABLED;
	uniform sampler2D _MK_FOG_SPIRIT_MODE_GRADIENT;
	uniform float     _MK_FOG_SPIRIT_MODE_RADIUS;
	uniform float3    _MK_FOG_SPIRIT_MODE_ORIGIN;
	
	/////////////////////////////////////////////////////////////////////////////////////////////
	// FRAGMENT SHADER
	/////////////////////////////////////////////////////////////////////////////////////////////
	fixed4 fragfwd (VertexOutputForward o) : SV_Target
	{
		UNITY_SETUP_INSTANCE_ID(o);
		//init surface struct for rendering
		MKToonSurface mkts = InitSurface(o);

		//apply lights, ambient and lightmap
		MKToonLightLMCombined(mkts, o);

		//Emission
		#if _MKTOON_EMISSION
			//apply rim lighting
			mkts.Color_Emission += RimDefault(_RimSize, mkts.Pcp.VdotN, _RimColor.rgb, _RimIntensity, _RimSmoothness);
			mkts.Color_Out.rgb += mkts.Color_Emission;
		#endif

		mkts.Color_Out.rgb = BControl(mkts.Color_Out.rgb, _Brightness);

		//if enabled add some fog - forward rendering only
		UNITY_APPLY_FOG(o.fogCoord, mkts.Color_Out);
		
		// Fog shenanigans
		float alpha = mkts.Color_Out.a;
		if (_MK_FOG_SPIRIT_MODE_ENABLED) {
			// Spirit Mode Shader
			float spiritScale = min(0.99, distance(_MK_FOG_SPIRIT_MODE_ORIGIN, o.posWorld) / _MK_FOG_SPIRIT_MODE_RADIUS);
			float gs = (mkts.Color_Out.r + mkts.Color_Out.g + mkts.Color_Out.b) / 3;
			float gz = (1 - spiritScale * spiritScale) * -5;
			mkts.Color_Out.r = (mkts.Color_Out.r * gz) + (gs * (1 - gz));
			mkts.Color_Out.g = (mkts.Color_Out.g * gz) + (gs * (1 - gz));
			mkts.Color_Out.b = (mkts.Color_Out.b * gz) + (gs * (1 - gz));
			mkts.Color_Out = lerp(mkts.Color_Out, tex2D(_MK_FOG_SPIRIT_MODE_GRADIENT, float2(spiritScale, 0)), spiritScale);
		} else {
			// Stylistic and Desaturated fog effects
			float fogScale = min(0.99, distance(_WorldSpaceCameraPos, o.posWorld) / 1000);
			
			// Stylistic
			fixed4 sCol = tex2D(_MK_FOG_STYLISTIC, float2(fogScale, 0));
			mkts.Color_Out = lerp(mkts.Color_Out, sCol, sCol.a);
			mkts.Color_Out.a = alpha;
			
			// Desaturation
			float gs = (mkts.Color_Out.r + mkts.Color_Out.g + mkts.Color_Out.b) / 3;
			float gz = tex2D(_MK_FOG_DESATURATE, float2(fogScale, 0)).a;
			mkts.Color_Out = lerp(mkts.Color_Out, float4(gs, gs, gs, gz), gz);	
		}

		mkts.Color_Out.a = alpha;

		return mkts.Color_Out;
	}
#endif