// Upgrade NOTE: replaced 'PositionFog()' with multiply of UNITY_MATRIX_MVP by position
// Upgrade NOTE: replaced 'V2F_POS_FOG' with 'float4 pos : SV_POSITION'
// Upgrade NOTE: replaced 'defined HAS_REFLECTION' with 'defined (HAS_REFLECTION)'
// Upgrade NOTE: replaced 'defined HAS_REFRACTION' with 'defined (HAS_REFRACTION)'
// Upgrade NOTE: replaced 'defined WATER_REFLECTIVE' with 'defined (WATER_REFLECTIVE)'
// Upgrade NOTE: replaced 'defined WATER_REFRACTIVE' with 'defined (WATER_REFRACTIVE)'
// Upgrade NOTE: replaced 'defined WATER_SIMPLE' with 'defined (WATER_SIMPLE)'

Shader "FX/Water" 
{
	Properties 
	{
		_WaveScale ("Wave scale", Range (0.02,0.15)) = 0.063
		_ReflDistort ("Reflection distort", Range (0,1.5)) = 0.44
		_RefrDistort ("Refraction distort", Range (0,1.5)) = 0.40
		_RefrColor ("Refraction color", COLOR) = ( .34, .85, .92, 1)
		_Fresnel ("Fresnel (A) ", 2D) = "gray" {}
		_BumpMap ("Bumpmap (RGB) ", 2D) = "bump" {}
		WaveSpeed ("Wave speed (map1 x,y; map2 x,y)", Vector) = (19,9,-16,-7)
		_ReflectiveColor ("Reflective color (RGB) fresnel (A) ", 2D) = "" {}
		_ReflectiveColorCube ("Reflective color cube (RGB) fresnel (A)", Cube) = "" { TexGen CubeReflect }
		_HorizonColor ("Simple water horizon color", COLOR) = ( .172, .463, .435, 1)
		_MainTex ("Fallback texture", 2D) = "" {}
		_ReflectionTex ("Internal Reflection", 2D) = "" {}
		_RefractionTex ("Internal Refraction", 2D) = "" {}
	}
	// -----------------------------------------------------------
	// Fragment program cards
	Subshader 
	{
		Tags { "WaterMode"="Refractive" "RenderType"="Opaque" }
		Pass 
		{
			CGPROGRAM
			// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it uses non-square matrices
			#pragma exclude_renderers gles
			// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct v2f members ref,bumpuv,viewDir)
			#pragma exclude_renderers d3d11 xbox360
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma fragmentoption ARB_fog_exp2
			#pragma multi_compile WATER_REFRACTIVE WATER_REFLECTIVE WATER_SIMPLE
			#if defined (WATER_REFLECTIVE) || defined (WATER_REFRACTIVE)
			#define HAS_REFLECTION 1
			#endif
			#if defined (WATER_REFRACTIVE)
			#define HAS_REFRACTION 1
			#endif
			#include "UnityCG.cginc"
			uniform float4 _WaveScale4;
			uniform float4 _WaveOffset;
			#ifdef HAS_REFLECTION
			uniform float _ReflDistort;
			#endif
			#ifdef HAS_REFRACTION
			uniform float _RefrDistort;
			#endif
			struct appdata 
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};
			struct v2f 
			{
				float4 pos : SV_POSITION;
				#if defined (HAS_REFLECTION) || defined (HAS_REFRACTION)
				float3 ref;
				#endif
				float2 bumpuv[2];
				float3 viewDir;
			};
			v2f vert(appdata v)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				// scroll bump waves
				float4 temp;
				temp.xyzw = v.vertex.xzxz * _WaveScale4 + _WaveOffset;
				o.bumpuv[0] = temp.xy;
				o.bumpuv[1] = temp.wz;  
				// object space view direction (will normalize per pixel)
				o.viewDir.xzy = ObjSpaceViewDir(v.vertex);
				#if defined (HAS_REFLECTION) || defined (HAS_REFRACTION)
				// calculate the reflection vector
				float3x4 mat = float3x4 (
					0.5, 0, 0, 0.5,
					0, 0.5 * _ProjectionParams.x, 0, 0.5,
					0, 0, 0, 1
				);
				o.ref = mul (mat, o.pos);
				#endif
				return o;
			}
			#if defined (WATER_REFLECTIVE) || defined (WATER_REFRACTIVE)
			sampler2D _ReflectionTex;
			#endif
			#if defined (WATER_REFLECTIVE) || defined (WATER_SIMPLE)
			sampler2D _ReflectiveColor;
			#endif
			#if defined (WATER_REFRACTIVE)
			sampler2D _Fresnel;
			sampler2D _RefractionTex;
			uniform float4 _RefrColor;
			#endif
			#if defined (WATER_SIMPLE)
			uniform float4 _HorizonColor;
			#endif
			sampler2D _BumpMap;
			half4 frag( v2f i ) : COLOR
			{
				i.viewDir = normalize(i.viewDir);
				// combine two scrolling bumpmaps into one
				half3 bump1 = tex2D( _BumpMap, i.bumpuv[0] ).rgb;
				half3 bump2 = tex2D( _BumpMap, i.bumpuv[1] ).rgb;
				half3 bump = bump1 + bump2 - 1;
				// fresnel factor
				half fresnelFac = dot( i.viewDir, bump );
				// perturb reflection/refraction UVs by bumpmap, and lookup colors
				#ifdef HAS_REFLECTION
				float3 uv1 = i.ref; uv1.xy += bump * _ReflDistort;
				half4 refl = tex2Dproj( _ReflectionTex, uv1 );
				#endif
				#ifdef HAS_REFRACTION
				float3 uv2 = i.ref; uv2.xy -= bump * _RefrDistort;
				half4 refr = tex2Dproj( _RefractionTex, uv2 ) * _RefrColor;
				#endif
				// final color is between refracted and reflected based on fresnel
				half4 color;
				#ifdef WATER_REFRACTIVE
				half fresnel = tex2D( _Fresnel, float2(fresnelFac,fresnelFac) ).a;
				color = lerp( refr, refl, fresnel );
				#endif
				#ifdef WATER_REFLECTIVE
				half4 water = tex2D( _ReflectiveColor, float2(fresnelFac,fresnelFac) );
				color.rgb = lerp( water.rgb, refl.rgb, water.a );
				color.a = refl.a * water.a;
				#endif
				#ifdef WATER_SIMPLE
				half4 water = tex2D( _ReflectiveColor, float2(fresnelFac,fresnelFac) );
				color.rgb = lerp( water.rgb, _HorizonColor.rgb, water.a );
				color.a = _HorizonColor.a;
				#endif
				return color;
			}
			ENDCG
		}
	}
 
	// three texture, cubemaps
	Subshader 
	{
		Tags { "WaterMode"="Simple" "RenderType"="Opaque" }
		Pass 
		{
			Color (0.5,0.5,0.5,0.5)
			SetTexture [_MainTex] 
			{
				Matrix [_WaveMatrix]
				combine texture * primary
			}
			SetTexture [_MainTex] 
			{
				Matrix [_WaveMatrix2]
				combine texture * primary + previous
			}
			SetTexture [_ReflectiveColorCube] 
			{
				combine texture +- previous, primary
				Matrix [_Reflection]
			}
		}
	}  
	// dual texture, cubemaps
	Subshader
	{
		Tags { "WaterMode"="Simple" "RenderType"="Opaque" }
		Pass 
		{
			Color (0.5,0.5,0.5,0.5)
			SetTexture [_MainTex] 
			{
				Matrix [_WaveMatrix]
				combine texture
			}
			SetTexture [_ReflectiveColorCube] 
			{
				combine texture +- previous, primary
				Matrix [_Reflection] 
			}
		}
	}
	// single texture
	Subshader 
	{
		Tags { "WaterMode"="Simple" "RenderType"="Opaque" }
		Pass 
		{
			Color (0.5,0.5,0.5,0)
			SetTexture [_MainTex] 
			{
				Matrix [_WaveMatrix]
				combine texture, primary
			}
		}
	}
}