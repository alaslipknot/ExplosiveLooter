// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "HardBit/MainPost"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		_Intensity("Intensity", Float) = 1
		_SDepthColor("SDepthColor", Color) = (0,0,0,0)
		_ScreenDepth("ScreenDepth", Float) = 50
		_Scale("Scale", Range( 0 , 1)) = 1
		_ScanTexture("ScanTexture", 2D) = "white" {}
		_WorldPosition("WorldPosition", Vector) = (0,0,0,0)
		_Power("Power", Range( 0 , 1)) = 1
		_Step("Step", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		LOD 0

		
		
		ZTest Always
		Cull Off
		ZWrite Off

		
		Pass
		{ 
			CGPROGRAM 

			

			#pragma vertex vert_img_custom 
			#pragma fragment frag
			#pragma target 2.0
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"


			struct appdata_img_custom
			{
				float4 vertex : POSITION;
				half2 texcoord : TEXCOORD0;
				
			};

			struct v2f_img_custom
			{
				float4 pos : SV_POSITION;
				half2 uv   : TEXCOORD0;
				half2 stereoUV : TEXCOORD2;
		#if UNITY_UV_STARTS_AT_TOP
				half4 uv2 : TEXCOORD1;
				half4 stereoUV2 : TEXCOORD3;
		#endif
				float4 ase_texcoord4 : TEXCOORD4;
			};

			uniform sampler2D _MainTex;
			uniform half4 _MainTex_TexelSize;
			uniform half4 _MainTex_ST;
			
			UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
			uniform float4 _CameraDepthTexture_TexelSize;
			uniform float _ScreenDepth;
			uniform float4 _SDepthColor;
			uniform float _Step;
			uniform float _Intensity;
			uniform float3 _WorldPosition;
			uniform float _Scale;
			uniform sampler2D _ScanTexture;
			uniform float4 _ScanTexture_ST;
			uniform float _Power;
			float2 UnStereo( float2 UV )
			{
				#if UNITY_SINGLE_PASS_STEREO
				float4 scaleOffset = unity_StereoScaleOffset[ unity_StereoEyeIndex ];
				UV.xy = (UV.xy - scaleOffset.zw) / scaleOffset.xy;
				#endif
				return UV;
			}
			
			struct Gradient
			{
				int type;
				int colorsLength;
				int alphasLength;
				float4 colors[8];
				float2 alphas[8];
			};
			
			Gradient NewGradient(int type, int colorsLength, int alphasLength, 
			float4 colors0, float4 colors1, float4 colors2, float4 colors3, float4 colors4, float4 colors5, float4 colors6, float4 colors7,
			float2 alphas0, float2 alphas1, float2 alphas2, float2 alphas3, float2 alphas4, float2 alphas5, float2 alphas6, float2 alphas7)
			{
				Gradient g;
				g.type = type;
				g.colorsLength = colorsLength;
				g.alphasLength = alphasLength;
				g.colors[ 0 ] = colors0;
				g.colors[ 1 ] = colors1;
				g.colors[ 2 ] = colors2;
				g.colors[ 3 ] = colors3;
				g.colors[ 4 ] = colors4;
				g.colors[ 5 ] = colors5;
				g.colors[ 6 ] = colors6;
				g.colors[ 7 ] = colors7;
				g.alphas[ 0 ] = alphas0;
				g.alphas[ 1 ] = alphas1;
				g.alphas[ 2 ] = alphas2;
				g.alphas[ 3 ] = alphas3;
				g.alphas[ 4 ] = alphas4;
				g.alphas[ 5 ] = alphas5;
				g.alphas[ 6 ] = alphas6;
				g.alphas[ 7 ] = alphas7;
				return g;
			}
			
			float4 SampleGradient( Gradient gradient, float time )
			{
				float3 color = gradient.colors[0].rgb;
				UNITY_UNROLL
				for (int c = 1; c < 8; c++)
				{
				float colorPos = saturate((time - gradient.colors[c-1].w) / (gradient.colors[c].w - gradient.colors[c-1].w)) * step(c, (float)gradient.colorsLength-1);
				color = lerp(color, gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), gradient.type));
				}
				#ifndef UNITY_COLORSPACE_GAMMA
				color = half3(GammaToLinearSpaceExact(color.r), GammaToLinearSpaceExact(color.g), GammaToLinearSpaceExact(color.b));
				#endif
				float alpha = gradient.alphas[0].x;
				UNITY_UNROLL
				for (int a = 1; a < 8; a++)
				{
				float alphaPos = saturate((time - gradient.alphas[a-1].y) / (gradient.alphas[a].y - gradient.alphas[a-1].y)) * step(a, (float)gradient.alphasLength-1);
				alpha = lerp(alpha, gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), gradient.type));
				}
				return float4(color, alpha);
			}
			


			v2f_img_custom vert_img_custom ( appdata_img_custom v  )
			{
				v2f_img_custom o;
				float4 ase_clipPos = UnityObjectToClipPos(v.vertex);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord4 = screenPos;
				
				o.pos = UnityObjectToClipPos( v.vertex );
				o.uv = float4( v.texcoord.xy, 1, 1 );

				#if UNITY_UV_STARTS_AT_TOP
					o.uv2 = float4( v.texcoord.xy, 1, 1 );
					o.stereoUV2 = UnityStereoScreenSpaceUVAdjust ( o.uv2, _MainTex_ST );

					if ( _MainTex_TexelSize.y < 0.0 )
						o.uv.y = 1.0 - o.uv.y;
				#endif
				o.stereoUV = UnityStereoScreenSpaceUVAdjust ( o.uv, _MainTex_ST );
				return o;
			}

			half4 frag ( v2f_img_custom i ) : SV_Target
			{
				#ifdef UNITY_UV_STARTS_AT_TOP
					half2 uv = i.uv2;
					half2 stereoUV = i.stereoUV2;
				#else
					half2 uv = i.uv;
					half2 stereoUV = i.stereoUV;
				#endif	
				
				half4 finalColor;

				// ase common template code
				float4 screenPos = i.ase_texcoord4;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float clampDepth175 = Linear01Depth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
				float4 ScreenDepth180 = ( (( clampDepth175 * _ScreenDepth )*1.0 + 0.1) * _SDepthColor );
				float2 uv0_MainTex = i.uv.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float2 localCenter138_g194 = uv0_MainTex;
				float4 break146 = ( _MainTex_TexelSize * _Step );
				float temp_output_2_0_g194 = break146.x;
				float localNegStepX156_g194 = -temp_output_2_0_g194;
				float temp_output_3_0_g194 = break146.y;
				float localStepY164_g194 = temp_output_3_0_g194;
				float2 appendResult14_g202 = (float2(localNegStepX156_g194 , localStepY164_g194));
				float4 tex2DNode16_g202 = tex2D( _MainTex, ( localCenter138_g194 + appendResult14_g202 ) );
				float temp_output_2_0_g202 = (tex2DNode16_g202).r;
				float temp_output_4_0_g202 = (tex2DNode16_g202).g;
				float temp_output_5_0_g202 = (tex2DNode16_g202).b;
				float localTopLeft172_g194 = ( sqrt( ( ( ( temp_output_2_0_g202 * temp_output_2_0_g202 ) + ( temp_output_4_0_g202 * temp_output_4_0_g202 ) ) + ( temp_output_5_0_g202 * temp_output_5_0_g202 ) ) ) * _Intensity );
				float2 appendResult14_g198 = (float2(localNegStepX156_g194 , 0.0));
				float4 tex2DNode16_g198 = tex2D( _MainTex, ( localCenter138_g194 + appendResult14_g198 ) );
				float temp_output_2_0_g198 = (tex2DNode16_g198).r;
				float temp_output_4_0_g198 = (tex2DNode16_g198).g;
				float temp_output_5_0_g198 = (tex2DNode16_g198).b;
				float localLeft173_g194 = ( sqrt( ( ( ( temp_output_2_0_g198 * temp_output_2_0_g198 ) + ( temp_output_4_0_g198 * temp_output_4_0_g198 ) ) + ( temp_output_5_0_g198 * temp_output_5_0_g198 ) ) ) * _Intensity );
				float localNegStepY165_g194 = -temp_output_3_0_g194;
				float2 appendResult14_g201 = (float2(localNegStepX156_g194 , localNegStepY165_g194));
				float4 tex2DNode16_g201 = tex2D( _MainTex, ( localCenter138_g194 + appendResult14_g201 ) );
				float temp_output_2_0_g201 = (tex2DNode16_g201).r;
				float temp_output_4_0_g201 = (tex2DNode16_g201).g;
				float temp_output_5_0_g201 = (tex2DNode16_g201).b;
				float localBottomLeft174_g194 = ( sqrt( ( ( ( temp_output_2_0_g201 * temp_output_2_0_g201 ) + ( temp_output_4_0_g201 * temp_output_4_0_g201 ) ) + ( temp_output_5_0_g201 * temp_output_5_0_g201 ) ) ) * _Intensity );
				float localStepX160_g194 = temp_output_2_0_g194;
				float2 appendResult14_g195 = (float2(localStepX160_g194 , localStepY164_g194));
				float4 tex2DNode16_g195 = tex2D( _MainTex, ( localCenter138_g194 + appendResult14_g195 ) );
				float temp_output_2_0_g195 = (tex2DNode16_g195).r;
				float temp_output_4_0_g195 = (tex2DNode16_g195).g;
				float temp_output_5_0_g195 = (tex2DNode16_g195).b;
				float localTopRight177_g194 = ( sqrt( ( ( ( temp_output_2_0_g195 * temp_output_2_0_g195 ) + ( temp_output_4_0_g195 * temp_output_4_0_g195 ) ) + ( temp_output_5_0_g195 * temp_output_5_0_g195 ) ) ) * _Intensity );
				float2 appendResult14_g196 = (float2(localStepX160_g194 , 0.0));
				float4 tex2DNode16_g196 = tex2D( _MainTex, ( localCenter138_g194 + appendResult14_g196 ) );
				float temp_output_2_0_g196 = (tex2DNode16_g196).r;
				float temp_output_4_0_g196 = (tex2DNode16_g196).g;
				float temp_output_5_0_g196 = (tex2DNode16_g196).b;
				float localRight178_g194 = ( sqrt( ( ( ( temp_output_2_0_g196 * temp_output_2_0_g196 ) + ( temp_output_4_0_g196 * temp_output_4_0_g196 ) ) + ( temp_output_5_0_g196 * temp_output_5_0_g196 ) ) ) * _Intensity );
				float2 appendResult14_g197 = (float2(localStepX160_g194 , localNegStepY165_g194));
				float4 tex2DNode16_g197 = tex2D( _MainTex, ( localCenter138_g194 + appendResult14_g197 ) );
				float temp_output_2_0_g197 = (tex2DNode16_g197).r;
				float temp_output_4_0_g197 = (tex2DNode16_g197).g;
				float temp_output_5_0_g197 = (tex2DNode16_g197).b;
				float localBottomRight179_g194 = ( sqrt( ( ( ( temp_output_2_0_g197 * temp_output_2_0_g197 ) + ( temp_output_4_0_g197 * temp_output_4_0_g197 ) ) + ( temp_output_5_0_g197 * temp_output_5_0_g197 ) ) ) * _Intensity );
				float temp_output_133_0_g194 = ( ( localTopLeft172_g194 + ( localLeft173_g194 * 2 ) + localBottomLeft174_g194 + -localTopRight177_g194 + ( localRight178_g194 * -2 ) + -localBottomRight179_g194 ) / 6.0 );
				float2 appendResult14_g200 = (float2(0.0 , localStepY164_g194));
				float4 tex2DNode16_g200 = tex2D( _MainTex, ( localCenter138_g194 + appendResult14_g200 ) );
				float temp_output_2_0_g200 = (tex2DNode16_g200).r;
				float temp_output_4_0_g200 = (tex2DNode16_g200).g;
				float temp_output_5_0_g200 = (tex2DNode16_g200).b;
				float localTop175_g194 = ( sqrt( ( ( ( temp_output_2_0_g200 * temp_output_2_0_g200 ) + ( temp_output_4_0_g200 * temp_output_4_0_g200 ) ) + ( temp_output_5_0_g200 * temp_output_5_0_g200 ) ) ) * _Intensity );
				float2 appendResult14_g199 = (float2(0.0 , localNegStepY165_g194));
				float4 tex2DNode16_g199 = tex2D( _MainTex, ( localCenter138_g194 + appendResult14_g199 ) );
				float temp_output_2_0_g199 = (tex2DNode16_g199).r;
				float temp_output_4_0_g199 = (tex2DNode16_g199).g;
				float temp_output_5_0_g199 = (tex2DNode16_g199).b;
				float localBottom176_g194 = ( sqrt( ( ( ( temp_output_2_0_g199 * temp_output_2_0_g199 ) + ( temp_output_4_0_g199 * temp_output_4_0_g199 ) ) + ( temp_output_5_0_g199 * temp_output_5_0_g199 ) ) ) * _Intensity );
				float temp_output_135_0_g194 = ( ( -localTopLeft172_g194 + ( localTop175_g194 * -2 ) + -localTopRight177_g194 + localBottomLeft174_g194 + ( localBottom176_g194 * 2 ) + localBottomRight179_g194 ) / 6.0 );
				float temp_output_111_0_g194 = sqrt( ( ( temp_output_133_0_g194 * temp_output_133_0_g194 ) + ( temp_output_135_0_g194 * temp_output_135_0_g194 ) ) );
				float3 appendResult113_g194 = (float3(temp_output_111_0_g194 , temp_output_111_0_g194 , temp_output_111_0_g194));
				float3 Sobel155 = ( appendResult113_g194 * 3.0 );
				float2 uv_MainTex = i.uv.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 GrabScreen111 = tex2D( _MainTex, uv_MainTex );
				float2 UV22_g3 = ase_screenPosNorm.xy;
				float2 localUnStereo22_g3 = UnStereo( UV22_g3 );
				float2 break64_g1 = localUnStereo22_g3;
				float4 tex2DNode36_g1 = tex2D( _CameraDepthTexture, ase_screenPosNorm.xy );
				#ifdef UNITY_REVERSED_Z
				float4 staticSwitch38_g1 = ( 1.0 - tex2DNode36_g1 );
				#else
				float4 staticSwitch38_g1 = tex2DNode36_g1;
				#endif
				float3 appendResult39_g1 = (float3(break64_g1.x , break64_g1.y , staticSwitch38_g1.r));
				float4 appendResult42_g1 = (float4((appendResult39_g1*2.0 + -1.0) , 1.0));
				float4 temp_output_43_0_g1 = mul( unity_CameraInvProjection, appendResult42_g1 );
				float4 appendResult49_g1 = (float4(( ( (temp_output_43_0_g1).xyz / (temp_output_43_0_g1).w ) * float3( 1,1,-1 ) ) , 1.0));
				float3 viewToObj10 = mul( unity_WorldToObject, mul( UNITY_MATRIX_I_V , float4( mul( unity_CameraToWorld, appendResult49_g1 ).xyz, 1 ) ) ).xyz;
				float clampResult100 = clamp( ( 1.0 - length( ( ( viewToObj10 + _WorldPosition ) * ( 1.0 - _Scale ) ) ) ) , 0.0 , 1.0 );
				float DepthLine22 = clampResult100;
				float DepthLine_Stepped139 = ( 1.0 - step( DepthLine22 , 0.0 ) );
				float4 lerpResult160 = lerp( float4( Sobel155 , 0.0 ) , GrabScreen111 , DepthLine_Stepped139);
				float4 SobelMask159 = lerpResult160;
				float grayscale122 = Luminance(GrabScreen111.rgb);
				float lerpResult134 = lerp( grayscale122 , 0.0 , DepthLine_Stepped139);
				float BlackAndWhite120 = lerpResult134;
				float4 lerpResult190 = lerp( ScreenDepth180 , SobelMask159 , BlackAndWhite120);
				float2 uv_ScanTexture = i.uv.xy * _ScanTexture_ST.xy + _ScanTexture_ST.zw;
				Gradient gradient131 = NewGradient( 0, 3, 2, float4( 0, 0, 0, 0 ), float4( 1, 1, 1, 0.001998932 ), float4( 0, 0, 0, 0.1936217 ), 0, 0, 0, 0, 0, float2( 1, 0 ), float2( 1, 1 ), 0, 0, 0, 0, 0, 0 );
				float4 Coloring25 = ( tex2D( _ScanTexture, uv_ScanTexture ) * SampleGradient( gradient131, DepthLine22 ) );
				float4 lerpResult167 = lerp( ( ( lerpResult190 + ( Coloring25 * _Power ) ) * ScreenDepth180 ) , GrabScreen111 , DepthLine_Stepped139);
				

				finalColor = ( lerpResult167 + Coloring25 );

				return finalColor;
			} 
			ENDCG 
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	Fallback "0"
}
/*ASEBEGIN
Version=18100
-1920;0;1920;1018;3050.942;-2781.035;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;23;-3209.598,-854.7692;Inherit;False;2260.443;804.4256;Comment;11;100;22;15;14;9;85;13;87;86;10;11;Depth Line;1,1,1,1;0;0
Node;AmplifyShaderEditor.FunctionNode;11;-3122.374,-709.7233;Inherit;False;Reconstruct World Position From Depth;0;;1;e7094bcbcc80eb140b2a3dbe6a861de8;0;0;1;FLOAT4;0
Node;AmplifyShaderEditor.TransformPositionNode;10;-2686.258,-653.3881;Inherit;False;View;Object;True;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;9;-2555,-294.2607;Inherit;False;Property;_Scale;Scale;21;0;Create;True;0;0;False;0;False;1;0.37;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;86;-2899.12,-404.7842;Inherit;False;Property;_WorldPosition;WorldPosition;23;0;Create;True;0;0;False;0;False;0,0,0;0,1,-10;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;85;-2357.861,-566.3276;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;87;-2277.89,-412.0367;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-2088.156,-559.4882;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LengthOpNode;14;-1880.17,-547.9576;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;15;-1679.285,-542.6262;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;157;-3146.324,2981.855;Inherit;False;1733.404;685.3833;Comment;10;155;149;172;147;146;145;144;154;153;193;Sobel;1,1,1,1;0;0
Node;AmplifyShaderEditor.ClampOpNode;100;-1444.998,-544.7311;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;154;-3084.479,3236.807;Float;False;Property;_Step;Step;25;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;153;-3096.324,3031.855;Inherit;False;0;0;_MainTex_TexelSize;Pass;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;143;-3161.763,122.9429;Inherit;False;1100.448;239.9829;Comment;4;138;140;142;139;Depth Line Stepped;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;22;-1180.429,-553.0663;Inherit;False;DepthLine;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;112;-3176.173,-1366.196;Inherit;False;931.6663;291.9866;Comment;3;79;80;111;Grab Screen;1,1,1,1;0;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;145;-2906.78,3365.135;Inherit;False;0;0;_MainTex;Shader;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;144;-2835.996,3158.589;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;138;-3111.763,173.394;Inherit;False;22;DepthLine;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;79;-3126.173,-1184.208;Inherit;False;0;0;_MainTex;Shader;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;146;-2631.964,3086.552;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.TextureCoordinatesNode;147;-2634.564,3455.753;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;80;-2879.635,-1316.195;Inherit;True;Property;_TextureSample0;Texture Sample 0;6;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;172;-2122.34,3345.929;Inherit;False;Constant;_Float6;Float 6;15;0;Create;True;0;0;False;0;False;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;149;-2310.264,3176.852;Inherit;False;SobelMain;2;;194;481788033fe47cd4893d0d4673016cbc;0;4;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT2;0,0;False;1;SAMPLER2D;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StepOpNode;140;-2744.174,229.926;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;111;-2487.507,-1225.484;Inherit;False;GrabScreen;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;176;-3172.471,-3480.24;Inherit;False;Property;_ScreenDepth;ScreenDepth;20;0;Create;True;0;0;False;0;False;50;35.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;193;-1992.942,3186.035;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;142;-2545.444,223.3461;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;28;-3202.89,555.408;Inherit;False;2225.317;880.8909;Comment;6;131;24;25;4;5;6;Coloring;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;123;-3178.278,1624.598;Inherit;False;1640.009;572.0996;Comment;6;120;122;114;133;135;134;Black And White Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.ScreenDepthNode;175;-3178.984,-3614.632;Inherit;False;1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GradientNode;131;-3103.256,788.507;Inherit;False;0;3;2;0,0,0,0;1,1,1,0.001998932;0,0,0,0.1936217;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;155;-1842.009,3178.984;Inherit;False;Sobel;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;183;-2854.941,-3599.6;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;114;-3128.278,1775.088;Inherit;True;111;GrabScreen;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;163;-3158.861,2363.315;Inherit;False;1121.459;406.894;Comment;5;160;161;158;162;159;SobelMask;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;24;-3098.105,1064.084;Inherit;False;22;DepthLine;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;139;-2340.314,172.943;Inherit;False;DepthLine_Stepped;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;158;-3073.914,2413.315;Inherit;False;155;Sobel;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;5;-2545.335,625.3723;Inherit;True;Property;_ScanTexture;ScanTexture;22;0;Create;True;0;0;False;0;False;-1;b2cff44be26e41a48aa375c23c83656a;b2cff44be26e41a48aa375c23c83656a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;161;-3096.861,2535.377;Inherit;False;111;GrabScreen;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GradientSampleNode;6;-2856.41,845.6931;Inherit;True;2;0;OBJECT;;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScaleAndOffsetNode;186;-2653.941,-3615.6;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCGrayscale;122;-2749.264,1753.166;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;135;-2819.753,2071.826;Inherit;False;139;DepthLine_Stepped;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;184;-2668.453,-3357.381;Inherit;False;Property;_SDepthColor;SDepthColor;19;0;Create;True;0;0;False;0;False;0,0,0,0;0.1273585,0.3333989,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;162;-3107.8,2655.209;Inherit;False;139;DepthLine_Stepped;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-2057.042,805.2789;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;160;-2645.44,2455.843;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;134;-2389.875,1871.826;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;185;-2320.941,-3600.6;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;25;-1636.014,739.0752;Inherit;False;Coloring;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;180;-2102.881,-3700.973;Inherit;False;ScreenDepth;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;120;-2115.637,1750.992;Inherit;False;BlackAndWhite;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;27;-3464.38,-2680.357;Inherit;False;2331.958;1200.657;Comment;17;166;167;0;169;181;170;165;168;130;132;164;171;106;129;190;191;192;Main;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;159;-2280.401,2515.671;Inherit;False;SobelMask;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;106;-3273.661,-2039.67;Inherit;False;Property;_Power;Power;24;0;Create;True;0;0;False;0;False;1;0.91;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;164;-3294.636,-2425.898;Inherit;False;159;SobelMask;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;129;-3252.579,-2159.031;Inherit;False;25;Coloring;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;192;-3398.988,-2313.317;Inherit;False;120;BlackAndWhite;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;191;-3290.019,-2512.843;Inherit;False;180;ScreenDepth;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;190;-3016.36,-2412.12;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;171;-2964.579,-2111.304;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;130;-2760.065,-2253.511;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;132;-2814.374,-1949.544;Inherit;False;180;ScreenDepth;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;166;-2593.248,-1887.212;Inherit;False;111;GrabScreen;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;168;-2600.855,-1692.755;Inherit;False;139;DepthLine_Stepped;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;165;-2527.582,-2142.73;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;167;-2277.377,-2082.071;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;170;-2285.563,-1769.721;Inherit;False;25;Coloring;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;181;-1803.221,-2149.686;Inherit;False;180;ScreenDepth;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;169;-1989.063,-2010.566;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;133;-2871.794,1885.469;Inherit;False;111;GrabScreen;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;-1557.866,-2022.999;Float;False;True;-1;2;ASEMaterialInspector;0;2;HardBit/MainPost;c71b220b631b6344493ea3cf87110c93;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;1;False;False;False;True;2;False;-1;False;False;True;2;False;-1;True;7;False;-1;False;True;0;False;0;False;False;False;False;False;False;False;False;False;False;True;0;0;0;0;0;Standard;0;0;1;True;False;;0
WireConnection;10;0;11;0
WireConnection;85;0;10;0
WireConnection;85;1;86;0
WireConnection;87;0;9;0
WireConnection;13;0;85;0
WireConnection;13;1;87;0
WireConnection;14;0;13;0
WireConnection;15;0;14;0
WireConnection;100;0;15;0
WireConnection;22;0;100;0
WireConnection;144;0;153;0
WireConnection;144;1;154;0
WireConnection;146;0;144;0
WireConnection;147;2;145;0
WireConnection;80;0;79;0
WireConnection;149;2;146;0
WireConnection;149;3;146;1
WireConnection;149;4;147;0
WireConnection;149;1;145;0
WireConnection;140;0;138;0
WireConnection;111;0;80;0
WireConnection;193;0;149;0
WireConnection;193;1;172;0
WireConnection;142;0;140;0
WireConnection;155;0;193;0
WireConnection;183;0;175;0
WireConnection;183;1;176;0
WireConnection;139;0;142;0
WireConnection;6;0;131;0
WireConnection;6;1;24;0
WireConnection;186;0;183;0
WireConnection;122;0;114;0
WireConnection;4;0;5;0
WireConnection;4;1;6;0
WireConnection;160;0;158;0
WireConnection;160;1;161;0
WireConnection;160;2;162;0
WireConnection;134;0;122;0
WireConnection;134;2;135;0
WireConnection;185;0;186;0
WireConnection;185;1;184;0
WireConnection;25;0;4;0
WireConnection;180;0;185;0
WireConnection;120;0;134;0
WireConnection;159;0;160;0
WireConnection;190;0;191;0
WireConnection;190;1;164;0
WireConnection;190;2;192;0
WireConnection;171;0;129;0
WireConnection;171;1;106;0
WireConnection;130;0;190;0
WireConnection;130;1;171;0
WireConnection;165;0;130;0
WireConnection;165;1;132;0
WireConnection;167;0;165;0
WireConnection;167;1;166;0
WireConnection;167;2;168;0
WireConnection;169;0;167;0
WireConnection;169;1;170;0
WireConnection;0;0;169;0
ASEEND*/
//CHKSM=CA549AC0A4952E5C04F535515BB9B5D5CBC008EE