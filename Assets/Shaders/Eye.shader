Shader "_Custom/Eye"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Exponention ("dissuse", float) = 1
        _ColorMode ("0:color 1:texture", int) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "LightMode" = "ForwardBase"}
        LOD 100

        Pass
        {
            Stencil {
               Ref 20
               Comp always
               Pass replace
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
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                float diffuse : COLOR1;
                float4 vertexWorld : POSITION1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            uniform float4 _LightColor0;
            uniform float4 _Color;
            uniform float _Exponention;
            uniform int _ColorMode;

            v2f vert (appdata v)
            {
                v2f o;

                float3 normal = normalize((mul(unity_ObjectToWorld, float4(v.normal, 0.0))).xyz);
                float3 light = normalize(_WorldSpaceLightPos0.xyz);
                float dotZeroOne = dot(normal, light) * 0.5 + 0.5;
                float diffuse = pow(dotZeroOne, _Exponention);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.vertexWorld = mul(unity_ObjectToWorld, v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = fixed4(diffuse, 0, 0, 1);//v.color;
                o.normal =  normalize(mul(unity_ObjectToWorld, float4(v.normal, 0.0)).xyz);;
                o.diffuse = diffuse;

                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float3 light = normalize(_WorldSpaceLightPos0.xyz);
                float3 viewDir = normalize((float4(_WorldSpaceCameraPos, 1.0) - i.vertexWorld).xyz);
                float specular = pow(max(0.0, dot(reflect(-light, i.normal), viewDir)), 8); 
                specular *= 0.3;

                fixed4 c;
                if (_ColorMode == 0){
                    c = _Color;
                } else if (_ColorMode == 1) {
                    c = tex2D (_MainTex, i.uv);
                } else if (_ColorMode == 2) {
                    c = i.color;
                }
                else {
                    c = fixed4(1, 1, 1, 1);
                }
                c = fixed4(c.xyz * i.diffuse + specular, 1);
                return c;
            }
            ENDCG
        }
    }
}
