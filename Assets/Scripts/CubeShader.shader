Shader "Unlit/CubeShader"
{
	Properties
	{
		_AverageHeight("Average Height", Float) = 0.0
		_TerrainHeight("Terrain Height", Float) = 0.0
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
			if (v.vertex.y < (_AverageHeight * 600*0.75)) {
				o.colour = float4(1.0f, 0.0f, 0.0f, 0.0f);
			}
			else {
				o.colour = float4(0.0f, 1.0f, 0.0f, 0.0f);
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
