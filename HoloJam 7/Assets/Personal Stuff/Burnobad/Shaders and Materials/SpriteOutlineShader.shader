Shader "BB_Shaders/SpriteOutlineShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    
        _OutlineColor("Outline Color", Color) = ( 1, 1, 1, 1)
        _Thickness("Outline Thickness", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "CanUseSpriteAtlas"="True"
        }

        Pass
        {
            Name "Outlines"
                     
            Tags { "LightMode"="Always" }

            ZWrite Off
            ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile __ OUTLINE_ON

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

            float4 _OutlineColor;
            float _Thickness;

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            static const float2 newUVs[8] =
            {
                float2( 1,  0),
                float2(-1,  0),
                float2( 0,  1),
                float2( 0, -1),
                float2( 1,  1),
                float2(-1,  1),
                float2( 1, -1),
                float2(-1, -1)
            };

            fixed4 frag (v2f i) : SV_Target
            {
                #if defined(OUTLINE_ON)
                    float alpha = 0;
                    for(int k = 0; k < 8; k++)
                    {
                        float2 newUV = newUVs[k] * _Thickness;
                        alpha += tex2D(_MainTex, i.uv + newUV).a;
                    }

                    alpha = saturate(alpha);
                    clip(alpha - 0.1);

                    return fixed4(_OutlineColor.rgb, 1) * alpha;
                #else
                    clip(-1);
                    return 0;
                #endif
            }
            ENDCG
        }
        Pass
        {
            Name "Main Sprite"

            Tags { "LightMode"="Always" }

            ZWrite Off
            ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off

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

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                clip(col.a - 0.1);

                return col;
            }
            ENDCG
        }
    }
}
