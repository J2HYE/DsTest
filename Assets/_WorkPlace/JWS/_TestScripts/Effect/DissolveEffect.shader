Shader "Ds Project/DissolveEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DissolveAmount ("Dissolve Amount", Range(0, 1)) = 0.5
        _EdgeColor ("Edge Color", Color) = (1,1,1,1)
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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _DissolveAmount;
            float4 _EdgeColor;

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

                // Dissolve logic
                float dissolve = lerp(0.0, 1.0, _DissolveAmount);
                if (col.a < dissolve)
                {
                    discard; // 픽셀 삭제
                }

                // Optional: Edge effect
                float edge = smoothstep(dissolve - 0.1, dissolve + 0.1, col.a);
                col.rgb = lerp(col.rgb, _EdgeColor.rgb, edge);

                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
