Shader "Custom/AlphaLight" {
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque"}
		Lighting Off
		ZWrite Off
		ColorMask A
		
		LOD 200

		CGPROGRAM
		#pragma surface surf NoLighting noambient alpha:blend
		
		fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, float aten)
		{
			fixed4 color;
			color.rgb = s.Albedo;
			color.a = s.Alpha;
			 
			return color;
		}
  
		sampler2D _MainTex;  
  
        struct Input {  
            float2 uv_MainTex;  
        };
  
		void surf (Input IN, inout SurfaceOutput o) {
			//half4 c = tex2D (_MainTex, IN.uv_MainTex);
            //o.Albedo = c.rgb;
			//o.Alpha = c.a;
			o.Alpha = 0.0f;
		}
		ENDCG
	}
	FallBack "Diffuse"
}