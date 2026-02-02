Shader "Custom/Sprite Outline"
{
	Properties
	{
		_MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		_OutlineThickness("Outline Thickness", Range (0.0, 0.1)) = 0.0
		_OutlineColor ("Outline Color", Color) = (1,1,1,1)
		_OutlineAlphaMultiplier("Outline Alpha Multiplier", Range (0.0, 1.0)) = 0
		_OutlineSampleQuality("Outline Sample Quality", Range (1, 128)) = 16
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
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
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
			};
			
			fixed4 _Color;
			sampler2D _MainTex;
			uniform fixed4 _MainTex_TexelSize;
			fixed4 _OutlineColor;
			fixed _OutlineThickness;
			fixed _OutlineSampleQuality;
			fixed _OutlineAlphaMultiplier;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				return OUT;
			}

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);
				return color;
			}

			fixed4 frag(v2f IN) : COLOR
			{
				// Sample the texture color and apply tint
				fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
				c.rgb *= c.a;

				// Calculate the center of the sprite in UV space
				float2 center = float2(0.5, 0.5);
				
				// Calculate the direction and distance from the center
				float2 direction = normalize(IN.texcoord - center);
				float distanceFromCenter = length(IN.texcoord - center);

				// Adjust for aspect ratio
				float aspectRatio = _MainTex_TexelSize.y / _MainTex_TexelSize.x;

				// Number of samples around the current pixel (adjustable for quality/performance)
				const int numSamples = _OutlineSampleQuality;
				float angleStep = 2.0 * UNITY_PI / numSamples;
				
				fixed alphaSum = 0.0;

				for (int i = 0; i < numSamples; i++)
				{
					// Calculate the angle for this sample
					float angle = i * angleStep;

					// Adjust offset based on aspect ratio
					float2 offset = _OutlineThickness * float2(cos(angle) / aspectRatio, sin(angle));

					// Sample the surrounding pixel in this direction
					alphaSum += SampleSpriteTexture(IN.texcoord + offset).a;
				}

				fixed averageAlpha = alphaSum / numSamples;

				// Determine if the outline should be drawn
				fixed4 outlineC = _OutlineColor;
				outlineC.rgb *= outlineC.a;

				// If the current pixel is transparent and the average alpha of the surrounding pixels is not, draw the outline
				if (c.a == 0.0 && averageAlpha > 0.0)
				{
					fixed outlineAlpha = (averageAlpha + _OutlineAlphaMultiplier);
					if (outlineAlpha > 1.0)
					{
						outlineAlpha = 1.0;
					}
					outlineC.a *= outlineAlpha;

					return outlineC;
				}

				return c;
				
			}
		ENDCG
		}
	}
}