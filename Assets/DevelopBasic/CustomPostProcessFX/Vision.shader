// Made with Amplify Shader Editor v1.9.2.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Hidden/Custom/Vision"
{
	Properties
	{
		_Blend("Blend", Range( 0 , 1)) = 0
		_MaskTex("MaskTex", 2D) = "white" {}
		_Screen_1("Screen_1", 2D) = "white" {}
		_Screen_2("Screen_2", 2D) = "white" {}
		_Screen_3("Screen_3", 2D) = "white" {}
		_Screen_0("Screen_0", 2D) = "black" {}

	}

	SubShader
	{
		LOD 0

		Cull Off
		ZWrite Off
		ZTest Always
		
		Pass
		{
			CGPROGRAM

			

			#pragma vertex Vert
			#pragma fragment Frag
			#pragma target 3.0

			#include "UnityCG.cginc"
			#define ASE_NEEDS_FRAG_SCREEN_POSITION_NORMALIZED

		
			struct ASEAttributesDefault
			{
				float3 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				
			};

			struct ASEVaryingsDefault
			{
				float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				float2 texcoordStereo : TEXCOORD1;
			#if STEREO_INSTANCING_ENABLED
				uint stereoTargetEyeIndex : SV_RenderTargetArrayIndex;
			#endif
				
			};

			uniform sampler2D _MainTex;
			uniform half4 _MainTex_TexelSize;
			uniform half4 _MainTex_ST;
			
			uniform sampler2D _Screen_1;
			uniform sampler2D _MaskTex;
			uniform float RedWeight;
			uniform float GreenWeight;
			uniform float BlueWeight;
			uniform sampler2D _Screen_2;
			uniform sampler2D _Screen_3;
			uniform float _Blend;
			uniform sampler2D _Screen_0;


			
			float2 TransformTriangleVertexToUV (float2 vertex)
			{
				float2 uv = (vertex + 1.0) * 0.5;
				return uv;
			}

			ASEVaryingsDefault Vert( ASEAttributesDefault v  )
			{
				ASEVaryingsDefault o;
				o.vertex = float4(v.vertex.xy, 0.0, 1.0);
				o.texcoord = TransformTriangleVertexToUV (v.vertex.xy);
#if UNITY_UV_STARTS_AT_TOP
				o.texcoord = o.texcoord * float2(1.0, -1.0) + float2(0.0, 1.0);
#endif
				o.texcoordStereo = TransformStereoScreenSpaceTex (o.texcoord, 1.0);

				v.texcoord = o.texcoordStereo;
				float4 ase_ppsScreenPosVertexNorm = float4(o.texcoordStereo,0,1);

				

				return o;
			}

			float4 Frag (ASEVaryingsDefault i  ) : SV_Target
			{
				float4 ase_ppsScreenPosFragNorm = float4(i.texcoordStereo,0,1);

				float2 appendResult21 = (float2(ase_ppsScreenPosFragNorm.x , ase_ppsScreenPosFragNorm.y));
				float2 screenUV23 = ( appendResult21 / ase_ppsScreenPosFragNorm.w );
				float4 tex2DNode12 = tex2D( _MainTex, screenUV23 );
				float4 tex2DNode15 = tex2D( _MaskTex, screenUV23 );
				float3 appendResult73 = (float3(RedWeight , GreenWeight , BlueWeight));
				float3 sinMask74 = appendResult73;
				float4 break49 = ( tex2DNode15 * float4( sinMask74 , 0.0 ) );
				float4 lerpResult51 = lerp( tex2DNode12 , tex2D( _Screen_1, screenUV23 ) , break49.r);
				float4 lerpResult57 = lerp( lerpResult51 , tex2D( _Screen_2, screenUV23 ) , break49.g);
				float4 lerpResult58 = lerp( lerpResult57 , tex2D( _Screen_3, screenUV23 ) , break49.b);
				float4 lerpResult14 = lerp( tex2DNode12 , lerpResult58 , _Blend);
				float3 blendOpSrc102 = (lerpResult14).rgb;
				float3 blendOpDest102 = (lerpResult14).rgb;
				float3 lerpBlendMode102 = lerp(blendOpDest102,(( blendOpDest102 > 0.5 ) ? ( 1.0 - 2.0 * ( 1.0 - blendOpDest102 ) * ( 1.0 - blendOpSrc102 ) ) : ( 2.0 * blendOpDest102 * blendOpSrc102 ) ),( saturate( ( (tex2DNode15).r * 7 ) ) * tex2D( _Screen_0, screenUV23 ).r ));
				float4 appendResult99 = (float4(( saturate( lerpBlendMode102 )) , (lerpResult14).a));
				

				float4 color = appendResult99;
				
				return color;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	Fallback Off
}
/*ASEBEGIN
Version=19201
Node;AmplifyShaderEditor.CommentaryNode;33;-3128.272,-489.9786;Inherit;False;810.0308;259.71;Comment;4;20;21;22;23;ScreenUV;1,1,1,1;0;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;20;-3078.272,-439.9786;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;21;-2901.271,-416.9787;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;22;-2680.083,-367.2686;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;23;-2560.242,-367.7753;Inherit;False;screenUV;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;26;-1989.344,703.4495;Inherit;True;Property;_Screen_2;Screen_2;3;0;Create;True;0;0;0;False;0;False;-1;cf5516e9cfb95b249bc055979e311e33;cf5516e9cfb95b249bc055979e311e33;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;27;-1963.344,912.7488;Inherit;True;Property;_Screen_3;Screen_3;4;0;Create;True;0;0;0;False;0;False;-1;6df0d635a7af4154785b20973f1c4831;6df0d635a7af4154785b20973f1c4831;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;12;-1870.249,-153.4745;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;24;-2072.074,-84.01248;Inherit;False;23;screenUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;14;-849.7629,-149.3406;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;3;-2038.126,-209.4417;Inherit;False;0;0;_MainTex;Shader;False;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;13;-1148.392,19.81129;Inherit;False;Property;_Blend;Blend;0;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;19;-1999.814,493.9189;Inherit;True;Property;_Screen_1;Screen_1;2;0;Create;True;0;0;0;False;0;False;-1;c64cb11f3eee13d45b6d30075802d614;c64cb11f3eee13d45b6d30075802d614;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;51;-1406.748,473.2845;Inherit;False;3;0;COLOR;0,0,0,1;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;58;-986.6702,895.5588;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;57;-1184.069,687.2947;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;31;-2280.808,936.7587;Inherit;False;23;screenUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;30;-2296.024,727.3566;Inherit;False;23;screenUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SinOpNode;71;-1829.891,2214.674;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;86;-2662.958,1464.89;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;87;-2662.957,2161.128;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;65;-2244.34,2263.236;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;67;-2516.519,2262.388;Inherit;False;Property;_OffsetBlue;OffsetBlue;8;0;Create;True;0;0;0;False;0;False;1.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;72;-2028.383,2215.524;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;59;-1826.303,1319.94;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;69;-2059.272,1315.833;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;62;-2456.371,1363.366;Inherit;False;Property;_OffsetRed;OffsetRed;6;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;63;-2297.406,1363.604;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;25;-2912.82,48.16082;Inherit;False;23;screenUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;15;-2699.917,24.52123;Inherit;True;Property;_MaskTex;MaskTex;1;0;Create;True;0;0;0;False;0;False;-1;2020b1e062cf19043801e2cfa744fa7d;2020b1e062cf19043801e2cfa744fa7d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;77;-2592.206,232.3627;Inherit;False;74;sinMask;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleTimeNode;60;-2897.783,1748.991;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;61;-3184.783,1744.991;Inherit;False;Property;_Speed;Speed;5;0;Create;True;0;0;0;False;0;False;0.25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;78;-2388.663,126.4031;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;80;-1836.414,1563.085;Inherit;False;Global;RedWeight;RedWeight;9;0;Create;True;0;0;0;False;0;False;1;0.05950902;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;82;-1836.14,2444.271;Inherit;False;Global;BlueWeight;BlueWeight;11;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;81;-1852.862,1986.802;Inherit;False;Global;GreenWeight;GreenWeight;10;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;79;-891.1508,1815.712;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0.25,0.25,0.25;False;2;FLOAT3;1,1,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;74;-1142.695,1687.527;Inherit;False;sinMask;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;73;-1383.386,1818.102;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;76;-1139.173,1816.494;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT;0.5;False;2;FLOAT;0.5;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PiNode;64;-2297.035,1809.109;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-2576.755,1807.5;Inherit;False;Property;_OffsetGreen;OffsetGreen;7;0;Create;True;0;0;0;False;0;False;0.6;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;70;-2043.681,1757.289;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;68;-1811.841,1757.968;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;29;-2316.412,517.3594;Inherit;False;23;screenUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;6;381.959,-169.3215;Float;False;True;-1;2;ASEMaterialInspector;0;8;Hidden/Custom/Vision;32139be9c1eb75640a847f011acf3bcf;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;True;7;False;;False;False;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
Node;AmplifyShaderEditor.WireNode;100;-635.215,81.20137;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;97;-275.0007,259.2791;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;89;-579.6832,313.0835;Inherit;True;Property;_Screen_0;Screen_0;9;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;88;-770.4771,337.1004;Inherit;False;23;screenUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;98;-36.58076,107.5167;Inherit;False;False;False;False;True;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;99;177.0708,-167.4418;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ComponentMaskNode;96;-579.0186,-94.9258;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;93;-566.9518,-209.7442;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BreakToComponentsNode;49;-2178.855,120.0816;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.ScaleNode;104;-824.36,167.0888;Inherit;False;7;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;105;-654.9987,164.3296;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;102;-68.6607,-169.1759;Inherit;False;Overlay;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;106;-2374.16,-34.7897;Inherit;False;True;False;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
WireConnection;21;0;20;1
WireConnection;21;1;20;2
WireConnection;22;0;21;0
WireConnection;22;1;20;4
WireConnection;23;0;22;0
WireConnection;26;1;30;0
WireConnection;27;1;31;0
WireConnection;12;0;3;0
WireConnection;12;1;24;0
WireConnection;14;0;12;0
WireConnection;14;1;58;0
WireConnection;14;2;13;0
WireConnection;19;1;29;0
WireConnection;51;0;12;0
WireConnection;51;1;19;0
WireConnection;51;2;49;0
WireConnection;58;0;57;0
WireConnection;58;1;27;0
WireConnection;58;2;49;2
WireConnection;57;0;51;0
WireConnection;57;1;26;0
WireConnection;57;2;49;1
WireConnection;71;0;72;0
WireConnection;86;0;60;0
WireConnection;87;0;60;0
WireConnection;65;0;67;0
WireConnection;72;0;87;0
WireConnection;72;1;65;0
WireConnection;59;0;69;0
WireConnection;69;0;86;0
WireConnection;69;1;63;0
WireConnection;63;0;62;0
WireConnection;15;1;25;0
WireConnection;60;0;61;0
WireConnection;78;0;15;0
WireConnection;78;1;77;0
WireConnection;79;0;76;0
WireConnection;74;0;73;0
WireConnection;73;0;80;0
WireConnection;73;1;81;0
WireConnection;73;2;82;0
WireConnection;76;0;73;0
WireConnection;64;0;66;0
WireConnection;70;0;60;0
WireConnection;70;1;64;0
WireConnection;68;0;70;0
WireConnection;6;0;99;0
WireConnection;100;0;14;0
WireConnection;97;0;105;0
WireConnection;97;1;89;1
WireConnection;89;1;88;0
WireConnection;98;0;100;0
WireConnection;99;0;102;0
WireConnection;99;3;98;0
WireConnection;96;0;14;0
WireConnection;93;0;14;0
WireConnection;49;0;78;0
WireConnection;104;0;106;0
WireConnection;105;0;104;0
WireConnection;102;0;93;0
WireConnection;102;1;96;0
WireConnection;102;2;97;0
WireConnection;106;0;15;0
ASEEND*/
//CHKSM=40E1585ADA07415474C99B36E01C125465EC99A7