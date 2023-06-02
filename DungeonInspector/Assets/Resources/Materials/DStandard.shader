Shader "Unlit/DStandard"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _flip("Flip", vector) = (0, 0, 0, 0)
        _color("Color", COLOR) = (1,1,1,1)
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
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

#define mat4 float4x4
#define vec4 float4

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float3 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            uniform float3 _flip;
            uniform half4 _color;
            uniform float _xCutOff;
            uniform half4 _cutOffColor;
            uniform vec4 _playerPos;

            float luminosity(half4 color)
            {
                return 0.21 * color.r + 0.72 * color.g + 0.07 * color.b;

            }


            v2f vert (appdata v)
            {
                v2f o;

                float c = cos(_flip.z);
                float s = sin(_flip.z);

              
                vec4 vertex = v.vertex;
                
               // vertex = mul(rot, vertex + float4(150, 150, 0, 1) * abs(sign(_flip.z)));
                vertex = mul(unity_ObjectToWorld, v.vertex);
                vertex.z = 1;
                //vertex = mul(rot, vertex);
                mat4 mvp  = mul(UNITY_MATRIX_V, UNITY_MATRIX_P);

                o.vertex = mul(mvp, vertex);
                o.uv = float3(TRANSFORM_TEX(v.uv, _MainTex), length(float2(v.vertex.x, v.vertex.y) - float2(_playerPos.x, _playerPos.y)));
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

               /* float pixel = 32;

                uv = float2(uv.x * pixel, uv.y * pixel);

                uv.x = ((int)uv.x) / pixel;
                uv.y = ((int)uv.y) / pixel;*/

                fixed4 col = tex2D(_MainTex, float2(abs(uv.x - _flip.x), abs(uv.y - _flip.y))) * _color;
                
                //if (uv.x >= _xCutOff)
                //{
                //    col = col * _color;
                //}
                //else
                //{
                //    col = _cutOffColor;
                //}
                
               /* if (i.uv.z < 80.0f) 
                {
                    float lum = luminosity(col);
                    col = float4(lum, lum, lum, col.a);

                }
                else {
                    clip(-1);
                }*/
               
                //
                // hit effect
                //col = float4(1, 1, 1, col.a);
                return  col;
            }
            
           
            ENDCG
        }
    }
}
