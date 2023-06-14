Shader "Custom/DarkenSceneShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _MaskTex("Mask Texture", 2D) = "white" {}
    }

        SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        sampler2D _MaskTex;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_MaskTex;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            fixed4 maskColor = tex2D(_MaskTex, IN.uv_MaskTex);

            // Adjust the darkening intensity as desired
            float darkenIntensity = 0.5;

            // Apply darkening to the scene, excluding the masked area
            c.rgb -= darkenIntensity * (1.0 - maskColor.r);

            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }

        ENDCG
    }
}
