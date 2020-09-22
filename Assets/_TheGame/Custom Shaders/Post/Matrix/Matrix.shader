// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Ala/Matrix"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		_Texture("_Texture", 2D) = "white" {}
		_Color("Color", Color) = (0,1,0.3174081,0)

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
				
			};

			uniform sampler2D _MainTex;
			uniform half4 _MainTex_TexelSize;
			uniform half4 _MainTex_ST;
			
			uniform sampler2D _Texture;
			uniform float4 _Color;


			v2f_img_custom vert_img_custom ( appdata_img_custom v  )
			{
				v2f_img_custom o;
				
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
				float2 uv06 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float4 GrabScreen85 = tex2D( _MainTex, uv06 );
				float2 uv041_g1 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float ScreenY4_g1 = _ScreenParams.y;
				float Y28_g1 = ( uv041_g1.y % ScreenY4_g1 );
				float2 uv040_g1 = i.uv.xy * float2( 16,1 ) + float2( 0,0 );
				float colmn11_g1 = ( uv040_g1.x - fmod( uv040_g1.x , 1.0 ) );
				float Rain_Offset23_g1 = sin( ( colmn11_g1 * 15.0 ) );
				float Rain_Speed21_g1 = ( ( cos( ( colmn11_g1 * 3.0 ) ) * 0.3 ) + 0.7 );
				float mulTime25_g1 = _Time.y * Rain_Speed21_g1;
				float S29_g1 = ( Rain_Offset23_g1 + mulTime25_g1 );
				float anim32_g1 = frac( ( Y28_g1 + S29_g1 ) );
				float Rain36_g1 = ( 1.0 / ( anim32_g1 * 50.0 ) );
				float mulTime3_g2 = _Time.y * 6.0;
				float2 uv012_g2 = i.uv.xy * ( float2( 16,16 ) * 0.0625 ) + ( ( float2( 0,0 ) + floor( mulTime3_g2 ) ) * 0.0625 );
				

				finalColor = ( GrabScreen85 + ( ( Rain36_g1 * tex2D( _Texture, uv012_g2 ).r ) * _Color ) );

				return finalColor;
			} 
			ENDCG 
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18100
-1920;0;1920;1018;1815.924;2610.651;2.423976;True;False
Node;AmplifyShaderEditor.CommentaryNode;84;248.5499,-2285.369;Inherit;False;1360.087;453.512;Comment;4;85;2;6;1;Grab Screen;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;323;370.8692,-1403.063;Inherit;False;1474.278;927.4663;Comment;8;320;318;321;322;19;0;330;331;Main;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;6;339.4933,-2013.641;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;1;332.1207,-2235.369;Inherit;True;0;0;_MainTex;Shader;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;330;410.697,-1047.546;Inherit;False;Fun_DigitalRain;-1;;1;9336ad0b9a52a2941b4d6672401c26c5;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;331;489.1974,-866.6731;Inherit;True;Fun_RandomTile;0;;2;aa535e8cd99e24049bb0aecda1e2b815;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;659.1461,-2170.723;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;320;814.4932,-682.5969;Inherit;False;Property;_Color;Color;2;0;Create;True;0;0;False;0;False;0,1,0.3174081,0;0,1,0.3174081,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;318;784.0746,-1031.311;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;85;1145.478,-2136.006;Inherit;True;GrabScreen;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;321;1056.493,-971.5967;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;322;937.0345,-1353.063;Inherit;True;85;GrabScreen;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;19;1259.937,-1300.315;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;1635.147,-1314.602;Float;False;True;-1;2;ASEMaterialInspector;0;2;Ala/Matrix;c71b220b631b6344493ea3cf87110c93;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;1;False;False;False;True;2;False;-1;False;False;True;2;False;-1;True;7;False;-1;False;True;0;False;0;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;;0
WireConnection;2;0;1;0
WireConnection;2;1;6;0
WireConnection;318;0;330;0
WireConnection;318;1;331;0
WireConnection;85;0;2;0
WireConnection;321;0;318;0
WireConnection;321;1;320;0
WireConnection;19;0;322;0
WireConnection;19;1;321;0
WireConnection;0;0;19;0
ASEEND*/
//CHKSM=B13B61825B0AEDD3C8C85A3B398AE7F14B04BDF6