Shader "Test/Water/SimpleWater"
{
	Properties
	{
		noiseMap("Refraction", 2D) = "white" {}
		reflectMap("Reflection", 2D) = "white" {}
		refractMap("Refraction", 2D) = "white" {}
	}
		
	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 100
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			uniform float4x4 worldViewProjMatrix;
			uniform float3 eyePosition;
			uniform float timeVal;
			uniform float scale;
			uniform float scroll;
			uniform float noise;

			uniform float4 tintColour;
			uniform float noiseScale;
			uniform float fresnelBias;
			uniform float fresnelScale;
			uniform float fresnelPower;
			uniform sampler2D noiseMap;
			uniform sampler2D reflectMap;
			uniform sampler2D refractMap;

			struct v2f
			{
				float4 oPos		: POSITION,
				float3 noiseCoord : TEXCOORD0,
				float4 projectionCoord : TEXCOORD1,
				float3 oEyeDir : TEXCOORD2,
				float3 oNormal : TEXCOORD3,
			};

			v2f vert(float4 pos	: POSITION,
					 float4 normal : NORMAL,
					 float2 tex : TEXCOORD0, )
			{
				oPos = mul(worldViewProjMatrix, pos);

				float4x4 scalemat = float4x4 (
					.5, 0, 0, .5,
					0, .5 * _ProjectionParams.x, 0, .5,
					0, 0, .5, .5,
					0, 0, 0, 1
					);
				projectionCoord = mul(scalemat, oPos);
				noiseCoord.xy = (tex + (timeVal * scroll)) * scale;
				noiseCoord.z = noise * timeVal;

				oEyeDir = normalize(pos.xyz - eyePosition);
				oNormal = normal.rgb;
			}

			float4 frag(v2f i) : COLOR
			{
				float2 final = projectionCoord.xy / projectionCoord.w;

				float3 noiseNormal = (tex2D(noiseMap, (noiseCoord.xy / 5)).rgb - 0.5).rbg * noiseScale;
				final += noiseNormal.xz;

				float fresnel = fresnelBias + fresnelScale * pow(1 + dot(eyeDir, normal), fresnelPower);

				float4 reflectionColour = tex2D(reflectMap, final);
				float4 refractionColour = tex2D(refractMap, final) + tintColour;

				col = lerp(refractionColour, reflectionColour, fresnel);
			}
			ENDCG
		}
	}
}