Shader "Unlit/Water02"
{
    Properties
    {
		_Color("WaterColor", Color) = (0,0,0,0.5)
		_DepthMaxDistance("Depth Maximum Distance", Float) = 1

		_SurfaceNoise("Surface Noise", 2D) = "white" {}	//ノイズ
		_SurfaceNoiseCutoff("Surface Noise Cutoff", Range(0, 1)) = 0.777
		_FoamDistance("Foam Distance", Float) = 0.4	//海岸線
		_SurfaceNoiseScroll("Surface Noise Scroll Amount", Vector) = (0.03, 0.03, 0, 0)	//スクロール
		_SurfaceDistortion("Surface Distortion", 2D) = "white" {}	//ディストーション
		_SurfaceDistortionAmount("Surface Distortion Amount", Range(0, 1)) = 0.27

		[MaterialToggle] _RimLight("Rim",Float) = 0
		_RimColor("RimColor", Color) = (0.5, 0.7, 0.5, 1)
		_Edge("RimEdge",Range(0,5)) = 2.5

    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }

		Blend SrcAlpha OneMinusSrcAlpha

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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
				float4 vertex : SV_POSITION;
				half4 grabPos : TEXCOORD1;
				float4 screenPos : TEXCOORD2;
				float2 noiseUV : TEXCOORD3;
				float2 distortUV : TEXCOORD4;

			};

			sampler2D _GrabPassTexture;
			sampler2D _CameraDepthTexture;

			half4 _Color;
			float _DepthMaxDistance;

			sampler2D _SurfaceNoise;	//ノイズ
			float4 _SurfaceNoise_ST;
			float _SurfaceNoiseCutoff;

			float _FoamDistance;	//淵
			float2 _SurfaceNoiseScroll;	//スクロール

			sampler2D _SurfaceDistortion;
			float4 _SurfaceDistortion_ST;

			float _SurfaceDistortionAmount;


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.grabPos = ComputeGrabScreenPos(o.vertex);
				o.screenPos = ComputeScreenPos(o.vertex);
				o.noiseUV = TRANSFORM_TEX(v.uv, _SurfaceNoise);
				o.distortUV = TRANSFORM_TEX(v.uv, _SurfaceDistortion);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				
				//水深の計算
				float existingDepth01 = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)).r;	//
				float existingDepthLinear = LinearEyeDepth(existingDepth01);	//非線形深度を線形に変換

				float depthDifference = existingDepthLinear - i.screenPos.w;	//水面からの相対へ
				depthDifference *= 5;

				fixed4 grabColor = tex2Dproj(_GrabPassTexture, UNITY_PROJ_COORD(i.grabPos));

				float waterDepthDifference01 = saturate(depthDifference / _DepthMaxDistance);

				float4 waterColor = lerp(grabColor, _Color, waterDepthDifference01);

				//ノイズ追加・スクロール
				float2 distortSample = (tex2D(_SurfaceDistortion, i.distortUV).xy * 2 - 1) * _SurfaceDistortionAmount;

				float2 noiseUV = float2((i.noiseUV.x + _Time.y * _SurfaceNoiseScroll.x) + distortSample.x, (i.noiseUV.y + _Time.y * _SurfaceNoiseScroll.y) + distortSample.y);
				float surfaceNoiseSample = tex2D(_SurfaceNoise, noiseUV).r;

				//カットオフ
				float foamDepthDifference01 = saturate(depthDifference / _FoamDistance);
				float surfaceNoiseCutoff = foamDepthDifference01 * _SurfaceNoiseCutoff;

				float surfaceNoise = surfaceNoiseSample > _SurfaceNoiseCutoff ? 0.9 : 0;

				return waterColor + surfaceNoise;
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
