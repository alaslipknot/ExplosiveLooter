// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "0_Tuto/MyXray"
{
	Properties
	{
		_ASEOutlineWidth( "Outline Width", Float ) = 0
		_MainColor("MainColor", Color) = (0,0,0,0)
		_XrayOutlineColor("XrayOutlineColor", Color) = (0,0,0,1)
		_XrayBias("XrayBias", Range( -1 , 1)) = 0
		_XrayScale("XrayScale", Range( 0 , 1)) = 0
		_XrayPower("XrayPower", Range( 0 , 5)) = 0
		_XrayOutlineIntensity("XrayOutlineIntensity", Range( 0 , 10)) = 0
		_InsideColor("InsideColor", Color) = (0.9716981,0.03208439,0.03208439,1)
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Pass
		{
			ColorMask 0
			ZWrite On
		}

		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0"}
		ZWrite Off
		ZTest Always
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Outline nofog alpha:fade  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc 
		
		
		
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};
		uniform float4 _XrayOutlineColor;
		uniform float _XrayBias;
		uniform float _XrayScale;
		uniform float _XrayPower;
		uniform float _XrayOutlineIntensity;
		uniform float4 _InsideColor;
		float _ASEOutlineWidth;
		
		void outlineVertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += ( v.normal * _ASEOutlineWidth );
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float fresnelNdotV7_g1 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode7_g1 = ( _XrayBias + _XrayScale * pow( 1.0 - fresnelNdotV7_g1, _XrayPower ) );
			float temp_output_10_0_g1 = ( fresnelNode7_g1 * 1.0 );
			float temp_output_13_0_g1 = ( 1.0 - temp_output_10_0_g1 );
			o.Emission = ( saturate( ( _XrayOutlineColor * temp_output_10_0_g1 * _XrayOutlineIntensity ) ) + ( temp_output_13_0_g1 * _InsideColor ) ).rgb;
			o.Alpha = ( temp_output_10_0_g1 + temp_output_13_0_g1 );
			o.Normal = float3(0,0,-1);
		}
		ENDCG
		

		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		ZWrite On
		ZTest LEqual
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			half filler;
		};

		uniform float4 _MainColor;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += 0;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = _MainColor.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18100
895;73;744;927;-2121.482;2014.258;1.633283;True;False
Node;AmplifyShaderEditor.ColorNode;1;2676.89,-1356.323;Inherit;False;Property;_MainColor;MainColor;0;0;Create;True;0;0;False;0;False;0,0,0,0;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;49;2692.491,-1084.782;Inherit;False;AL_Xray;1;;1;5bb6d0ab86033774ba2d4f09352cc1f8;0;0;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;3059.577,-1355.068;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;0_Tuto/MyXray;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;1;False;-1;0;False;-1;False;0;False;-1;0;False;-1;True;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;0;0;1;0
WireConnection;0;11;49;0
ASEEND*/
//CHKSM=F2891595B1D76026821584F0EE33CFB0211C00F2