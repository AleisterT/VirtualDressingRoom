Shader "Custom/Toczek"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Specular ("Specular", Color) = (0,0,0,1)
        _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
        _Offset ("Offset",Int) = 0
    }
    SubShader
    {
//        Tags { "Queue"="AlphaTest" "RenderType"="TransparentCutout" }
        Tags {"Queue"="Geometry" "RenderType"="Opaque"}

//        ZWrite Off
//        Blend SrcAlpha OneMinusSrcAlpha
        Cull back
        Offset [_Offset],0

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf StandardSpecular fullforwardshadows vertex:vert keepalpha //alphatest:_Cutoff

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex : TEXCOORD0;
            float3 vertexPos : TEXCOORD1;
        };

        half _Glossiness;
        half4 _Specular;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

          void vert (inout appdata_full v, out Input o) {
                UNITY_INITIALIZE_OUTPUT(Input,o);
                o.vertexPos = v.vertex;
          }
        
        void surf (Input IN, inout SurfaceOutputStandardSpecular o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Specular = _Specular.rgb;
            o.Smoothness = _Glossiness;

            float3 viewCenter = UnityObjectToClipPos(float3(0,0,0));
            float3 viewPos = UnityObjectToClipPos(IN.vertexPos);

            o.Alpha = viewCenter.z <= viewPos.z ? 1 : 0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
