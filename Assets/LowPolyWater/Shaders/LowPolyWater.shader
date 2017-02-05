// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'
// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "LowPolyWater" {
	Properties {
		// Lighting
		_Color ("Color", Color) = (0,0.5,0.7)
		_Opacity ("Opacity", Range(0,1)) = 0.7
		_Gloss ("Specular Gloss", Range(0,1)) = 0.6
		_Specular ("Specular", Range(0.03,3)) = 0.6
		_SpecColor("Sun Color", Color) = (1,1,1,1)
		_Smoothness("Smoothness", Range(0,1)) = 1
		[NoScaleOffset] _FresnelTex ("Fresnel (A) ", 2D) = "" { }
		[KeywordEnum(Flat, VertexLit, PixelLit)] _Shading("Shading", Float) = 0

		// Waves
		[KeywordEnum(Off, LowQuality, HighQuality)]  _Waves("Enable Waves", Float) = 0
		_Length("Wave Length", Float) = 3.3
		_Stretch("Wave Stretch", Float) = 10
		_Speed("Wave Speed", Float) = 0.5
		_Height ("Wave Height", Float) = 1
		_Steepness ("Wave Steepness", Range(0,1)) = 0.5
		_Direction ("Wave Direction", Range(0,360)) = 180.0

		//Ripples
		_RSpeed("Ripple Speed", Float) = 1
		_RHeight ("Ripple Height", Float) = 0.2

		//Shore
		[Toggle] _EdgeBlend("Enable Shore", Float) = 0
		_ShoreColor("Shore Color", Color) = (1,1,1,1)
		_ShoreIntensity("Shore Intensity", Range(-1,1)) = 0
		_ShoreDistance("Shore Distance", Float) = 1

		//Other
		[NoScaleOffset] _NoiseTex("Noise Texture (A)", 2D) = "white" {}
		[Toggle] _ZWrite ("Write To Depth Buffer", Float) = 0
		
		[HideInInspector] __Direction ("__Direction", Vector) = (0,0,0,0)
		[HideInInspector] __Scale("__Scale", Float) = 1
		[HideInInspector] __RHeight ("__RHeight", Float) = 0.2
		[HideInInspector] __RSpeed ("__RSpeed", Float) = 0.2
		[HideInInspector] __TexSize("__TexSize", Float) = 64
		[HideInInspector] __Speed("__Speed", Float) = 0
		[HideInInspector] __Height("__Height", Float) = 0
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent-200"}
		LOD 200
		ZWrite [_ZWrite]
		
		Pass {
			Name "FORWARD"
			Tags { "LightMode" = "ForwardBase" }
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma target 2.0 
			#pragma vertex vert
			#pragma fragment frag

			#pragma shader_feature _ _SHADING_VERTEXLIT _SHADING_PIXELLIT
			#pragma shader_feature _EDGEBLEND_ON
			#pragma shader_feature _ _WAVES_OFF _WAVES_HIGHQUALITY
			#pragma multi_compile_fog

			#include "UnityStandardUtils.cginc"
			#include "UnityLightingCommon.cginc"

			#if UNITY_VERSION < 540
				#define UNITY_VERTEX_INPUT_INSTANCE_ID
				#define UNITY_VERTEX_OUTPUT_STEREO
				#define UNITY_SETUP_INSTANCE_ID(v)
				#define UNITY_TRANSFER_INSTANCE_ID(v,o)
				#define UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o)
				#define COMPUTESCREENPOS ComputeScreenPos
			#else
				#define COMPUTESCREENPOS ComputeNonStereoScreenPos
			#endif

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f {
				float4 pos : SV_POSITION;
				UNITY_FOG_COORDS(0)
				#ifdef _SHADING_PIXELLIT
					half3 worldPos : TEXOORD1;
					half3 worldNormal : TEXOORD2;
				#else
					fixed4 vertexLight : TEXCOORD1;
				#endif
				#ifdef _EDGEBLEND_ON
					float4 screenPos : TEXOORD3;
				#endif
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			half __RSpeed, __RHeight, _Opacity, _Gloss, _Specular, _Smoothness, __TexSize, __Speed;
			sampler2D _NoiseTex, _FresnelTex; 
			fixed4 _Color;

			#ifdef _EDGEBLEND_ON
				sampler2D _CameraDepthTexture;
				half _ShoreIntensity, _ShoreDistance;
				fixed4 _ShoreColor;
			#endif

			#ifndef _WAVES_OFF
				half __Height, _Length, _Stretch;
				half4 __Direction; //cos, sin, cos*steepness, sin*steepness

				#ifdef _WAVES_HIGHQUALITY
					half __Scale;

					inline half hash( half n ){
					    return frac(sin(n)*43758.5453);
					}

					inline half noise( half2 x ){
						x /= __Scale;
					    half2 p = floor(x);
					    half2 f = frac(x);
					    f = smoothstep(0.0, 1.0, f); // f = f*f*(3.0-2.0*f);
					    half n = p.x + p.y*57.0;
					    return lerp(lerp( hash(n), hash(n+1.0),f.x), lerp( hash(n+57.0), hash(n+58.0),f.x),f.y) -0.5;
					}
				#else
					inline half noise(half2 uv){
					    return smoothstep(0,1,tex2Dlod(_NoiseTex, half4(uv/__TexSize, 0,0)).a)-0.5;
					}
				#endif

				inline void gerstner(inout half3 p, half phase){
					half x = p.x*__Direction.x - p.z*__Direction.y;
					half z = p.z*__Direction.x + p.x*__Direction.y;
					half n = noise(half2(x/_Stretch, z/_Length + phase));
					p.y += __Height*n;
					p.xz -= n*__Direction.wz;
				}
			#endif

			inline half noiseTex(half2 uv){
			    return tex2Dlod(_NoiseTex, half4(uv/__TexSize, 0,0)).a-0.5;
			}

			inline half ripple(half2 p, half phase){
				return noiseTex(half2(p.x, phase+p.y))*__RHeight;
			}

			inline half4 lighting(half3 normal, half3 worldPos){
				float3 lightDir = _WorldSpaceLightPos0.xyz;
				half3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));

				//diffuse
				half3 diff = _Color*max(0, dot (normal, lightDir));

				//ambient
				half3 ambient = max(0, ShadeSH9(half4(normal, 1.0)));

				//fresnel
				half dn = max (0, dot( worldViewDir, normal ));
				half fresPower = tex2Dlod(_FresnelTex, half4(dn,dn,0,0) ).a;
				half3 fres = ambient * fresPower;
				fres = lerp(diff, fres, _Smoothness);

				//specular
				half3 h = normalize (lightDir + worldViewDir);
				half nh = max (0, dot (normal, h));
				half specPower = pow (nh, _Specular*128.0) * _Gloss;
				half3 spec = _SpecColor.rgb*specPower;

				return fixed4(
					/*rgb:  */_Color*ambient + _LightColor0.rgb * (fres + spec),
					/*alpha:*/_Opacity*(1+0.2*fresPower+specPower));
			}

			v2f vert (appdata v) {
			  	UNITY_SETUP_INSTANCE_ID(v);
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f,o);
  				UNITY_TRANSFER_INSTANCE_ID(v,o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				half4 pos0 = mul(unity_ObjectToWorld, v.vertex);

				// decode positions
				half4 offs = half4(floor(v.uv),frac(v.uv)) * half4(1.0/10000.0, 1.0/10000.0, 10.0, 10.0) - 5.0;
				float4 p = v.vertex;
				p.xz -= offs.xz;
				half3 pos1 = mul(unity_ObjectToWorld, p).xyz;
				p.xz = v.vertex.xz-offs.yw;
				half3 pos2 = mul(unity_ObjectToWorld, p).xyz;

				// ripples
				half phase = _Time[1]*__RSpeed;
				pos0.y += ripple(pos0.xz, phase);
				pos1.y += ripple(pos1.xz, phase);
				pos2.y += ripple(pos2.xz, phase);

				// waves
				#ifndef _WAVES_OFF
					phase = _Time[1]*__Speed;
					gerstner(pos0.xyz, phase);
					gerstner(pos1, phase);
					gerstner(pos2, phase);
				#endif

				half3 worldNormal = cross(pos1-pos0.xyz, pos2-pos0.xyz);
				worldNormal = normalize(worldNormal);

				#ifdef _SHADING_PIXELLIT
					o.worldNormal = worldNormal;
					o.worldPos = pos0.xyz;
				#elif _SHADING_VERTEXLIT
					o.vertexLight = lighting(worldNormal, pos0.xyz);
				#else // flat shading
					o.vertexLight = lighting(worldNormal, (pos0.xyz+pos1+pos2)/3.0);
				#endif

				o.pos = mul(UNITY_MATRIX_VP, pos0);

				#ifdef _EDGEBLEND_ON
					o.screenPos = COMPUTESCREENPOS(o.pos);
					o.screenPos.z = lerp(o.screenPos.w, mul(UNITY_MATRIX_V, pos0).z, unity_OrthoParams.w);
				#endif

				UNITY_TRANSFER_FOG(o,o.pos); // pass fog coordinates to pixel shader
				return o;
			}


			fixed4 frag (v2f i) : SV_Target {
				UNITY_SETUP_INSTANCE_ID(i);

				#ifdef _SHADING_PIXELLIT
					fixed4 c = lighting(i.worldNormal, i.worldPos);
				#else
					fixed4 c = i.vertexLight;
				#endif

				#ifdef _EDGEBLEND_ON
					half sceneZ = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos));
					sceneZ = lerp(
						LinearEyeDepth(sceneZ), //perspective
						#if defined(UNITY_REVERSED_Z)
							(1-sceneZ)*(_ProjectionParams.y - _ProjectionParams.z) - _ProjectionParams.y
						#else
							sceneZ*(_ProjectionParams.y - _ProjectionParams.z) - _ProjectionParams.y
						#endif
						, //orthographic
						unity_OrthoParams.w);

					half diff = abs(sceneZ - i.screenPos.z)/_ShoreDistance;
					diff = smoothstep(_ShoreIntensity , 1 , diff);
                    c = lerp(lerp(c, _ShoreColor, _ShoreColor.a), c, diff);
				#endif

				UNITY_APPLY_FOG(i.fogCoord, c); // apply fog
				return c;
			}
			ENDCG

		} // Pass
	} // Subshader

	Fallback "VertexLit"
	CustomEditor "LowPolyWaterShaderGUI"
} // Shader
