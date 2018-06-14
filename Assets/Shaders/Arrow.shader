Shader "_Custom/Arrow"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "LightMode" = "ForwardBase" "Queue"="Transparent"}
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Stencil {
               Ref 10
               Comp Greater
               Pass replace
               Fail keep
               ZFail DecrSat
            }

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
                o.normal =  normal;
                o.diffuse = diffuse;

                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float3 viewDir = normalize((float4(_WorldSpaceCameraPos, 1.0) - i.vertexWorld).xyz);
                float outline = saturate(dot(viewDir, i.normal));
                outline = pow(outline, 2);
                outline = max(0, outline);

                //fixed4 c = tex2D (_MainTex, i.uv);
                //fixed4 c = fixed4(_Color.xyz * outline, 1);
                fixed4 c = fixed4(lerp(_Color.xyz, float3(1, 1, 1), outline), _Color.w);
                return c;
            }
            ENDCG
        }

         Pass
        {
            ZTest Always
            //Offset 0, -1
            ZWrite Off

            Stencil {
               Ref 1
               Comp Greater
               Pass keep
               Fail keep
               ZFail keep
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            struct appdata
            {
                float4 vertex : POSITION;
                float4 vertexWorld : POSITION1;
                float3 normal : NORMAL;
            };
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 vertexWorld : POSITION1;
                float3 normal : NORMAL;
            };
            uniform float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.vertexWorld = mul(unity_ObjectToWorld, v.vertex);
                o.normal =  normalize(mul(unity_ObjectToWorld, float4(v.normal, 0.0)).xyz);;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float3 viewDir = normalize((float4(_WorldSpaceCameraPos, 1.0) - i.vertexWorld).xyz);
                float outline = 1 - saturate(dot(viewDir, i.normal));
                float outline2 = step(0.9, outline);
                float mySin = sin(_Time.y * 6.28 / 3);
                mySin = mySin * 0.5 + 0.5;
                mySin = mySin * 0.4 + 0.2;

                fixed4 c = (1 - _Color) + 0.3;
                c = min(1, c);
                return fixed4(c.xyz, mySin);
            }
            ENDCG
        }
    }
}
