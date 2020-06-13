Shader "Custom/DistortionSpace" {
    Properties {
        _Distortion_Tex ("Distortion_Tex", 2D) = "black" {}
        _Distortion ("Distortion", Range(0, 1)) = 0.6276302
        _Speed ("Speed", Range(0, 10)) = 5.856099
        _Opacity ("Opacity", Range(0, 1)) = 1
        _Fresnel ("Fresnel", Range(0, 10)) = 1.223416
        _Bokef ("Bokef", Range(0, 5)) = 2.197412
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        GrabPass{ }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            //#define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform sampler2D _Distortion_Tex; uniform float4 _Distortion_Tex_ST;
            uniform float _Distortion;
            uniform float _Speed;
            uniform float _Opacity;
            uniform float _Fresnel;
            uniform float _Bokef;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 projPos : TEXCOORD3;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float2 sceneUVs = (i.projPos.xy / i.projPos.w);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
////// Lighting:
////// Emissive:
                float4 node_4262 = _Time;
                float2 node_7243 = lerp(i.uv0,(i.uv0+node_4262.g*float2(0.1,0.1)),_Speed);
                float4 _Distortion_Tex_var = tex2D(_Distortion_Tex,TRANSFORM_TEX(node_7243, _Distortion_Tex));
                float3 emissive = tex2D( _GrabTexture, (float3(sceneUVs.rg,0.0)+(lerp(float3(0,0,0),_Distortion_Tex_var.rgb,_Distortion)*pow((1.0 - pow(1.0-max(0,dot(normalDirection, viewDirection)),_Fresnel)),_Bokef)))).rgb;
                float3 finalColor = emissive;
                return fixed4(finalColor,_Opacity);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
