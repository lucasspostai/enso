Shader "Unlit/Water_Sprite"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		_Fade("Fade", Range(0,1)) = 0.5 
		_Amplitude("Amlplitude", Float) = 0.0075
		_Frequency("Frequency", Float) = 100
		_Speed("Speed", Float) = 0.5
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
			Blend One OneMinusSrcAlpha

			GrabPass
			{
			}

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
					fixed4 color : COLOR;
					float2 texcoord  : TEXCOORD0;
					float4 grabPos : TEXCOORD1;
				};

				fixed4 _Color;

				v2f vert(appdata_t IN)
				{
					v2f OUT;
					OUT.vertex = UnityObjectToClipPos(IN.vertex);
					OUT.texcoord = IN.texcoord;
					OUT.color = IN.color * _Color;
					#ifdef PIXELSNAP_ON
					OUT.vertex = UnityPixelSnap(OUT.vertex);
					#endif
					OUT.grabPos = ComputeGrabScreenPos(OUT.vertex);
					return OUT;
				}

				sampler2D _MainTex;
				sampler2D _AlphaTex;
				float _AlphaSplitEnabled;
				sampler2D _GrabTexture;
				float _Fade;
				float _Amplitude;
				float _Frequency;
				float _Speed;

				fixed4 SampleSpriteTexture(float2 uv)
				{
					fixed4 color = tex2D(_MainTex, uv);

	#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
					if (_AlphaSplitEnabled)
						color.a = tex2D(_AlphaTex, uv).r;
	#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

					return color;
				}

				fixed4 frag(v2f IN) : SV_Target
				{
					fixed4 color = SampleSpriteTexture(IN.texcoord) * IN.color;
					fixed4 bgColor = tex2Dproj(_GrabTexture, float4(IN.grabPos.x + sin((IN.texcoord.y + (_Time.y*_Speed)) * _Frequency) * _Amplitude, IN.grabPos.y, IN.grabPos.z, IN.grabPos.w)) * IN.color;

					color.rgb = lerp(bgColor.rgb, color.rgb, _Fade );
					color.rgb *= color.a;
					return color;

				}
			ENDCG
			}
		}
}