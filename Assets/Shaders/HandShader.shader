Shader "Custom/HandShader"
{
    Properties
    {
        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"}
        
        Pass {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes {
                float4 positionOS: POSITION;
            };

            struct Varyings {
                float4 positionHCS : SV_POSITION;
            };

            Varyings vert(Attributes i) {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(i.positionOS.xyz);
                return o;
            };

            half4 frag() : SV_TARGET {
                return half4(0.5, 0, 0, 1);
            }
            ENDHLSL
        }
    }
}
