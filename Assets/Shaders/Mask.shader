Shader "Unlit/Mask"
{
    Properties
    {
        _MainTex ("Texture",2D) = "white"{}
        _CutOff("Cutoff",Range(0,1)) = 0
    }
    SubShader
    {
        LOD 100
        Blend One OneMinusSrcAlpha
        Tags { "Queue" = "Geometry-1" }
        ColorMask 0
        ZWrite Off
        
        Stencil
        {
            Ref 1
            Comp Always
            Pass Replace
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
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _CutOff;

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
                float dissolve = step(col, _CutOff);
                clip(_CutOff-dissolve);
                return float4(1,1,1,1)*dissolve;
            }
            ENDCG
        }
    }
}
