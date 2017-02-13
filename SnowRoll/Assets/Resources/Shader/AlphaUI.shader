Shader "Test/UIETC"
{
    Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" { }
		_AlphaTex("AlphaTex", 2D) = "white" { }
	}
    SubShader
	{
		Tags
		{
			"RenderType"="Transparent"
			"Queue"="Transparent"
			//"IgnoreProjector"="True"
            //"PreviewType"="Plane"
            //"CanUseSpriteAtlas"="True"
		}
		
		Blend SrcAlpha OneMinusSrcAlpha
		
		Pass
		{
			Lighting Off
			ZTest Off
			
			Cull Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _AlphaTex;

			float _AlphaFactor;

			struct v2f
			{
				float4  pos : SV_POSITION;
				float2  uv : TEXCOORD0;
				float4 color :COLOR;
			};

			half4 _MainTex_ST;
			half4 _AlphaTex_ST;

			v2f vert (appdata_full v)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.texcoord;
				o.color = v.color;
				return o;
			}

			half4 frag (v2f i) : COLOR
			{
				half4 texcol = tex2D(_MainTex, i.uv);

				half4 result = texcol;

				//result.a = tex2D(_AlphaTex, i.uv).a * i.color.a;
				result.a = tex2D(_AlphaTex, i.uv).a;
				
				//if(result.a == 1)
				//{
				//	result = float4(0.5, 0.5, 0.5, 0.5);
				//}
				//else
				//{
				//	result = float4(1, 1, 1, 1);
				//}

				return result;
			}
			ENDCG
		}
    }
}