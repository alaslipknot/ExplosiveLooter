// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "HardBit/DepthTest"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		[HDR]_EffectColor("EffectColor", Color) = (4,4,4,0)
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_DepthStep("DepthStep", Float) = 1
		_Depth("Depth", Range( 0 , 100)) = 1
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
			#pragma target 3.0
			#include "UnityCG.cginc"
			

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
			
			uniform sampler2D _TextureSample1;
			uniform float4 _TextureSample1_ST;
			uniform float4 _EffectColor;
			UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
			uniform float4 _CameraDepthTexture_TexelSize;
			uniform float _Depth;
			uniform float _DepthStep;


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
				float2 uv_TextureSample1 = i.uv.xy * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
				float2 uv_MainTex = i.uv.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 ScreenImage17 = tex2D( _MainTex, uv_MainTex );
				float4 screenPos = i.ase_texcoord4;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float clampDepth12 = Linear01Depth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
				float temp_output_15_0 = (1.0 + (( clampDepth12 * _Depth ) - 0.0) * (0.0 - 1.0) / (1.0 - 0.0));
				float DepthVar25 = step( temp_output_15_0 , _DepthStep );
				float4 lerpResult45 = lerp( ( ( tex2D( _TextureSample1, uv_TextureSample1 ) * _EffectColor ) + ScreenImage17 ) , ScreenImage17 , DepthVar25);
				

				finalColor = lerpResult45;

				return finalColor;
			} 
			ENDCG 
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18400
-1920;0;1319;1018;-369.3611;2761.317;2.763588;True;False
Node;AmplifyShaderEditor.ScreenPosInputsNode;24;131.5521,-1050.347;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;13;342.1017,-852.4489;Inherit;False;Property;_Depth;Depth;24;0;Create;True;0;0;False;0;False;1;0;0;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;1;-683.4932,43.62003;Inherit;False;0;0;_MainTex;Shader;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenDepthNode;12;461.4131,-1091.519;Inherit;False;1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;673.2386,-975.1141;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-207,137.5;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;15;844.9183,-991.4882;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;17;143.7183,202.2438;Inherit;False;ScreenImage;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;44;1414.697,-1369.835;Inherit;False;Property;_EffectColor;EffectColor;20;1;[HDR];Create;True;0;0;False;0;False;4,4,4,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;37;1402.717,-1634.49;Inherit;True;Property;_TextureSample1;Texture Sample 1;21;0;Create;True;0;0;False;0;False;-1;4e3951c538fc8a647a4a10a99b480987;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;33;1101.385,-954.1227;Inherit;False;Property;_DepthStep;DepthStep;22;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;42;1705.855,-1304.504;Inherit;False;17;ScreenImage;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;32;1181.56,-1127.846;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;1728.026,-1492.592;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;25;1712.531,-1160.376;Inherit;False;DepthVar;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;46;1985.466,-1490.37;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;1511.971,-378.6118;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;29;1385.333,-622.1347;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-683,-158.5;Inherit;False;Property;_Step;Step;26;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;26;1333.815,278.201;Inherit;False;25;DepthVar;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;48;1462.474,-2005.839;Float;False;1;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;52;1861.247,-1927.576;Inherit;False;True;True;True;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;3;-718,-365.5;Inherit;False;0;0;_MainTex_TexelSize;Pass;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;7;231.8876,-324.5283;Inherit;False;SobelMain;3;;1;481788033fe47cd4893d0d4673016cbc;0;4;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT2;0,0;False;1;SAMPLER2D;0.0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;18;1162.452,102.4984;Inherit;False;17;ScreenImage;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;6;-154.3704,-337.9747;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.GetLocalVarNode;27;1200.397,-408.3683;Inherit;False;25;DepthVar;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;9;502.8087,-528.5168;Inherit;False;Property;_Color0;Color 0;25;1;[HDR];Create;True;0;0;False;0;False;1,0.6831585,0,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-404,-311.5;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LerpOp;20;1649.499,84.72161;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;30;1050.731,-516.3283;Inherit;False;Property;_Offset;Offset;23;0;Create;True;0;0;False;0;False;5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TriplanarNode;49;2333.674,-1770.176;Inherit;True;Cylindrical;World;False;Top Texture 0;_TopTexture0;white;2;None;Mid Texture 0;_MidTexture0;gray;0;None;Bot Texture 0;_BotTexture0;bump;1;None;Triplanar Sampler;Tangent;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT3;1,1,1;False;3;FLOAT2;1,1;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;45;2255.552,-1410.245;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;8;-159.1913,-10.51685;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;19;1790.495,-194.3127;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;1116.809,-321.5168;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;2977.727,-1583.127;Float;False;True;-1;2;ASEMaterialInspector;0;2;HardBit/DepthTest;c71b220b631b6344493ea3cf87110c93;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;1;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;True;2;False;-1;True;7;False;-1;False;True;0;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;;True;0
WireConnection;12;0;24;0
WireConnection;14;0;12;0
WireConnection;14;1;13;0
WireConnection;2;0;1;0
WireConnection;15;0;14;0
WireConnection;17;0;2;0
WireConnection;32;0;15;0
WireConnection;32;1;33;0
WireConnection;43;0;37;0
WireConnection;43;1;44;0
WireConnection;25;0;32;0
WireConnection;46;0;43;0
WireConnection;46;1;42;0
WireConnection;16;0;27;0
WireConnection;16;1;10;0
WireConnection;29;0;15;0
WireConnection;29;1;30;0
WireConnection;52;0;48;0
WireConnection;7;2;6;0
WireConnection;7;3;6;1
WireConnection;7;4;8;0
WireConnection;7;1;1;0
WireConnection;6;0;5;0
WireConnection;5;0;3;0
WireConnection;5;1;4;0
WireConnection;20;0;10;0
WireConnection;20;1;18;0
WireConnection;20;2;26;0
WireConnection;45;0;46;0
WireConnection;45;1;42;0
WireConnection;45;2;25;0
WireConnection;8;2;1;0
WireConnection;19;0;16;0
WireConnection;19;1;18;0
WireConnection;10;0;9;0
WireConnection;10;1;7;0
WireConnection;0;0;45;0
ASEEND*/
//CHKSM=DD28EA29AF9BC89B08ED40786CBB108DD6214AD7