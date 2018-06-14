Shader "_Custom/Outline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "LightMode" = "ForwardBase"}
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
                float4 vertex : SV_POSITION;
                float4 vertexWorld : POSITION1;
                float3 normal : NORMAL;
                float diffuse : COLOR1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            uniform float4 _LightColor0;
            uniform float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                float3 normal = normalize((mul(unity_ObjectToWorld, float4(v.normal, 0.0))).xyz);
                float3 light = normalize(_WorldSpaceLightPos0.xyz);
                float dotZeroOne = max(0.0, dot(normal, light)) * 0.5 + 0.5;
                float diffuse = pow(dotZeroOne, 0.8);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.vertexWorld = mul(unity_ObjectToWorld, v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                o.normal =  normalize(mul(unity_ObjectToWorld, float4(v.normal, 0.0)).xyz);;
                o.diffuse = diffuse;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float3 viewDir = normalize((float4(_WorldSpaceCameraPos, 1.0) - i.vertexWorld).xyz);
                float outline = saturate(dot(viewDir, i.normal));
                outline = pow(outline, 2);
                outline = max(0, outline);
                fixed4 c = fixed4(lerp(_Color.xyz, float3(1, 1, 1), outline), 1);
                return c;
            }
            ENDCG
        }
    }
}
