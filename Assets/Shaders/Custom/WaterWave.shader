Shader "Unlit/WaterWave"
{
    Properties
    {
		[MaterialToggle]_Distortion("Distortion",Float) = 0
		_DistortionTex("Distortion Texture(RG)", 2D) = "grey" {}
		_DistortionPower("Distortion Power", Range(0, 1)) = 0
		_WaveSpeed("WaveSpeed",Range(0,1)) = 0.1
		[Toggle] _ZWrite("ZWrite", Float) = 0	//Zバッファ有効化
		_Color("WaterColor", Color) = (0,0,0.5,1)
		_Const("Const",Range(0,2)) = 1.25	//色補正
    }
    SubShader
    {
		Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }
		Cull Back
		ZWrite[_ZWrite]

		GrabPass { }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
				half4 grabPos : TEXCOORD1;
            };

			sampler2D _MainTex;
			float _Distortion;
			sampler2D _DistortionTex;
			half4 _DistortionTex_ST;
			half _DistortionPower;
			sampler2D _GrabTexture;
			half _WaveSpeed;
			half4 _Color;
			float _Const;


            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _DistortionTex);
				o.grabPos = ComputeGrabScreenPos(o.vertex);

				return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				// w除算
				half2 uv = half2(i.grabPos.x / i.grabPos.w, i.grabPos.y / i.grabPos.w);
			
				if (_Distortion)
				{
					half2 distortion = UnpackNormal(tex2D(_DistortionTex, i.uv + _Time.x * _WaveSpeed)).rg;
					distortion *= _DistortionPower;

					uv += distortion;

					fixed4 c = tex2D(_GrabTexture, uv);
					fixed4 fin = c.r * 0.3 + c.g * 0.6 + c.b * 0.1;

					return fin * _Color * _Const;
				}
				return tex2D(_GrabTexture, uv);

			}
            ENDCG
        }
    }
}
