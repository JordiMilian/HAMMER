Shader "Unlit/SimpleInPuddleShaderTest"
{
     Properties
    {
        _MainTex ("Texture", 2D) = "white" {} // Main texture property for the sprite
        _Color ("Color", Color) = (1,1,1,1) // Flat color property
        _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5 // Alpha cutoff property
    }
    SubShader
    {
        Tags { "Queue"="AlphaTest" "RenderType"="TransparentCutout" }
        LOD 200

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

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _Color;
            float _Cutoff;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.texcoord);
                clip(texColor.a - _Cutoff); // Alpha cutout
                return fixed4(_Color.rgb, texColor.a); // Apply flat color and preserve alpha
            }
            ENDCG
        }
    }
}
