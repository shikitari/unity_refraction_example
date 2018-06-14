Shader "_Custom/Cup3"
{
    Properties
    {
        _Color ("Water Color", Color) = (0,0,1,0.3)
        _MainTex ("Texture", 2D) = "white" {}
        _Refract ("Refract Indices", float) = 0.666666
        _Distance ("Ray trace distance", float) = 3
        _Visible ("1:visible 0:hidden", int) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
                float3 direction : DIRECTION;
            };

            fixed4 _Color;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Refract;
            float _Distance;
            float4x4 _RTCameraProjection;
            float4x4 _RTCameraView;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                o.vertexWorld = mul(unity_ObjectToWorld, v.vertex);
                o.direction = normalize(o.vertexWorld - _WorldSpaceCameraPos.xyz);
                o.normal =  normalize(mul(unity_ObjectToWorld, float4(v.normal, 0.0)).xyz);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float4 pos;
                if (_Refract >= 0.99999 && _Refract <= 1.00001) {
                    pos = i.vertexWorld + float4(i.direction, 0) * _Distance;
                } else {
                    float3 r = refract(i.direction, i.normal, _Refract);
                    pos = i.vertexWorld + float4(r, 0) * _Distance;
                }

                // Diffrent MainCamera's FOV and RTCamera's FOV. MainCamera's FOV is Depend on viewport aspect ratio.
                //pos = mul(UNITY_MATRIX_VP, pos);

                // In order to do "Scene View" debug. comment out.
                //pos = mul(UNITY_MATRIX_V, pos);

                pos = mul(_RTCameraView, pos);
                pos = mul(_RTCameraProjection, pos);

                pos = float4(pos.xyz / pos.w, 1);
          
                float2 uvCoord = (pos.xy) * 0.5 + 0.5;
                uvCoord = clamp(uvCoord, 0, 1);
                
                fixed4 c = tex2D (_MainTex, uvCoord);
                c = lerp(c, fixed4(_Color.xyz, 1), _Color.w);
                return c;
            }
            ENDCG
        }
    }
}
