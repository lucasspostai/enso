Shader "Unlit/Water_Texture"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
		}
        LOD 100

		Lighting Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		GrabPass
		{
		}


        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
				float4 grabPos : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			sampler2D _GrabTexture;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.grabPos = ComputeGrabScreenPos(o.vertex);
                return o;
            }

			float _Fade;
			float _Amplitude;
			float _Frequency;
			float _Speed;

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 color = tex2D(_MainTex, i.uv);

				float2 uvGrab = i.grabPos.xy / i.grabPos.w;
				uvGrab.x += sin((i.uv.y + (_Time.y * _Speed)) * _Frequency) * _Amplitude;
				fixed4 bgColor = tex2D(_GrabTexture, uvGrab);
				color.rgb = lerp(bgColor.rgb, color.rgb, _Fade);

                return color;
            }
            ENDCG
        }
    }
}
