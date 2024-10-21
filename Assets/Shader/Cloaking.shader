Shader "Custom/Cloaking"
{
    Properties
    {
        _Color ("Primary Color (R)", Color) = (1,1,1,0.5)
        _Color1 ("Secondary Color (G)", Color) = (0.5,0.5,0.5,0.5)
        _Color2 ("Tretiary Color (B)", Color) = (0,0,0,0.5)
        _MainTex ("Fallback (RGB)", 2D) = "white" {}
        _MaskTex ("Mask", 2D) = "black" {}
        _BumpMap ("Normals", 2D) = "" {}
        _SpecMap ("Specular", 2D) = "" {}
        _AOMap ("Ambient Occlusion", 2D) = "" {}
        _AOScale ("Ambient Occlusion Intensity", Range(1.0,10.0)) = 1
        _DetailMap ("Pattern (R)", 2D) = "" {}
        _Power("Vertex Color Intensity", Range(1.0,16.0) ) = 2.0
        [HDR]_GlowColor ("Self-Illumination Color", Color) = (0,0,0,1)
        _DamageColor ("Damage Color", Color) = (1,1,1,0)
        _BurnLevel("Burn Level", Range(0.0,1.0)) = 0.0
        _Opacity ("Opacity", Range(0.0, 1.0)) = 0.5
        [HDR]_OutColor("OutColor", Color) = (1,1,1,1)
        _NoiseTex("Noise Texture", 2D) = "white" {}
        _OutThinkness("OutThinkness", Range(0, 2.0)) = 1.15
        _CutRender("_CutRender", Range(0.0, 1.0)) = 0.0
    }

    SubShader {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent"  }

        CGPROGRAM
        #pragma surface surf StandardSpecular alpha:fade
        #pragma target 4.0

        fixed4 _Color;
        fixed4 _Color1;
        fixed4 _Color2;
        fixed4 _DamageColor;
        fixed4 _GlowColor;
        fixed _BurnLevel;
        float _Opacity;

        sampler2D _MaskTex;
        sampler2D _SpecMap;
        sampler2D _BumpMap;
        sampler2D _DetailMap;

        sampler2D _AOMap;
        fixed _AOScale;

        sampler2D _NoiseTex;
        float4 _OutColor;
        float _OutThinkness;
        fixed _CutRender;

        struct Input {
            float2 uv_MainTex;
            float2 uv_NoiseTex;
            float2 uv2_DetailMap;
            float3 viewDir;
        };

        float _Power;

        void surf (Input IN, inout SurfaceOutputStandardSpecular o)
        {
            // Calculate colors and masks
            fixed4 col = fixed4(0,0,0,0);
            fixed4 mask = tex2D(_MaskTex, IN.uv_MainTex);
            fixed4 spec = tex2D(_SpecMap, IN.uv_MainTex);
            fixed4 patt = tex2D(_DetailMap, IN.uv2_DetailMap);
            
            // Optimize color calculations
            col += (_Color * mask.r + _Color1 * mask.g + _Color2 * mask.b) * patt;

            o.Albedo = col.rgb * (1 - _BurnLevel) * _Power;
            o.Smoothness = col.a * (1 - _BurnLevel);
            o.Specular = spec.g * (1 - _BurnLevel);

            // Ambient occlusion
            fixed4 ao = tex2D(_AOMap, IN.uv_MainTex);
            o.Albedo *= ao * _AOScale;

            // Calculate normals and emission
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
            fixed4 noise = tex2D(_NoiseTex, IN.uv_NoiseTex);
            
            float alpha = step(_CutRender, noise.r);
            float outline = step(noise.r, _CutRender * _OutThinkness);

            //fixed4 glowColor = saturate((_GlowColor.rgb * mask.a * _GlowColor.a + _DamageColor.rgb * _DamageColor.a) * (1 - _BurnLevel));
            fixed4 glowColor = saturate(fixed4((_GlowColor.rgb * mask.a * _GlowColor.a + _DamageColor.rgb * _DamageColor.a) * (1 - _BurnLevel), 0));
            o.Emission = (outline * _OutColor.rgb) + (col.rgb * mask.rgb) + (glowColor * _Opacity);
            o.Alpha = alpha * _Opacity;
        }
        ENDCG
    }
    FallBack "Diffuse"
}