//shadow rendering input and output
#ifndef MK_TOON_SHADOWCASTER
	#define MK_TOON_SHADOWCASTER

	/////////////////////////////////////////////////////////////////////////////////////////////
	// VERTEX SHADER
	/////////////////////////////////////////////////////////////////////////////////////////////
	void vertShadowCaster (
		 VertexInputShadowCaster v,
		 out float4 pos : SV_POSITION,
		 out VertexOutputShadowCaster o
		 #ifdef UNITY_STEREO_INSTANCING_ENABLED
			,out VertexOutputStereoShadowCaster os
		 #endif
		)
	{
		UNITY_SETUP_INSTANCE_ID(v);
		UNITY_INITIALIZE_OUTPUT(VertexOutputShadowCaster, o);
		#ifdef UNITY_STEREO_INSTANCING_ENABLED
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(os);
		#endif
		
		#ifdef SHADOWS_CUBE //point light shadows
			pos = UnityObjectToClipPos(v.vertex);
			o.sv = mul(unity_ObjectToWorld, v.vertex).xyz - _LightPositionRange.xyz;
		#else //other shadows
			//pos with unity macros
			pos = UnityClipSpaceShadowCasterPos(v.vertex.xyz, v.normal);
			pos = UnityApplyLinearShadowBias(pos);
		#endif
	}

	#if (defined(SHADER_API_D3D9) || defined(SHADER_API_PSP2)) && defined(SHADER_STAGE_FRAGMENT) && SHADER_TARGET >= 30
	#define MK_POSFRAG(pos) float4 pos : VPOS
	#else
	#define MK_POSFRAG(pos) float4 pos : SV_POSITION
	#endif

	/////////////////////////////////////////////////////////////////////////////////////////////
	// FRAGMENT SHADER
	/////////////////////////////////////////////////////////////////////////////////////////////
	half4 fragShadowCaster 
		(
			VertexOutputShadowCaster o,
			MK_POSFRAG(vpos)
		) : SV_Target
	{	
		#ifdef SHADOWS_CUBE
			return UnityEncodeCubeShadowDepth ((length(o.sv) + unity_LightShadowBias.x) * _LightPositionRange.w);
		#else
			return 0;
		#endif
	}			
#endif