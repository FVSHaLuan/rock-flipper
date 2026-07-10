// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "BSB/Sprites-WithVerticesMatrix"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		_ClearColor("Clear color", Color) = (0,0,0,0)
		_OutTexColor("Out of texture color", Color) = (0,0,0,0)
		_AlphaTexture("Alpha Texture", 2D) = "white" {}
		_SoftEdge("Soft Edge", Float) = 0.95
	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Blend SrcAlpha OneMinusSrcAlpha

			// Clear
			Pass
			{
				Blend Off
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				struct appdata_t
				{
					float4 vertex   : POSITION;
				};

				struct v2f
				{
					float4 vertex   : SV_POSITION;
				};

				v2f vert(appdata_t IN)
				{
					v2f OUT;
					OUT.vertex = UnityObjectToClipPos(IN.vertex);
	#ifdef PIXELSNAP_ON
						OUT.vertex = UnityPixelSnap(OUT.vertex);
	#endif
						return OUT;
					}

					fixed4 _ClearColor;

					fixed4 frag(v2f IN) : COLOR
					{
						///
						return _ClearColor;
					}
				ENDCG
				}

			// Main pass
			Pass
			{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile DUMMY PIXELSNAP_ON
				#include "UnityCG.cginc"

				struct appdata_t
				{
					float4 vertex   : POSITION;
					float4 color    : COLOR;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex   : SV_POSITION;
					fixed4 color : COLOR;
					float2 texcoord  : TEXCOORD0;
				};

				fixed4 _Color;
				fixed4 _OutTexColor;
				float4x4 _VertexTransform;

				v2f vert(appdata_t IN)
				{
					v2f OUT;
					OUT.vertex = UnityObjectToClipPos(IN.vertex);
					OUT.texcoord = mul(_VertexTransform, float4(IN.texcoord, 0, 1)).xy;
					OUT.color = IN.color * _Color;
					#ifdef PIXELSNAP_ON
					OUT.vertex = UnityPixelSnap(OUT.vertex);
					#endif

					return OUT;
				}

				sampler2D _MainTex;
				sampler2D _AlphaTexture;
				float _SoftEdge;

				fixed4 frag(v2f IN) : COLOR
				{
					///
					float2 uv = IN.texcoord;
					half4 texcol = tex2D(_MainTex, uv);
					texcol = texcol * IN.color;

					///
					texcol.a = tex2D(_AlphaTexture, uv).a;

					///
					half outTex = step(0, uv.x) * step(uv.x, 1) * step(0, uv.y) * step(uv.y, 1);
					texcol = texcol * outTex + (1 - outTex) * _OutTexColor;

					///
					float2 clampedUV = clamp(uv, float2(0, 0), float2(1, 1));
					float _SoftEdge2 = _SoftEdge * 0.5f;
					float softEdgeMultiplier = (0.5 - clamp(abs(clampedUV.x - 0.5), _SoftEdge2, 0.5)) / (0.5 - _SoftEdge2);
					softEdgeMultiplier *= (0.5 - clamp(abs(clampedUV.y - 0.5), _SoftEdge2, 0.5)) / (0.5 - _SoftEdge2);
					texcol.a *= softEdgeMultiplier;

					///
					return texcol;
				}
			ENDCG
			}
		}
			Fallback "Sprites/Default"
}