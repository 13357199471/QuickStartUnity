Shader "Custom/YUVPlayer"
{
	Properties
	{
		_MainTex ("MainTexture", 2D) = "Black" {}
		_YTex ("YTexture", 2D) = "Black" {}
        _UTex ("UTexture", 2D) = "White" {}
        _VTex ("VTexture", 2D) = "White" {}
        _YCutPercent("YCutPercent",float) = 1.0
        _UCutPercent("UCutPercent",float) = 1.0
		_VCutPercent("VCutPercent",float) = 1.0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

            sampler2D _YTex;
            sampler2D _UTex;
            sampler2D _VTex;
            float _YCutPercent;
            float _UCutPercent;
			float _VCutPercent;

			float4 _YTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _YTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
                fixed2 uv = fixed2(i.uv.x,1 - i.uv.y);
				uv.x = uv.x * _YCutPercent;
                fixed y = tex2D(_YTex, uv).a;

				uv = fixed2(i.uv.x,1 - i.uv.y);
				uv.x = uv.x * _UCutPercent;
                fixed u = tex2D(_UTex, uv).a - 0.5;

				uv = fixed2(i.uv.x,1 - i.uv.y);
				uv.x = uv.x * _VCutPercent;
				fixed v = tex2D(_VTex, uv).a - 0.5;
                
                float r = y + 1.403*v;
                float g = y - 0.344*u - 0.714*v;
                float b = y + 1.77*u;
			
#if  !UNITY_COLORSPACE_GAMMA
				r = GammaToLinearSpaceExact(r);
				g = GammaToLinearSpaceExact(g);
				b = GammaToLinearSpaceExact(b);
#endif
				float4 outcolor = fixed4(r, g, b, 1);
				return outcolor;
			}
			ENDCG
		}
	}
}