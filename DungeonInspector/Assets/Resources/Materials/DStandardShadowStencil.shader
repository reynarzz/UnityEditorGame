Shader "Unlit/DStandardShadow"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _flip("Flip", vector) = (0, 0, 0, 0)
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType"="Opaque" }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off
        //ZTest Always
        
        Pass
        {
            Stencil
            {
                Ref 5
                comp Equal
               // pass IncrWrap
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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            uniform float3 _flip;
            uniform half4 _color;
            uniform float _xCutOff;
            uniform half4 _cutOffColor;
            uniform half4 _dtime;
            uniform int _isHit;

            // Atlas
            uniform int _IsAtlas;
            uniform sampler2D _AtlasTex;
            uniform float4 _AtlasRect;

            float luminosity(half4 color)
            {
                return 0.21 * color.r + 0.72 * color.g + 0.07 * color.b;

            }

#define mat4 float4x4
#define vec4 float4

            v2f vert (appdata v)
            {
                v2f o;

                float c = cos(_flip.z);
                float s = sin(_flip.z);

                mat4 rot = mat4(vec4(c,-s, 0,0),
                                vec4(s, c, 0, 0),
                                vec4(0, 0, 1, 0),
                                vec4(0, 0, 0, 1));

                vec4 vertex = v.vertex;

               // vertex = mul(rot, vertex + float4(150, 150, 0, 1) * abs(sign(_flip.z)));
                vertex = mul(unity_ObjectToWorld, v.vertex);
                vertex.z = 1;
                //vertex = mul(rot, vertex);
                mat4 V =  mul(UNITY_MATRIX_V, rot);
                mat4 mvp  = mul(V, UNITY_MATRIX_P);

                o.vertex = mul(mvp, vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

                /* float pixel = 32;

                uv = float2(uv.x * pixel, uv.y * pixel);

                uv.x = ((int)uv.x) / pixel;
                uv.y = ((int)uv.y) / pixel;*/

                fixed4 col = _color;

                if (_IsAtlas)
                {
                    col = tex2D(_AtlasTex, uv / _AtlasRect.zw + _AtlasRect.xy);
                }
                else
                {
                    col = tex2D(_MainTex, float2(abs(uv.x - _flip.x), abs(uv.y - _flip.y)));
                }


                if (uv.x <= 1- _xCutOff)
                {
                    col = col * _color;
                }
                else
                {
                    col = _cutOffColor;
                }

                if (col.a <= 0) {
                    clip(-1);
                }

                float lum = luminosity(col);
                //col = float4(lum, lum, lum, col.a);
               
                // hit effect
                if (_isHit)
                {
                    col = lerp(col, lum + 0.35f, (cos(_dtime.x * 50) + 1) * 0.5 * _isHit);
                }


                return col;

            }
            
           
            ENDCG
        }
//        Pass
//        {
//            Stencil
//            {
//                Ref 7
//                comp Equal
//                fail IncrWrap
//            }
//
//
//            CGPROGRAM
//            #pragma vertex vert
//            #pragma fragment frag
//
//            #include "UnityCG.cginc"
//
//            struct appdata
//            {
//                float4 vertex : POSITION;
//                float2 uv : TEXCOORD0;
//            };
//
//            struct v2f
//            {
//                float2 uv : TEXCOORD0;
//                UNITY_FOG_COORDS(1)
//                float4 vertex : SV_POSITION;
//            };
//
//            sampler2D _MainTex;
//            float4 _MainTex_ST;
//            uniform float3 _flip;
//            uniform half4 _color;
//            uniform float _xCutOff;
//            uniform half4 _cutOffColor;
//
//            float luminosity(half4 color)
//            {
//                return 0.21 * color.r + 0.72 * color.g + 0.07 * color.b;
//
//            }
//
//#define mat4 float4x4
//#define vec4 float4
//
//            v2f vert(appdata v)
//            {
//                v2f o;
//
//                float c = cos(_flip.z);
//                float s = sin(_flip.z);
//
//                mat4 rot = mat4(vec4(c,-s, 0,0),
//                                vec4(s, c, 0, 0),
//                                vec4(0, 0, 1, 0),
//                                vec4(0, 0, 0, 1));
//
//                vec4 vertex = v.vertex;
//
//                // vertex = mul(rot, vertex + float4(150, 150, 0, 1) * abs(sign(_flip.z)));
//                 vertex = mul(unity_ObjectToWorld, v.vertex);
//                 vertex.z = 1;
//                 //vertex = mul(rot, vertex);
//                 mat4 V = mul(UNITY_MATRIX_V, rot);
//                 mat4 mvp = mul(V, UNITY_MATRIX_P);
//
//                 o.vertex = mul(mvp, vertex);
//                 o.uv = TRANSFORM_TEX(v.uv, _MainTex);
//                 return o;
//             }
//                
//             fixed4 frag(v2f i) : SV_Target
//             {
//                 float2 uv = i.uv;
//
//                 /* float pixel = 32;
//
//                  uv = float2(uv.x * pixel, uv.y * pixel);
//
//                  uv.x = ((int)uv.x) / pixel;
//                  uv.y = ((int)uv.y) / pixel;*/
//
//                  fixed4 col = tex2D(_MainTex, float2(abs(uv.x - _flip.x), abs(uv.y - _flip.y)));
//
//                  if (col.a <= 0) {
//                      clip(-1);
//                  }
//
//                  if (uv.x >= _xCutOff)
//                  {
//                      col = col * _color;
//                  }
//                  else
//                  {
//                      col = _cutOffColor;
//                  }
//
//                  
//                  float lum = luminosity(col);
//                  //col = float4(lum, lum, lum, col.a);
//
//                  // hit effect
//                  //col = float4(1, 1, 1, col.a);
//                  float shadow = 0.19f;
//                  float4 sCol = float4(col.r - shadow, col.g - shadow, col.b - shadow, col.a);
//
//                 /* if (((int)(uv.y * 100)) * ((int)(uv.x * 100)) % 2 == 0)
//                  {
//                      sCol = col;
//                  }*/
//
//                  
//
//                  return sCol;
//              }
//                
//
//              ENDCG
//        }
    }
}
