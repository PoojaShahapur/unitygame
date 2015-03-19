Shader "Custom/AlphaTestDiffuse" {
    Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		//_Cutoff ("Alpha cutoff", Range (0,1)) = 2
	}
    SubShader {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
		//AlphaTest Equal [_Cutoff]
		Lighting Off
		Blend OneMinusDstAlpha DstAlpha
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;

        struct Input {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o) {
            half4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            //o.Alpha = c.a;
			o.Alpha = 1.0f;
			//clip (o.Alpha - _Cutoff);
        }
        ENDCG
    }
    FallBack "Diffuse"
}