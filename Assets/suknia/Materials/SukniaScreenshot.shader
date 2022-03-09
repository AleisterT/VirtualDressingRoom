Shader "Custom/Suknia - screenshot"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _BackfaceTex ("Backface albedo", 2D) = "white" {}
        _FresnelGradient ("Fresnel gradient", 2D) = "black" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Specular ("Specular", Color) = (0,0,0,1)
        _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
        [Enum(Off,2,On,0)] _Cull("Double Sided", Float) = 0 //"Back"
    }
    SubShader
    {
//        Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
        Tags {"Queue"="AlphaTest" "RenderType"="TransparentCutout"}
        LOD 200
        Cull [_Cull]

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf StandardSpecular fullforwardshadows vertex:vert alphatest:_Cutoff

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BackfaceTex;
        sampler2D _FresnelGradient;

        struct Input
        {
            float2 uv_MainTex;
            float fresnelValue;
            float3 worldNormal;
            float3 viewDir;
        };

        half _Glossiness;
        fixed4 _Specular;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

          void vert (inout appdata_full v, out Input o) {
                UNITY_INITIALIZE_OUTPUT(Input,o);
                float3 viewDir = normalize(ObjSpaceViewDir ( v.vertex ));
                o.fresnelValue = clamp ( dot ( viewDir,v.normal ),0,0.95 );
          }
        
        void surf (Input IN, inout SurfaceOutputStandardSpecular  o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c;
            float3 gradientValue = tex2D(_FresnelGradient,float2(0,IN.fresnelValue));

            //normal geometry
            if (dot(IN.worldNormal, IN.viewDir) > 0)
            {
                c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
                o.Emission = gradientValue;
            }
            //reversed geometry (2nd side)
            else
            {
                c = tex2D (_BackfaceTex, IN.uv_MainTex) * _Color;
                o.Emission = float3(0,0,0);
            }
            o.Albedo = c.rgb;
            o.Specular = _Specular.rgb;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a > 0.5 ? 1 : 0;
            
        }
        ENDCG
    }
    FallBack "Diffuse"
}
