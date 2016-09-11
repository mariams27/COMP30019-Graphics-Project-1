Shader "Unlit/TerrainShader"
{
	Properties
	{
		_AverageHeight("Average Height", Float) = 0.0
		_TerrainHeight("Terrain Height", Float) = 0.0
		_WaterLevel("Water Level", Float) = 0.0
	}

		SubShader
	{
		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

		float _AverageHeight;
		float _TerrainHeight;
		float _WaterLevel;

		struct vertIn
		{
			float4 vertex : POSITION;
			float4 normal : NORMAL;
			
		};

		struct vertOut
		{
			float4 vertex : SV_POSITION;
			float3 worldNormal : TEXCOORD1;
			float4 worldHeight : TEXCOORD0;
			float4 colour : COLOR;
		};

		// Implementation of the vertex shader
		vertOut vert(vertIn v)
		{
			vertOut o;
			// colour the high points white to represent snow
			if (v.vertex.y > (_AverageHeight * _TerrainHeight* 1.5)) {
				o.colour = float4(1.0f, 1.0f, 1.0f, 0.0f);
			} 
			// if a point is below water, colour it blue
			else if (v.vertex.y < (_AverageHeight * _TerrainHeight* _WaterLevel)) {
				o.colour = float4(0.0f, 0.5f, 1.0f, 0.0f);
			}

			// If it's slightly above sealevel, colour it green
			else if (v.vertex.y < (_AverageHeight * _TerrainHeight* _WaterLevel*1.25)) {
				o.colour = float4(0.0f, 0.51f, 0.0f, 0.0f);
			}

			// otherwise colour it brown
			else {
				o.colour = float4(0.4f, 0.2f, 0.0f, 0.0f);
			}
			o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
			o.worldHeight = mul(_Object2World, float4(1, 1, 1, 1));
			return o;
		}

		// Implementation of the fragment shader
		fixed4 frag(vertOut v) : SV_Target
		{
			return v.colour;
		}
			ENDCG
		}
	}
}
