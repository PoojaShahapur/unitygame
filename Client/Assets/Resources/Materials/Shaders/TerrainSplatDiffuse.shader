Shader "My/Terrain/TerrainSplatDiffuse"
{
	Properties
	{
		_MainTex("Layer 0 (R)", 2D) = "white" {}
		_Splat1("Layer 1 (G)", 2D) = "white" {}
		_Splat2("Layer 2 (B)", 2D) = "white" {}
		_Splat3("Layer 3 (A)", 2D) = "white" {}
		_Control("Control (RGBA)", 2D) = "red" {}
		_UVMultiplier("UVMultiplier", Vector) = (32, 32, 32, 32)
	}
	
	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 150

		CGPROGRAM
		#pragma surface surf Lambert noforwardadd
		//#pragma surface surf BlinnPhong noforwardadd

		sampler2D _MainTex;
		sampler2D _Splat1;
		sampler2D _Splat2;
		sampler2D _Splat3;
		sampler2D _Control;
		uniform float4 _UVMultiplier;

		struct Input
		{
			float2 uv_MainTex;
		};

		void SplatmapMixSurf(Input V2F, out fixed4 mixedDiffuse)
		{
			half4 splat_control = tex2D(_Control, V2F.uv_MainTex);
			mixedDiffuse = 0.0f;
			mixedDiffuse += splat_control.r * tex2D(_MainTex, V2F.uv_MainTex * _UVMultiplier.x);
			mixedDiffuse += splat_control.g * tex2D(_Splat1, V2F.uv_MainTex * _UVMultiplier.y);
			mixedDiffuse += splat_control.b * tex2D(_Splat2, V2F.uv_MainTex * _UVMultiplier.z);
			mixedDiffuse += splat_control.a * tex2D(_Splat3, V2F.uv_MainTex * _UVMultiplier.w);
		}

		void surf(Input IN, inout SurfaceOutput o)
		{
			fixed4 mixedDiffuse;
			SplatmapMixSurf(IN, mixedDiffuse);
			o.Albedo = mixedDiffuse.rgb;
			o.Alpha = mixedDiffuse.a;
		}

		ENDCG
	}

	Fallback "Mobile/Diffuse"
}