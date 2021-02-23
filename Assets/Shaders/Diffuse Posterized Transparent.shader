Shader "InfiniteEcho/Diffuse Posterized Transparent" {
	Properties {
		_Color ( "Albedo", Color ) = ( 1, 1, 1, 1 )
		_MainTex ( "Main Surface Texture", 2D ) = "white" {}

		_LowColor ( "Low Color", Color ) = ( .25, .25, .25, 1 )
		_HighColor ( "High Color", Color ) = ( .75, .75, .75, 1 )
		_LerpThreshMid ( "Color Lerping Threshold Midpoint", Range( 0, 1 ) ) = .5
		_LerpThreshRange ( "Color Lerping Threshold Range", Range( 0, 1 ) ) = .06
		// _LerpThreshLow ( "Ignore Low", Range( 0, 1 ) ) = .25
		// _LerpThreshHigh ( "Ignore High", Range( 0, 1 ) ) = .75
	}

	SubShader {
		Tags { "RenderMode"="Transparent" "Queue"="Transparent" }

		LOD 200

		Blend SrcAlpha OneMinusSrcAlpha

		Pass {
			Tags { "LightMode"="ForwardBase" }

			Cull Back

			CGPROGRAM

				#include "UnityCG.cginc"
				#include "Lighting.cginc"
				#include "AutoLight.cginc"

				#pragma vertex vert
				#pragma fragment pixel
				#pragma multi_compile_fwdbase

				// Properties
				float4 _Color;
				sampler2D _MainTex;
				
				float4 _LowColor;
				float4 _HighColor;
				float _LerpThreshMid;
				float _LerpThreshRange;

				// float _LerpThreshLow;
				// float _LerpThreshHigh;

				// Structs
				struct appdata {
					float4 pos : POSITION;
					float3 normal : NORMAL;
					float2 uv : TEXCOORD0;
				};

				struct pixelIn {
					float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
					float3 worldNormal : TEXCOORD2;
					float2 lerpThresh : TEXCOORD3;
					SHADOW_COORDS( 1 )
					float3 diffuseColor : COLOR0;
					float3 ambientColor : COLOR1;
				};


				// Vertex
				pixelIn vert( appdata v ) {
					pixelIn o;

					o.pos = UnityObjectToClipPos( v.pos );
					o.uv = v.uv;

					// _LerpThreshLow = _LerpThreshMid - ( _LerpThreshRange / 2 );
					// _LerpThreshHigh = _LerpThreshMid + ( _LerpThreshRange / 2 );
					o.lerpThresh.x = _LerpThreshMid - ( _LerpThreshRange / 2 );
					o.lerpThresh.y = _LerpThreshMid + ( _LerpThreshRange / 2 );

					o.worldNormal = UnityObjectToWorldNormal( v.normal );
					float dotNormal = max( 0, dot( o.worldNormal, normalize( _WorldSpaceLightPos0 ) ) );
					o.diffuseColor = _LightColor0 * dotNormal;
					o.ambientColor = ShadeSH9( float4( o.worldNormal, 1 ) );

					TRANSFER_SHADOW( o )

					return o;
				}

				// Pixel
				float4 pixel( pixelIn i ) : SV_TARGET {
					float4 outColor = _Color * tex2D( _MainTex, i.uv );

					// outColor.rgb *= i.diffuseColor * SHADOW_ATTENUATION( i ) + i.ambientColor;

					float dotNormal = max( 0, dot( i.worldNormal, _WorldSpaceLightPos0 ) );
					float lerpThresh = ( dotNormal - i.lerpThresh.x ) / _LerpThreshRange;
					float4 normalColor = 
						( dotNormal < i.lerpThresh.x ) *
							_LowColor +
						( dotNormal >= i.lerpThresh.x && dotNormal < i.lerpThresh.y ) *
							( ( 1 - lerpThresh ) * _LowColor + lerpThresh * _HighColor ) +
						( dotNormal >= i.lerpThresh.y ) *
							_HighColor;

					outColor = normalColor * SHADOW_ATTENUATION( i );

					return outColor;
				}

			ENDCG
		}


		Pass {
			Tags { "LightMode"="ShadowCaster" }

			Cull Back

			CGPROGRAM
				#include "UnityCG.cginc"

				#pragma vertex vert
				#pragma fragment pixel
				#pragma multi_compile_shadowcaster

				struct pixelIn {
					V2F_SHADOW_CASTER;
				};

				pixelIn vert( appdata_base v ) {
					pixelIn o;
					TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
					return o;
				}

				float4 pixel( pixelIn i ) : SV_TARGET {
					SHADOW_CASTER_FRAGMENT( i )
				}
			ENDCG
		}
	}
}