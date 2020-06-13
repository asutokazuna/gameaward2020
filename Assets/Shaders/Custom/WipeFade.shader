Shader "Custom/WipeFade"
{
	Properties
	{
		_MainTex("MainTex", 2D) = ""{}
		_MaskTex("MaskTex", 2D) = "white"{}
		_Threshold("Threshold",Range(0,1)) = 0.2
		_Timer("Timer",Range(0,1)) = 0
		_FillColor("FillColor",Color) = (0, 0, 0, 1)
		[MaterialToggle]_Reverse("Reverse",Float) = 0
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
			sampler2D _MaskTex;
			float _Threshold;
			float _Timer;
			fixed4 _FillColor;
			float _Reverse;

			fixed4 frag(v2f_img i) : COLOR 
			{
				/*
				float gray = c.r * 0.3 + c.g * 0.6 + c.b * 0.1;
				c.rgb = fixed3(gray, gray, gray);
				return c;
				*/

				float _Size = 5 * _Timer;	//5 ≒ ReductionRate

				fixed4 c = tex2D(_MainTex, i.uv);
				fixed4 d = tex2D(_MaskTex, (i.uv * _Size) - ((_Size - 1)/2));

				//fixed Alpha = 0;
				
				
				if (d.r*0.3 + d.g*0.6 + d.b*0.1 < _Threshold && !_Reverse
					|| d.r*0.3 + d.g*0.6 + d.b*0.1 > _Threshold && _Reverse)//Mask
				{
					if (distance(i.uv, fixed2(0.5, 0.5)) < (5 - _Size)) {//Circle
						return c;
					}
					return _FillColor;
				}
				return _FillColor;

				
				//i.uv -= fixed2(0.5, 0.5);
				////i.uv.x *= 16.0 / 9.0;
				//if (distance(i.uv, fixed2(0, 0)) < (20-_Size)) {
				//	return c;
				//}
				//return fixed4(0.0, 0.0, 0.0, 1.0);
				
			}

			ENDCG
		}
	}
}