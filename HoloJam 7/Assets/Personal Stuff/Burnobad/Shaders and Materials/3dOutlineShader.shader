Shader "BB_Shaders/3D-Outline-Shader"
{
    Properties
    {
        _OutlineColor("Outline Color", Color) = ( 1, 1, 1, 1)
        _Thickness("Outline Thickness", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            Cull Front
            ZWrite Off
            //ZTest Always

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile __ OUTLINE_ON
            //#define OUTLINE_ON

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;


                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            float4 _OutlineColor;
            float _Thickness;

            v2f vert (appdata v)
            {
                v2f o;

                float4 finalVert = v.vertex;

                #if defined(OUTLINE_ON)

                float3 modifiedVerts = v.vertex.xyz * (_Thickness + 1);
                finalVert = float4(modifiedVerts.xyz, v.vertex.w);
                
                #endif

                o.vertex = UnityObjectToClipPos(finalVert);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }
    }
}
