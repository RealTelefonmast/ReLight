Shader "Unlit/RelightSectionCut"
{
    Properties
    {
        _MainTex ("_MainTex", 2D) = "white" {}
        //_LightMap ("_LightMap", 2D) = "white" {}
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "IgnoreProjector" = "True"
            "PreviewType" = "Plane"
        }
        
        Pass
        {
            
            ZWrite Off
            Blend DstColor Zero
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

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
            //sampler2D _LightMap;
            //float4 _LightMap_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                //fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 col = tex2D(_MainTex, i.uv);
                //fixed4 light = tex2D(_LightMap, TRANSFORM_TEX(i.uv, _LightMap));
                return col;
            }
            ENDCG
        }
    }
}