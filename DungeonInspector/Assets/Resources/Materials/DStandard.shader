Shader "Unlit/DStandard"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _flip ("Flip", vector) = (0, 0, 0, 0)
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType"="Opaque" }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Stencil
        {
            Ref 2
            comp Equal
        }
        
        Pass
        {
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
            uniform float2 _flip;

            float luminosity(half4 color)
            {
                return 0.21 * color.r + 0.72 * color.g + 0.07 * color.b;

            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, float2(abs(i.uv.x - _flip.x), abs(i.uv.y - _flip.y)));
                
                float lum = luminosity(col);
                //col = float4(lum, lum, lum, col.a);

                // hit effect
                //col = float4(1, 1, 1, col.a);

                return  col;
            }
            
           
            ENDCG
        }
    }
}
