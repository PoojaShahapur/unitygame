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
				float4 oPos		: POSITION;
				float3 noiseCoord : TEXCOORD0;
				float4 projectionCoord : TEXCOORD1;
				float3 oEyeDir : TEXCOORD2;
				float3 oNormal : TEXCOORD3;
			};

			v2f vert(float4 pos	: POSITION,
					 float4 normal : NORMAL,
					 float2 tex : TEXCOORD0 )
			{
				v2f o;
				o.oPos = mul(worldViewProjMatrix, pos);

				float4x4 scalemat = float4x4 (
					0.5, 0, 0, 0.5,
					0, 0.5 * _ProjectionParams.x, 0, 0.5,
					0, 0, 0.5, 0.5,
					0, 0, 0, 1
					);
				o.projectionCoord = mul(scalemat, o.oPos);
				o.noiseCoord.xy = (tex + (timeVal * scroll)) * scale;
				o.noiseCoord.z = noise * timeVal;

				o.oEyeDir = normalize(pos.xyz - eyePosition);
				o.oNormal = normal.rgb;
				return o;
			}

			float4 frag(v2f i) : COLOR
			{
				half4 outcolor = half4(1,1,1,1);
				float2 final = i.projectionCoord.xy / i.projectionCoord.w;

				float3 noiseNormal = (tex2D(noiseMap, (i.noiseCoord.xy / 5)).rgb - 0.5).rbg * noiseScale;
				final += noiseNormal.xz;

				float fresnel = fresnelBias + fresnelScale * pow(1 + dot(i.oEyeDir, i.oNormal), fresnelPower);

				float4 reflectionColour = tex2D(reflectMap, final);
				float4 refractionColour = tex2D(refractMap, final) + tintColour;

				outcolor = lerp(refractionColour, reflectionColour, fresnel);
				return outcolor;
			}
			ENDCG
		}
	}
}