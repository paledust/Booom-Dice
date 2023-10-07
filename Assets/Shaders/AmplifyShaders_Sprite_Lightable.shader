// Made with Amplify Shader Editor v1.9.2.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "AmplifyShaders/Sprite_Lightable"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		[Toggle(_TEXTUREAFFECTLIGHT_ON)] _TextureAffectLight("TextureAffectLight", Float) = 1
		_Y_MakeUp("Y_MakeUp", Float) = 1
		_FarDistance("FarDistance", Float) = 5
		_CloseDistance("CloseDistance", Float) = 1
		_FadeHeight("FadeHeight", Float) = 0
		_LightMultiplier("LightMultiplier", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		LOD 0

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		
		
		Pass
		{
		CGPROGRAM
			
			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"
			#define ASE_NEEDS_FRAG_COLOR
			#pragma shader_feature_local _TEXTUREAFFECTLIGHT_ON


			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord1 : TEXCOORD1;
			};
			
			uniform fixed4 _Color;
			uniform float _EnableExternalAlpha;
			uniform sampler2D _MainTex;
			uniform sampler2D _AlphaTex;
			uniform float4 _MainTex_ST;
			uniform float4 SpriteLightColor;
			uniform float SpriteLightIntensity;
			uniform float _LightMultiplier;
			uniform float3 SpriteLightPos;
			uniform float _Y_MakeUp;
			uniform float SpriteLightRadius;
			uniform float SpriteLightLength;
			uniform float _FadeHeight;
			uniform float _CloseDistance;
			uniform float _FarDistance;

			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				float3 ase_worldPos = mul(unity_ObjectToWorld, float4( (IN.vertex).xyz, 1 )).xyz;
				OUT.ase_texcoord1.xyz = ase_worldPos;
				
				
				//setting value to unused interpolator channels and avoid initialization warnings
				OUT.ase_texcoord1.w = 0;
				
				IN.vertex.xyz +=  float3(0,0,0) ; 
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				fixed4 alpha = tex2D (_AlphaTex, uv);
				color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}
			
			fixed4 frag(v2f IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float2 uv_MainTex = IN.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 tex2DNode4 = tex2D( _MainTex, uv_MainTex );
				float4 temp_output_7_0 = ( IN.color * tex2DNode4 );
				float3 ase_worldPos = IN.ase_texcoord1.xyz;
				float3 appendResult33 = (float3(1.0 , _Y_MakeUp , 1.0));
				float4 lighting22 = ( ( SpriteLightColor * SpriteLightIntensity * _LightMultiplier ) * saturate( -( ( length( ( ( ase_worldPos - SpriteLightPos ) * appendResult33 ) ) - ( SpriteLightRadius - SpriteLightLength ) ) / SpriteLightLength ) ) );
				#ifdef _TEXTUREAFFECTLIGHT_ON
				float4 staticSwitch25 = ( tex2DNode4 * lighting22 );
				#else
				float4 staticSwitch25 = lighting22;
				#endif
				float temp_output_37_0 = ( _FadeHeight - ase_worldPos.y );
				float Fade54 = ( saturate( ( temp_output_37_0 / _CloseDistance ) ) * saturate( ( ( _FarDistance - temp_output_37_0 ) / _CloseDistance ) ) );
				float4 appendResult28 = (float4(( temp_output_7_0 + staticSwitch25 ).rgb , ( (temp_output_7_0).a * Fade54 )));
				
				fixed4 c = appendResult28;
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	Fallback Off
}
/*ASEBEGIN
Version=19201
Node;AmplifyShaderEditor.SamplerNode;4;-1162.988,-21.81622;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;6;-1064.016,-200.0646;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-832.0157,-108.0646;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;11;-1344.989,901.6567;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;15;-1133.587,1108.033;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;14;-1502.678,1066.039;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;16;-901.932,1109.129;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;2;-1362.489,-20.31625;Inherit;False;0;0;_MainTex;Shader;False;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;23;-1173.466,237.175;Inherit;False;22;lighting;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-960.6636,239.3365;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;25;-790.6636,236.3365;Inherit;False;Property;_TextureAffectLight;TextureAffectLight;0;0;Create;True;0;0;0;False;0;False;0;1;1;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;27;-741.4944,1109.696;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;24;-473.1466,78.64303;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-1753.078,1045.839;Inherit;False;Global;SpriteLightRadius;SpriteLightRadius;0;0;Create;True;0;0;0;False;0;False;0;1.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-1733.078,1129.84;Inherit;False;Global;SpriteLightLength;SpriteLightLength;0;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LengthOpNode;10;-1502.062,900.3814;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;8;-2236.688,746.197;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldPosInputsNode;5;-2477.318,611.8496;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector3Node;9;-2502.741,808.5004;Inherit;False;Global;SpriteLightPos;SpriteLightPos;0;0;Create;True;0;0;0;False;0;False;0,0,0;-100.134,-1,-35.835;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-1992.367,904.8203;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;33;-2316.958,1088.439;Inherit;False;FLOAT3;4;0;FLOAT;1;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-2478.723,1110.948;Inherit;False;Property;_Y_MakeUp;Y_MakeUp;1;0;Create;True;0;0;0;False;0;False;1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;37;-2243.986,1466.007;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;41;-1975.094,1467.315;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-2222.466,1629.946;Inherit;False;Property;_CloseDistance;CloseDistance;3;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-1541.694,1468.214;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-522.2995,1084.446;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-2338.298,1906.179;Inherit;False;Property;_FarDistance;FarDistance;2;0;Create;True;0;0;0;False;0;False;5;30;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;45;-2110.49,1910.555;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;46;-1924.425,1913.121;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;52;-1677.614,1958.809;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;53;-1754,1468.885;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;22;260.0378,1091.891;Inherit;False;lighting;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;54;-1337.033,1467.399;Inherit;False;Fade;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;55;-476.0254,353.1637;Inherit;False;54;Fade;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;29;-478.1174,229.7572;Inherit;False;False;False;False;True;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-264.0254,284.1637;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;28;-51.21759,76.45716;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;130.9874,75.54056;Float;False;True;-1;2;ASEMaterialInspector;0;10;AmplifyShaders/Sprite_Lightable;0f8ba0101102bb14ebf021ddadce9b49;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;False;True;3;1;False;;10;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
Node;AmplifyShaderEditor.WorldPosInputsNode;36;-2490.704,1516.089;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;57;-2488.36,1400.509;Inherit;False;Property;_FadeHeight;FadeHeight;4;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;58;-987.0464,827.4006;Inherit;False;Property;_LightMultiplier;LightMultiplier;5;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-1020.821,751.0532;Inherit;False;Global;SpriteLightIntensity;SpriteLightIntensity;0;0;Create;True;0;0;0;False;0;False;1;200;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;17;-1021.345,577.967;Inherit;False;Global;SpriteLightColor;SpriteLightColor;0;0;Create;True;0;0;0;False;0;False;1,1,1,1;1,0.5445026,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-755.174,701.9753;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
WireConnection;4;0;2;0
WireConnection;7;0;6;0
WireConnection;7;1;4;0
WireConnection;11;0;10;0
WireConnection;11;1;14;0
WireConnection;15;0;11;0
WireConnection;15;1;13;0
WireConnection;14;0;12;0
WireConnection;14;1;13;0
WireConnection;16;0;15;0
WireConnection;26;0;4;0
WireConnection;26;1;23;0
WireConnection;25;1;23;0
WireConnection;25;0;26;0
WireConnection;27;0;16;0
WireConnection;24;0;7;0
WireConnection;24;1;25;0
WireConnection;10;0;30;0
WireConnection;8;0;5;0
WireConnection;8;1;9;0
WireConnection;30;0;8;0
WireConnection;30;1;33;0
WireConnection;33;1;32;0
WireConnection;37;0;57;0
WireConnection;37;1;36;2
WireConnection;41;0;37;0
WireConnection;41;1;40;0
WireConnection;43;0;53;0
WireConnection;43;1;52;0
WireConnection;21;0;19;0
WireConnection;21;1;27;0
WireConnection;45;0;39;0
WireConnection;45;1;37;0
WireConnection;46;0;45;0
WireConnection;46;1;40;0
WireConnection;52;0;46;0
WireConnection;53;0;41;0
WireConnection;22;0;21;0
WireConnection;54;0;43;0
WireConnection;29;0;7;0
WireConnection;56;0;29;0
WireConnection;56;1;55;0
WireConnection;28;0;24;0
WireConnection;28;3;56;0
WireConnection;1;0;28;0
WireConnection;19;0;17;0
WireConnection;19;1;18;0
WireConnection;19;2;58;0
ASEEND*/
//CHKSM=68182ED41D7DE08FA730963DB0E2A0F8B32EE837