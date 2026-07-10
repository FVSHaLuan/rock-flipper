Shader "BSB/CameraFx/CameraStabilizerEffect"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1, 1, 1, 1)
		_Intensity("Intensity", Range(0, 1)) = 0
		_Multiplier("Multiplier", Range(0, 1)) = 0.15
		_SizeScale("Size scale", Range(0, 1)) = 1
		_ViewOffset("View offset", Vector) = (0,0,0,0)
		_OriginalOverride("Original Override", Range(0, 1)) = 0
	}

		SubShader
		{
			Tags { "Queue" = "Transparent" }

			Pass
			{
					CGPROGRAM
					#pragma debug
					#pragma vertex vert
					#pragma fragment frag 

					#ifndef SHADER_API_D3D11
					#pragma target 3.0
					#else
					#pragma target 4.0
					#endif

					sampler2D _MainTex;
					half4 _Color;
					float _Intensity;
					float _Multiplier;
					float _SizeScale;
					float4 _ViewOffset;
					float _OriginalOverride;

					struct data
					{
						float4 vertex : POSITION;
						float3 normal : NORMAL;
					};

					struct v2f
					{
						float4 position : POSITION;
						float4 screenPos : TEXCOORD0;
					};

					v2f vert(data i)
					{
						v2f o;
						o.position = UnityObjectToClipPos(i.vertex);
						o.screenPos = o.position;

						return o;
					}

					half4 frag(v2f i) : COLOR
					{
						float2 screenPos = i.screenPos.xy / i.screenPos.w;
						float depth = _Intensity * _Multiplier / 200;

						screenPos.x = (screenPos.x + 1) * 0.5;
						screenPos.y = 1 - (screenPos.y + 1) * 0.5;

						half4 sum = half4(0.0h, 0.0h, 0.0h, 0.0h);

						float scale = _SizeScale;
						float2 offset = _ViewOffset;

						half4 originalColor = tex2D(_MainTex, float2(screenPos.x , screenPos.y) * scale + offset);

						sum += tex2D(_MainTex, float2(screenPos.x - 5.0 * depth, screenPos.y + 5.0 * depth) * scale + offset) * 0.025;
						sum += tex2D(_MainTex, float2(screenPos.x + 5.0 * depth, screenPos.y - 5.0 * depth) * scale + offset) * 0.025;

						sum += tex2D(_MainTex, float2(screenPos.x - 4.0 * depth, screenPos.y + 4.0 * depth) * scale + offset) * 0.05;
						sum += tex2D(_MainTex, float2(screenPos.x + 4.0 * depth, screenPos.y - 4.0 * depth) * scale + offset) * 0.05;

						sum += tex2D(_MainTex, float2(screenPos.x - 3.0 * depth, screenPos.y + 3.0 * depth) * scale + offset) * 0.09;
						sum += tex2D(_MainTex, float2(screenPos.x + 3.0 * depth, screenPos.y - 3.0 * depth) * scale + offset) * 0.09;

						sum += tex2D(_MainTex, float2(screenPos.x - 2.0 * depth, screenPos.y + 2.0 * depth) * scale + offset) * 0.12;
						sum += tex2D(_MainTex, float2(screenPos.x + 2.0 * depth, screenPos.y - 2.0 * depth) * scale + offset) * 0.12;

						sum += tex2D(_MainTex, float2(screenPos.x - 1.0 * depth, screenPos.y + 1.0 * depth) * scale + offset) * 0.15;
						sum += tex2D(_MainTex, float2(screenPos.x + 1.0 * depth, screenPos.y - 1.0 * depth) * scale + offset) * 0.15;

						sum += tex2D(_MainTex, (screenPos - 5.0 * depth) * scale + offset) * 0.025;
						sum += tex2D(_MainTex, (screenPos - 4.0 * depth) * scale + offset) * 0.05;
						sum += tex2D(_MainTex, (screenPos - 3.0 * depth) * scale + offset) * 0.09;
						sum += tex2D(_MainTex, (screenPos - 2.0 * depth) * scale + offset) * 0.12;
						sum += tex2D(_MainTex, (screenPos - 1.0 * depth) * scale + offset) * 0.15;
						sum += tex2D(_MainTex, (screenPos)*scale + offset) * 0.16;
						sum += tex2D(_MainTex, (screenPos + 5.0 * depth) * scale + offset) * 0.15;
						sum += tex2D(_MainTex, (screenPos + 4.0 * depth) * scale + offset) * 0.12;
						sum += tex2D(_MainTex, (screenPos + 3.0 * depth) * scale + offset) * 0.09;
						sum += tex2D(_MainTex, (screenPos + 2.0 * depth) * scale + offset) * 0.05;
						sum += tex2D(_MainTex, (screenPos + 1.0 * depth) * scale + offset) * 0.025;

						half4 blurredColor = sum / 2 * _Color;

						return blurredColor * (1 - _OriginalOverride) + originalColor * _OriginalOverride;
					}

				ENDCG
				}
		}

			Fallback Off
}
