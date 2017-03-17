Shader "ct/Diffuse Detail" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_liangdu1("lingt1",Range(0, 10)) = 1
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Detail ("Detail (RGB)", 2D) = "gray" {}
	_NormalTex("NormalTex (RGB)", 2D) = "bump" {}
	_Xspeed("X speed", Range(-100, 100)) = 0
	_Yspeed("Y speed", Range(-100, 100)) = 0
	_Color2("Other Color", Color) = (1,1,1,1)
	_OtherTex("Other (RGB)", 2D) = "white" {}
	_Xspeed2("X speed2", Range(-100, 100)) = 0
	_Yspeed2("Y speed2", Range(-100, 100)) = 0
	_liangdu2("lingt2",Range(0, 10)) = 1
}

SubShader {
	Tags { "RenderType"="Opaque" "Queue" = "Transparent" }
	LOD 250
		
	
CGPROGRAM
#pragma surface surf Lambert alphatest:_Cutoff

sampler2D _MainTex;
sampler2D _NormalTex;
sampler2D _Detail;
fixed4 _Color;
fixed4 _Color2;
sampler2D _OtherTex;

fixed _Xspeed;
fixed _Yspeed;

fixed _Xspeed2;
fixed _Yspeed2;
fixed _liangdu1;
fixed _liangdu2;
struct Input {
	float2 uv_MainTex;
	float2 uv_Detail;
	float2 uv_NormalTex;
	float2 uv_OtherTex;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	
	fixed2 uvmo = IN.uv_NormalTex ;
	fixed2 uvmo2 = IN.uv_OtherTex;

	fixed xs = _Xspeed *_Time;
	fixed ys = _Yspeed *_Time;
	uvmo += fixed2(xs, ys);

	fixed xs2 = _Xspeed2 *_Time;
	fixed ys2 = _Yspeed2 *_Time;

	uvmo2+= fixed2(xs2, ys2);

	fixed4 c2 = tex2D(_OtherTex, uvmo2) * _Color2;
	float3 normalMap = UnpackNormal(tex2D(_NormalTex, uvmo));

	c.rgb *= tex2D(_Detail,IN.uv_Detail).rgb * unity_ColorSpaceDouble.r;

	//o.Albedo = c.rgb * c2.rgb;
	o.Albedo = lerp(c.rgb, c2.rgb, _liangdu2);
	o.Alpha = c.a*_liangdu1;

	o.Normal = normalMap.rgb;
}
ENDCG
}

Fallback "Legacy Shaders/Diffuse"
}
