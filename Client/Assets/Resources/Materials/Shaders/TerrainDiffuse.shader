Shader "My/Terrain/TerrainSplatDiffuse"
{
	Properties 
	{
		_Splat0 ("Layer 0 (R)", 2D) = "white" {}
		_Splat1 ("Layer 1 (G)", 2D) = "white" {}
		_Splat2 ("Layer 2 (B)", 2D) = "white" {}
		_Splat3 ("Layer 3 (A)", 2D) = "white" {}
		_Control ("Control (RGBA)", 2D) = "red" {}
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 150

		CGPROGRAM
		#pragma surface surf Lambert noforwardadd

		sampler2D _Splat0;
		sampler2D _Splat1;
		sampler2D _Splat2;
		sampler2D _Splat3;
		sampler2D _Control;

		struct VertexInput 
		{
			float4 vertex : POSITION;
			float4 tangent : TANGENT;
			float3 normal : NORMAL;
			float4 texcoord : TEXCOORD0;
		};
		
		struct Vertex2Frag
		{
			float4 pos : SV_POSITION;
			float3 uv : TEXCOORD0;
			float4 tangent : TANGENT;
			float3 normal : NORMAL;
		};
		
		void SplatmapMix(Vertex2Frag V2F, out fixed4 mixedDiffuse)
		{
			half4 splat_control = tex2D(_Control, V2F.uv);
			mixedDiffuse = 0.0f;
			mixedDiffuse += splat_control.r * tex2D(_Splat0, V2F.uv);
			mixedDiffuse += splat_control.g * tex2D(_Splat1, V2F.uv);
			mixedDiffuse += splat_control.b * tex2D(_Splat2, V2F.uv);
			mixedDiffuse += splat_control.a * tex2D(_Splat3, V2F.uv);
		}
		
		Vertex2Frag vert(VertexInput v) 
		{
			Vertex2Frag o;
			o.pos = mul(UNITY_MATRIX_MVP,v.vertex);
			o.tangent = v.tangent;
			o.normal = v.normal;
			o.uv = v.texcoord;
			return o;
		}

		void surf (Vertex2Frag IN, inout SurfaceOutput o) 
		{
			fixed4 mixedDiffuse;
			SplatmapMix(IN, mixedDiffuse);
			o.Albedo = mixedDiffuse.rgb;
			o.Alpha = weight;
			o.Smoothness = mixedDiffuse.a;
			o.Metallic = dot(splat_control, half4(_Metallic0, _Metallic1, _Metallic2, _Metallic3));
		}
		
		ENDCG
	}

	Fallback "Mobile/Diffuse"
}
