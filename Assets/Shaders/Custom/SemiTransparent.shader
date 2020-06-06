Shader "Custom/SemiTransparent"
{
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	    _SubTex("Albedo (RGB)", 2D) = "white" {}
		_Blend("Blend",Range(0, 1)) = 1

			//X方向のシフトとスピードに関するパラメータを追加
		_XShift("Xuv Shift", Range(-1.0, 1.0)) = 0.1
		_XSpeed("X Scroll Speed", Range(1.0, 100.0)) = 10.0
		
		//Y方向のシフトとスピードに関するパラメータを追加
		_YShift("Yuv Shift", Range(-1.0, 1.0)) = 0.1
		_YSpeed("Y Scroll Speed", Range(1.0, 100.0)) = 10.0
	}

		SubShader{
			Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}
			LOD 200

			Pass{
			  ZWrite ON
			  ColorMask 0
			}

			CGPROGRAM
			#pragma surface surf Standard fullforwardshadows alpha:fade
			#pragma target 3.0

			sampler2D _MainTex;
			sampler2D _SubTex;
			float _Blend;

			float _XShift;
			float _YShift;
			float _XSpeed;
			float _YSpeed;

			struct Input {
				float2 uv_MainTex;
				float2 uv_SubTex;
			};

			fixed4 _Color;

			void surf(Input IN, inout SurfaceOutputStandard o)
			{
				_XShift = _XShift * _XSpeed;
				_YShift = _YShift * _YSpeed;

				IN.uv_SubTex.x = IN.uv_SubTex.x + _XShift * _Time;
				IN.uv_SubTex.y = IN.uv_SubTex.y + _YShift * _Time;

				IN.uv_MainTex.x = IN.uv_MainTex.x + _XShift * _Time;
				IN.uv_MainTex.y = IN.uv_MainTex.y + _YShift * _Time;

				//_Blend = 

				fixed4 main = tex2D(_MainTex, IN.uv_MainTex);
				fixed4 sub = tex2D(_SubTex, IN.uv_SubTex);
				fixed4 c = (main * (1 - _Blend) + sub * _Blend) * _Color;
				o.Albedo = c.rgb;
				o.Metallic = 0;
				o.Smoothness = 0;
				o.Alpha = c.a;
			}
			ENDCG
	}
		FallBack "Diffuse"
}
