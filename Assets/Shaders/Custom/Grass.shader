Shader "Unlit/Grass"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_NoiseTex("Noise",2D) = "white"{}
		_speed("Speed",Float) = 1.0
		_powerX("PowerX",Float) = 1.0
		_noiseSpeed("_NoiseSpeed",Float) = 1.0
		_noisePower("NoisePower",Float) = 0.1
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" "Queue" = "Transparent"}

		Pass
		{
			Cull Off
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off

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
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			sampler2D _NoiseTex;

			float _speed;
			float _powerX;
			float _noiseSpeed;
			float _noisePower;

			float random(float2 p) 
			{
				return frac(sin(dot(p, fixed2(12.9898, 78.233))) * 43758.5453);
			}

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				//o.vertex.x += (sin(_Time * _speed) * _powerX * (o.uv.y * o.uv.y));
				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				float ti = sin((_Time + worldPos.x)* _speed )* sin(2 * (_Time + worldPos.z));
				o.vertex.x += (ti * _powerX * (o.uv.y * o.uv.y));

				
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				half2 noise = tex2D(_NoiseTex, i.uv + _Time.x * _noiseSpeed).rg;
				noise *= _noisePower;
				noise.y = 0;
					

				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv + noise);
				return col;
			}
			ENDCG
		}
	}
}
