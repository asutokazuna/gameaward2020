Shader "Custom/NormalFade"
{
	Properties
	{
		_MainTex("MainTex", 2D) = ""{}
		_Timer("Timer",Range(0,1)) = 0
		_Color("Color",Color) = (1, 1, 1, 1)
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM

			#include "UnityCG.cginc"

			#pragma vertex vert_img
			#pragma fragment frag

			sampler2D _MainTex;
			float  _Timer;
			fixed4 _Color;

			fixed4 frag(v2f_img i) : COLOR
			{
				fixed4 c = tex2D(_MainTex, i.uv) * (_Color * (1 - _Timer));

				return c;
			}

			ENDCG
		}
	}
}