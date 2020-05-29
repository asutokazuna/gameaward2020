Shader "Unlit/PlanetWater"
{
    Properties
    {
		_Color("WaterColor", Color) = (0,0,0,0.5)
		_DepthMaxDistance("Depth Maximum Distance", Float) = 1

		[MaterialToggle] _RimLight("Rim",Float) = 0
		_RimColor("RimColor", Color) = (0.5, 0.7, 0.5, 1)
		_Edge("RimEdge",Range(0,5)) = 2.5

		_A("A",Float) = 1.0

    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }

		Blend SrcAlpha OneMinusSrcAlpha
		//ZWrite Off


		GrabPass { "_GrabPassTexture" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
				float4 vertex : SV_POSITION;
				half4 grabPos : TEXCOORD1;
				float4 screenPos : TEXCOORD2;

			};

			sampler2D _GrabPassTexture;
			sampler2D _CameraDepthTexture;

			half4 _Color;
			float _DepthMaxDistance;

			float _A;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.grabPos = ComputeGrabScreenPos(o.vertex);
				o.screenPos = ComputeScreenPos(o.vertex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				
				//水深の計算
				float existingDepth01 = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)).r;	//
				float existingDepthLinear = LinearEyeDepth(existingDepth01);	//非線形深度を線形に変換

				float depthDifference = existingDepthLinear - i.screenPos.w;	//水面からの相対へ
				depthDifference *= _A;

				fixed4 grabColor = tex2Dproj(_GrabPassTexture, UNITY_PROJ_COORD(i.grabPos));

				float waterDepthDifference01 = saturate(depthDifference / _DepthMaxDistance);

				float4 waterColor = lerp(grabColor, _Color, waterDepthDifference01);

				return waterColor;
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


		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			//[RimColor]
			if (_RimLight)
			{
				float rim = 1 - saturate(dot(IN.viewDir, o.Normal));
				o.Emission = _RimColor * pow(rim, _Edge);	//Rim変数(光)の減衰をシャープにする為に乗算.
			}

			o.Smoothness = _Smoothness;
		}
		ENDCG
	}
}
