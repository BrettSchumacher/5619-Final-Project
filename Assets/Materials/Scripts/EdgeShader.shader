Shader "Custom/EdgeShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Tint ("Tint", Color) = (1,1,1,1)
        _AbientColor("Ambient Color", Color) = (0.4, 0.4, 0.4, 1)
        _SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,1)
        _Glossiness("Glossiness", Float) = 32
    }
    SubShader
    {
        Pass
        {
            Tags 
            {
                "LightMode" = "ForwardBase"
                "PassFlags" = "OnlyDirectional"
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;

            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldNormal : NORMAL;
                float2 uv : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
                SHADOW_COORDS(2)
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = WorldSpaceViewDir(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                TRANSFER_SHADOW(o);
                return o;
            }

            float4 _AbientColor;
            float4 _Tint;
            float4 _SpecularColor;
            float _Glossiness;

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                float3 normal = normalize(i.worldNormal);
                float3 viewDir = normalize(i.viewDir);

                float shadow = SHADOW_ATTENUATION(i);
                float nDotL = dot(_WorldSpaceLightPos0, normal);
                float lightIntensity = nDotL < 0 ? 0.5 * smoothstep(0, 0.01, nDotL + 0.1) : (0.5 + 0.5 * smoothstep(0, 0.01, nDotL - 0.25));
                lightIntensity = lightIntensity * smoothstep(0, 0.01, shadow);

                float4 light = lightIntensity * _LightColor0;
                float3 halfVec = normalize(_WorldSpaceLightPos0 + viewDir);
                float nDotH = max(0, dot(normal, halfVec));
                float specInten = pow(nDotH * lightIntensity, _Glossiness * _Glossiness);
                float specular = smoothstep(0.005, 0.01, specInten) * _SpecularColor;

                return col * (_Tint * (_AbientColor + light) + specular);
            }
            ENDCG
        }
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}
