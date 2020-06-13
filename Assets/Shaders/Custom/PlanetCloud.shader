Shader "Unlit/PlanetCloud"
{
    Properties
    {
		_Color("Color", Color) = (1,1,1,1)

		_Noise("NoiseTex", 2D) = "white" {}	//ノイズ
		_NoiseScroll("Noise Scroll Amount", Vector) = (0.03, 0.03, 0, 0)	//スクロール

		_SubNoise("SubNoiseTex", 2D) = "white" {}	//ノイズ
		_SubNoiseScroll("SubNoise Scroll Amount", Vector) = (0.03, 0.03, 0, 0)	//スクロール

		_NoiseCutoff("Noise Cutoff", Range(0, 1)) = 0.777
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent"  "Queue" = "Transparent"}

		Blend SrcAlpha OneMinusSrcAlpha
		

		Pass
        {
			CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
				float2 noiseUV : TEXCOORD0;
				float2 subnoiseUV : TEXCOORD2;
				float2 distortUV : TEXCOORD1;
            };


			float4 _Color;

			sampler2D _Noise;	//ノイズ
			float4 _Noise_ST;
			float2 _NoiseScroll;	//スクロール

			sampler2D _SubNoise;	
			float4 _SubNoise_ST;
			float2 _SubNoiseScroll;

			float _NoiseCutoff;


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.noiseUV = TRANSFORM_TEX(v.uv, _Noise);
				o.subnoiseUV = TRANSFORM_TEX(v.uv, _SubNoise);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float2 noiseUV = float2((i.noiseUV.x + _Time.y * _NoiseScroll.x), (i.noiseUV.y + _Time.y * _NoiseScroll.y));
				float NoiseSample = tex2D(_Noise, noiseUV).r;

				float2 snoiseUV = float2((i.subnoiseUV.x + _Time.y * _SubNoiseScroll.x), (i.subnoiseUV.y + _Time.y * _SubNoiseScroll.y));
				float SubNoiseSample = tex2D(_SubNoise, snoiseUV).r;

				float Noise = (NoiseSample + SubNoiseSample) / 2;
				
				float Alpha = Noise > _NoiseCutoff ? Noise : 0;


				return fixed4(Noise, Noise, Noise, Alpha) * _Color * 1.2;
			}
            ENDCG
        }
    }
}
