
Shader "StormFX Studio/Twosided/Alphablend_UV" {
    Properties {
        _Main_Diffect ("Main_Diffect", 2D) = "white" {}
        _Main_Color ("Main_Color", Color) = (0.5,0.5,0.5,1)
        _Tex_U_Speed ("Tex_U_Speed", Float ) = 0
        _Tex_V_Speed ("Tex_V_Speed", Float ) = 0
        _OpactiyTex ("OpactiyTex", 2D) = "white" {}
        _DissloveTex ("Disslove Tex", 2D) = "white" {}
        [MaterialToggle] _Disslove ("Disslove", Float ) = 1
        _DissloveSwitch ("Disslove Switch", Float ) = 1
        [MaterialToggle] _UVon ("UV on", Float ) = 0
        [MaterialToggle] _OpacitySelf ("OpacitySelf", Float ) = 0
        _Strength ("Strength", Float ) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 100
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma target 3.0
            uniform float4 _Main_Color;
            uniform sampler2D _Main_Diffect; uniform float4 _Main_Diffect_ST;
            uniform float _Tex_U_Speed;
            uniform float _Tex_V_Speed;
            uniform sampler2D _OpactiyTex; uniform float4 _OpactiyTex_ST;
            uniform sampler2D _DissloveTex; uniform float4 _DissloveTex_ST;
            uniform fixed _Disslove;
            uniform float _DissloveSwitch;
            uniform fixed _UVon;
            uniform fixed _OpacitySelf;
            uniform float _Strength;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float4 node_1892 = _Time;
                float2 _UVon_var = lerp( (float2((_Tex_U_Speed*node_1892.g),(node_1892.g*_Tex_V_Speed))+i.uv0), (i.uv0+float2(i.uv1.b,i.uv1.a)), _UVon );
                float4 _Main_Diffect_var = tex2D(_Main_Diffect,TRANSFORM_TEX(_UVon_var, _Main_Diffect));
                float3 emissive = ((_Main_Diffect_var.rgb*_Main_Color.rgb*i.vertexColor.rgb)*_Strength);
                float3 finalColor = emissive;
                float4 _OpactiyTex_var = tex2D(_OpactiyTex,TRANSFORM_TEX(i.uv0, _OpactiyTex));
                float4 _DissloveTex_var = tex2D(_DissloveTex,TRANSFORM_TEX(i.uv0, _DissloveTex));
                fixed4 finalRGBA = fixed4(finalColor,(lerp( _Main_Diffect_var.r, _Main_Diffect_var.a, _OpacitySelf )*i.vertexColor.a*_OpactiyTex_var.r*lerp( _DissloveSwitch, step(i.uv1.r,_DissloveTex_var.r), _Disslove )));
               
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma target 3.0
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    
}
