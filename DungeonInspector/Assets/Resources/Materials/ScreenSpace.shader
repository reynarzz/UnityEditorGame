Shader "Unlit/ScreenSpace"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }


		Pass
		{
			Stencil
			{
				Ref 5
				comp LEqual
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

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

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _dtime;

			float luminosity(half4 color)
			{
				return 0.21 * color.r + 0.72 * color.g + 0.07 * color.b;

			}

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);

			//    if (i.uv.z < 80.0f)
			//    {
			//        float lum = luminosity(col);
			//        col = float4(lum, lum, lum, col.a);

			//        /*if (i.uv.z > 76.0f)
			//        {
			//            col = float4(1, 1, 1, 1) * 0.5f;

			//        }*/
			//    }
			//else {
			//    clip(-1);
			//}
			float dist = length(i.uv - 0.5);

			//if (dist < 0.1) {
			//	float lum = luminosity(col);
			//	//col = ;

			//	col.b += 0.05f;
			//}
			//else 
			{
				//(int)((i.uv.y) * 360 + _dtime.x * 0.1)
				if (sin((i.uv.y + _dtime.x * 0.01f) * 1000) < 0)
				{
					float lum = luminosity(col);
					//col = float4(lum, lum, lum, col.a);

					col.rgb -= 0.05;
					col.b += 0.05f;
					//col.g += 0.01f;
					//clip(-1);

				}
			}
				


				return col;
			}
			ENDCG
		}
	}
}
