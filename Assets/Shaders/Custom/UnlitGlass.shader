Shader "Unlit/UnlitGlass"
{
	Properties{
		_Color("Color"     , Color) = (1, 1, 1, 1)
		_Smoothness("Smoothness", Range(0, 1)) = 1
		_Alpha("Alpha"     , Range(0, 1)) = 0
		[Toggle] _ZWrite("ZWrite", Float) = 0	//Zバッファ有効化
		[MaterialToggle] _RimLight("Rim",Float) = 0
		_RimColor("RimColor", Color) = (0.5, 0.7, 0.5, 1)
		_Edge("RimEdge",Range(0,5)) = 2.5
		[MaterialToggle]_Emission("_Emission",Float) = 0
		_EmissionColor("EmissionColor",Color) = (1, 1, 1, 1)

	}

	SubShader
	{
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent"}

		ZWrite[_ZWrite]
		Blend DstColor Zero		// 背景とのブレンド法を「乗算」に指定

		Pass 
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			half3 _Color;
			half _Alpha;

			struct appdata {
				float4 vertex : POSITION;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
			};


			v2f vert(appdata v) 
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);

				return o;
			}

			fixed4 frag(v2f i) : SV_Target 
			{
				return fixed4(lerp(_Color, 0, _Alpha), 1);
			}
			ENDCG
		}

		// V/FシェーダーはReflection Probeに反応しないので
		// 反射だけを描画するSurface Shaderを追記する
		CGPROGRAM
	
		#pragma target 3.0
		#pragma surface surf Standard alpha

		half _Smoothness;

		struct Input {
			float3 worldNormal;
			float3 viewDir;
		};
		fixed4 _RimColor;
		half _Edge;
		half _RimLight;
		float _Emission;
		fixed4 _EmissionColor;

		void surf(Input IN, inout SurfaceOutputStandard o) 
		{
			//[RimColor]
			if (_RimLight)
			{
				float rim = 1 - saturate(dot(IN.viewDir, o.Normal));
				o.Emission = _RimColor * pow(rim, _Edge);	//Rim変数(光)の減衰をシャープにする為に乗算.
			}

			o.Smoothness = _Smoothness;
			if (_Emission)
			{
				o.Emission = (_EmissionColor);
			}
		}
		ENDCG
	}
	FallBack "Standard"
}