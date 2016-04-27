Shader "ShaderYb/wt3_1" {

	Properties 
	{
	
	    _AttenValue ("AttenValue", Range(0,10)) = 1
	    _TransVal ("Transparency Value", Range(0,1)) = 0.5
	    _TransVal2 ("Transparency Value2", Range(0,3)) = 0.5
	    _MainTex ("MainTex (RGB)", 2D) = "white" {}
	    _Color("Color", Color) = (1,1,1,1)
	    _ScrollXSpeed1 ("X MainTex Speed", Range(-10, 10)) = -1
		_ScrollYSpeed1 ("Y MainTex Speed", Range(-10, 10)) = -1
		
		_ScrollXSpeed2 ("X MainTex Speed2", Range(-10, 10)) = -1
		_ScrollYSpeed2 ("Y MainTex Speed2", Range(-10, 10)) = -1
	    
	    _NormalTex ("Normal Map", 2D) = "bump" {}
	    _NormalIntensity ("Normal Map Intensity", Range(0,2)) = 1
	    
	    _BumCube ("BumCubemap", CUBE) = "" {}
		_BumRefl ("BumReflection Power", Range(0,10)) = 0.3
        _BumAlbedo ("BumAlbedo Power", Range(0,10)) = 0.7
	    
		_EmissiveColor ("Emissive Color", Color) = (1,1,1,1)
		_EmissValue ("EmissValue", Range(-10,10)) = 0
		
		_AmbientColor  ("Ambient Color", Color) = (1,1,1,1)
		_AmbientValue ("AmbientValue", Range(-10,10)) = 0
		
		_WaterTex("Water Texture", 2D) = "white"{}
		_WaterTex2("Water Texture2", 2D) = "white"{}
		
		_WtAValue ("WtAValue", Range(0,10)) = 1
		_SpecularColor ("Specular Tint", Color) = (1,1,1,1)
		_SpecPower ("SunScale", Range(-10,200)) = 3
		//_WaterVl ("WaterVl", Range(-1000000,1000000)) = 0
		//_WaterSp ("WaterSp", Range(-10,1000)) = 6
		

	}
	
	SubShader 
	{
		Tags { "RenderType"="Tranparent"
            "Queue" = "Transparent"}
		LOD 400

		    Blend SrcAlpha OneMinusSrcAlpha
		    ZWrite Off
		CGPROGRAM
		#pragma surface surf YBBasicDiffuse alpha
		#pragma target 3.0
		#pragma debug
		
		float _AttenValue;
		float _TransVal;
		float _TransVal2;
		sampler2D _MainTex;
		float4 _Color;
		float _ScrollXSpeed1;
		float _ScrollYSpeed1;
		float _ScrollXSpeed2;
		float _ScrollYSpeed2;
		
		sampler2D _NormalTex;
		float _NormalIntensity;
		samplerCUBE _BumCube;
        float _BumRefl;
        float _BumAlbedo;
		
		float4 _EmissiveColor;
		float _EmissValue;
		float4 _AmbientColor;
		float _AmbientValue;
		
		sampler2D _RampTex;
		sampler2D _WaterTex;
		sampler2D _WaterTex2;
		float _WtAValue;
		float _WaterVl;
		float _WaterSp;
		
		float4 _SpecularColor;
		float _SpecPower;
		
		
		float _inBlack;
		float _inGamma;
		float _inWhite;
		float _outWhite;
		float _outBlack;
		

		
		
		
		struct SurfaceCustomOutput 
		{
			float3 Albedo;
			float3 Normal;
			float3 Emission;
			float3 SpecularColor;
			half Specular;
			float Gloss;
			float Alpha;
		};
		
		//PhotoshopLevels setting
		float GetPixelLevel(float pixelColor)
		{
			float pixelResult;
			pixelResult = (pixelColor * 255.0);
			pixelResult = max(0, pixelResult - _inBlack);
			pixelResult = saturate(pow(pixelResult / (_inWhite - _inBlack), _inGamma));
			pixelResult = (pixelResult * (_outWhite - _outBlack) + _outBlack)/255.0;	
			return pixelResult;
		}
		
		inline float4 LightingYBBasicDiffuse (SurfaceCustomOutput s, float3 lightDir, half3 viewDir, float atten)
		{
		    //lightDir = normalize(lightDir);
		    
		    //float3 halfVector = normalize (lightDir + viewDir);
            //float NdotH = max (0, dot (s.Normal, halfVector));
		    float diff = max (0, dot (s.Normal, lightDir));
		    //float spec = pow (NdotH, _SpecPower);
		    
			//
			//float rimLight = max(0, dot (s.Normal, viewDir)); 
			
			float hLambert = diff * 0.5 + 0.5;
			//float rimLambert = rimLight * 0.5 + 0.5;
			//rimLambert = rimLambert + (_LightColor0.rgb * _SpecularColor.rgb * spec) * (atten * 2);
			//float3 ramp = tex2D(_RampTex, float2(hLambert,rimLambert)).rgb;
			
			
		
			
			float4 col;
			col.rgb = (s.Albedo * _LightColor0.rgb * (hLambert * atten * _AttenValue));
			//col.rgb = (s.Albedo * _LightColor0.rgb  * (hLambert * atten * _AttenValue));
			
			//c.rgb = (s.Albedo * _LightColor0.rgb * diff) + (_LightColor0.rgb * _SpecularColor.rgb * spec) * (atten * 2);

			col.a = s.Alpha;
			//s.Emission = 1;
			return col;
			//return float4(s.Normal.r,s.Normal.g,s.Normal.b,1);
		}

		struct Input 
		{
			float2 uv_MainTex;
			float2 uv_NormalTex;
			
			float2 uv_WaterTex;
			float2 uv_WaterTex2;
			float3 worldRefl;
       
           INTERNAL_DATA
		};

		void surf (Input IN, inout SurfaceCustomOutput o) 
		{
		    float2 position = distance(float3(IN.uv_WaterTex.x,0,0), IN.uv_WaterTex);

		    
		    float2 waterCoord = float2(position.x, position.y);
		    
		    float2 texCoordNormal0 = waterCoord * _WaterVl;
		    float t1= _Time/_WaterSp;
		    
		    texCoordNormal0 = t1 * texCoordNormal0;
		    
		    IN.uv_WaterTex.x =t1 + texCoordNormal0;
		    IN.uv_WaterTex.y =t1 - texCoordNormal0;
		    
		    float4 wt1 = tex2D (_WaterTex, IN.uv_WaterTex);    		    		    
		    float3 wt_normal= normalize(wt1);
		   
			//float4 c;
			//c =  pow(_EmissiveColor, _EmissValue) + pow(_AmbientColor, _AmbientValue);
			
			float2 scrolledUV = IN.uv_WaterTex2;
			float xScrollValue = _ScrollXSpeed1 * _Time;
			float yScrollValue = _ScrollYSpeed1 * _Time; 
			
			scrolledUV += float2(xScrollValue, yScrollValue);
			float4 c1 = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			float4 c2 = tex2D (_WaterTex2, scrolledUV) * _Color;
			
			float2 scrolledUV2 = IN.uv_NormalTex;
			float xScrollValue2 = _ScrollXSpeed2 * _Time;
			float yScrollValue2 = _ScrollYSpeed2 * _Time; 
			//scrolledUV2 += float2(xScrollValue2, yScrollValue2);
			
			scrolledUV2 = scrolledUV2 + float2(xScrollValue2, yScrollValue2);
			//float4 c2 = tex2D (_WaterTex2, scrolledUV) * _Color;
			
			float4 nc = tex2D(_NormalTex, scrolledUV2);

            
			//nc.a = _TransVal2;
			nc.rgb = nc.rgb-1;
			//float3 normalMap = UnpackNormal(nc*_TransVal2);
			float3 normalMap = normalize(nc*_TransVal2 - c2);
			normalMap = float3(normalMap.x * _NormalIntensity, normalMap.y * _NormalIntensity, normalMap.z) - c2;
			
			
			o.Normal = normalMap + wt_normal;
			//o.Normal = wt_normal.rgb;
			//c2.a = _WtAValue;
			o.Albedo = c1.rgb * _BumAlbedo + c2.rgb*_WtAValue ;
			
			o.Alpha = _TransVal;
			
			float4 refl = texCUBE (_BumCube, WorldReflectionVector (IN, o.Normal) + IN.worldRefl);
			//IN.worldRefl.x= IN.worldRefl.x *_SunScale;
			//IN.worldRefl.y= IN.worldRefl.y /_SunScale;
			//IN.worldRefl.z= IN.worldRefl.z *_SunScale;
			
			//float4 refl = texCUBE (_BumCube, IN.worldRefl);
			
			

			
			//o.Emission = c1 - rgb1;
            o.Emission = refl.rgb * _BumRefl * refl.a;
		}
		
		ENDCG
	} 
	
	FallBack "Diffuse"
}
