Shader "Unlit/TerrainShader" {
	Properties{
		_MaxHeight("Max Height", Float) = 0
		_MinHeight("Min Height", Float) = 0

		_WaterLevel("Water Level", Float) = 0
		_GrassLevel("Grass Level", Float) = 0
		_SnowLevel("Snow Level", Float) = 0

		_SunColor("Sun Colour", Color) = (0,0,0)
		_SunPosition("Sun Pos", Vector) = (0.0, 0.0, 0.0)
	}

	SubShader {
		Pass {
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			float _MaxHeight;
			float _MinHeight;
			float _WaterLevel;
			float _GrassLevel;
			float _SnowLevel;

			uniform float3 _SunColour;
			uniform float3 _SunPosition;

			struct vertIn {
				float4 vertex : POSITION;
				float4 normal : NORMAL;
			};

			struct vertOut {
				float4 vertex : SV_POSITION;
				float4 colour : COLOR;
				float4 worldVertex : TEXCOORD0;
				float3 worldNormal : TEXCOORD1;
				float  spec : TEXCOORD2;
			};

			// Implementation of the vertex shader
			vertOut vert(vertIn v) {
				vertOut o;

				// Colour vertices based on height
				float h = v.vertex.y;
				if (h > _MaxHeight - (_MaxHeight - _MinHeight) * (1 - _SnowLevel)) {
					o.colour = float4(1.0f, 1.0f, 1.0f, 1.0f);  // white
					o.spec = 0.4;
				} else if (h < _MinHeight + (_MaxHeight - _MinHeight) * _WaterLevel) {
					o.colour = float4(0.0f, 0.5f, 1.0f, 1.0f);  // blue
					o.spec = 1;
				} else if (h < _MinHeight + (_MaxHeight - _MinHeight) * _GrassLevel) {
					o.colour = float4(0.0f, 0.51f, 0.0f, 1.0f); // green
					o.spec = 0;
				} else {
					o.colour = float4(0.4f, 0.2f, 0.0f, 1.0f);  // brown
					o.spec = 0;
				}

				// Convert Vertex position and corresponding normal into world coords
				o.worldVertex = mul(_Object2World, v.vertex);
				o.worldNormal = normalize(mul(transpose((float3x3)_World2Object), v.normal.xyz));

				// Transform vertex in world coordinates to camera coordinates
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
			
				return o;
			}

			// Implementation of the fragment shader
			fixed4 frag(vertOut v) : SV_Target {
				// Implementation of the phong illumination model

				float3 worldNormal = normalize(v.worldNormal);

				// Calculate ambient RGB intensities
				float Ka = 1;
				float3 amb = v.colour.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * Ka;

				// Calculate diffuse RBG reflections, we save the results of L.N because we will use it again
				// (when calculating the reflected ray in our specular component)
				float fAtt = 1;
				float Kd = 1;
				float3 L = normalize(_SunPosition - v.worldVertex.xyz);
				float LdotN = dot(L, worldNormal.xyz);
				float3 dif = fAtt * _SunColour.rgb * Kd * v.colour.rgb * saturate(LdotN);

				// Calculate specular reflections
				float Ks = v.spec;
				float specN = 5; // Values>>1 give tighter highlights
				float3 V = normalize(_WorldSpaceCameraPos - v.worldVertex.xyz);
				float3 H = (L + V) / length(L + V);
				float NdotH = dot(worldNormal, H);
				float3 R = (2 * LdotN * worldNormal) - L;
				float3 spe = fAtt * _SunColour.rgb * Ks * pow(saturate(NdotH), specN);

				// Combine Phong illumination model components
				v.colour.rgb = amb.rgb + dif.rgb + spe.rgb;
				v.colour.a = v.colour.a;

				return v.colour;
			}

			ENDCG
		}
	}
}
