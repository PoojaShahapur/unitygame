Shader "Test/GameCore/Mobile/Refraction/Mirror"
{
	Properties
	{
		_RefractionTex("Refraction", 2D) = "white" {TexGen ObjectLinear }
		_RefractionColor("Color",Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			uniform float4x4 _ProjMatrix;
			sampler2D _RefractionTex;
			float4 _RefractionColor;
			struct outvertex 
			{
				float4 pos : SV_POSITION;
				float3 uv : TEXCOORD0;
				float4 posProj : TEXCOORD1;
			};
			outvertex vert(appdata_tan v) 
			{
				outvertex o;
				o.pos = mul(UNITY_MATRIX_MVP,v.vertex);
				o.posProj = mul(_ProjMatrix, v.vertex);
				return o;
			}
			float4 frag(outvertex i) : COLOR
			{
				half4 reflcol = tex2D(_RefractionTex,float2(i.posProj) / i.posProj.w);
				return reflcol*_RefractionColor;
			}
			ENDCG
		}
	}
}