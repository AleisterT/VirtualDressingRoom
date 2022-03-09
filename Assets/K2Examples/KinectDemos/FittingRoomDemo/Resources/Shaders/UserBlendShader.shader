﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/UserBlendShader" 
{
	Properties
	{
		_MainTex ("MainTex", 2D) = "white" {}
		_BackTex ("BackTex", 2D) = "white" {}
        _ColorTex ("ColorTex", 2D) = "white" {}
        _Threshold ("Depth Threshold", Range(0, 0.5)) = 0.1
		_BlurOffset("Blur Offset", Range(0, 10)) = 2
	}

	SubShader 
	{
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }

		Pass
		{
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
		
			CGPROGRAM
			#pragma target 5.0
			//#pragma enable_d3d11_debug_symbols

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			//float4 _MainTex_ST;
			uniform float4 _MainTex_TexelSize;
			sampler2D _CameraDepthTexture;

			uniform sampler2D _BackTex;
			uniform sampler2D _ColorTex;
			uniform float _Threshold;
			uniform float _HeadThreshold;
			uniform int _BlurOffset;

			uniform float _ColorResX;
			uniform float _ColorResY;
			uniform float _DepthResX;
			uniform float _DepthResY;

			uniform float _ColorOfsX;
			uniform float _ColorMulX;
			uniform float _ColorOfsY;
			uniform float _ColorMulY;

			uniform float4 _HeadPosition;
			
			//uniform float _DepthFactor;

			StructuredBuffer<float2> _DepthCoords;
			StructuredBuffer<float> _DepthBuffer;


			struct v2f 
			{
			   float4 pos : SV_POSITION;
			   float2 uv : TEXCOORD0;
			   float2 uv2 : TEXCOORD1;
			   float4 scrPos : TEXCOORD2;
			};

			v2f vert (appdata_base v)
			{
			   v2f o;
			   
			   o.pos = UnityObjectToClipPos (v.vertex);
			   o.uv = v.texcoord;

			   o.uv2.x = o.uv.x;
			   o.uv2.y = 1 - o.uv.y;

			   o.scrPos = ComputeScreenPos(o.pos);

			   return o;
			}

			half getKinectAlpha(int2 cxy, float camDepth, float3 screenPos)
			{
				float threshold = _Threshold;
				float2 screenPosScaled = screenPos.xy;
				screenPosScaled.y = screenPosScaled.y * 16/9;

				float2 head_pos_scaled = _HeadPosition.xy - float2(0,0.02);
				head_pos_scaled.y = head_pos_scaled.y * 16/9;
				
				float headDistance = abs(length(screenPosScaled - head_pos_scaled));

				float radius = 0.15;
				float transitionRadius = 0.01;
				
				// if(headDistance < radius)
				// {
				// 	threshold = _HeadThreshold;
				// }
				// if(headDistance >= radius && headDistance < (radius+transitionRadius))
				// {
				// 	threshold = lerp(_HeadThreshold,_Threshold,(headDistance-radius)/transitionRadius);
				// }
				
				int rcCount = 2 * _BlurOffset + 1;
				int maxCount = rcCount * rcCount;

				int ci0 = (int)((cxy.x - _BlurOffset) + (cxy.y - _BlurOffset) * _ColorResX);
				int pixCount = 0;

				for (int iY = -_BlurOffset; iY <= _BlurOffset; iY++)
				{
					for (int iX = -_BlurOffset, ci = ci0; iX <= _BlurOffset; iX++, ci++)
					{
						if (!isinf(_DepthCoords[ci].x) && !isinf(_DepthCoords[ci].y))
						{
							int dx = (int)_DepthCoords[ci].x;
							int dy = (int)_DepthCoords[ci].y;
							int di = (int)(dx + dy * _DepthResX);

							float kinDepth = _DepthBuffer[di] / 1000.0;

							if ((camDepth < 0.1 || camDepth >= 10.0) || (kinDepth >= 0.1 && camDepth > (kinDepth + threshold)))
							{
								pixCount++;
							}
						}
						else
						{
							if (camDepth < 0.1 || camDepth >= 10.0)
							{
								pixCount++;
							}
						}
					}

					ci0 += _ColorResX;
				}

				half alpha = (half)pixCount / (half)maxCount;

				return alpha;
			}

			half4 frag (v2f i) : COLOR
			{
			    float camDepth = LinearEyeDepth (tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos)).r);
				//float camDepth01 = Linear01Depth (tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos)).r);

				float2 ctUv = float2(_ColorOfsX + i.uv.x * _ColorMulX, 1.0 - i.uv.y /**_ColorOfsY + i.uv.y * _ColorMulY*/);
#if UNITY_UV_STARTS_AT_TOP
                if (_MainTex_TexelSize.y < 0)
                {
                    ctUv.y = 1.0 - ctUv.y;
                }
#endif

				// for non-flipped textures
				float2 ctUv2 = float2(ctUv.x, 1.0 - ctUv.y);
				
				int cx = (int)(ctUv.x * _ColorResX);
				int cy = (int)(ctUv.y * _ColorResY);
				int ci = (int)(cx + cy * _ColorResX);

				//return half4(1.0 - kinDepth/3,0,1,1);
				//return half4(i.uv.x, i.uv.y, 0, 1);
				half4 clrBack = tex2D(_BackTex, ctUv2);
				half4 clrFront = tex2D(_ColorTex, ctUv);
				half3 clrBlend = clrBack.rgb * (1.0 - clrFront.a) + clrFront.rgb * clrFront.a;
				//clrBlend = float3(1, 0, 0);

				half4 clrMain = tex2D(_MainTex, i.uv);
				half kinAlpha = getKinectAlpha(int2(cx, cy), camDepth,i.scrPos) * (1-clrMain.a);
				if(clrMain.a == 0)
				{
					kinAlpha = 1;
				}
				//clrBlend = lerp(clrMain.rgb, clrBlend.rgb, kinAlpha);
				clrBlend = clrMain.rgb * (1.0 - kinAlpha) + clrBlend * kinAlpha;//float3(kinAlpha,kinAlpha,kinAlpha);

				//main - render unity (zbroja na niebieskim tle)
				//back - tło (plaża)
				//front - kamera

				// i.scrPos.y = i.scrPos.y / 9 * 16;
				// float4 headPos = _HeadPosition;
				// headPos.y = headPos.y / 9 * 16;
				// float headDistance = abs(length(i.scrPos.xy - headPos.xy));
				// if(headDistance< 0.2)
				// {
				// 	return half4(clrMain.xyz,1);
				// }
				//
				// if(headDistance >= 0.2 && headDistance < 0.3)
				// {
				// 	float blendRation = (headDistance-0.2)/0.1;
				// 	return half4(lerp(clrMain,clrBlend,blendRation),1);
				// }
				
				return half4(clrBlend.rgb, 1.0);
			}

			ENDCG
		}
	}

	FallBack "Diffuse"
}
