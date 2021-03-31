﻿Shader "Unlit/GaussianBlur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_KernelSize("Kernel Size (N)", Int) = 21
		_Spread("St. dev. (sigma)", Float) = 3.0
	}

    CGINCLUDE
	#include "UnityCG.cginc"

	// Define the constants used in Gaussian calculation.
	static const float TWO_PI = 6.28319;
	static const float E = 2.71828;

	sampler2D _MainTex;
	float4 _MainTex_ST;
	float2 _MainTex_TexelSize;
	int	_KernelSize;
	float _Spread;

	// Two-dimensional Gaussian curve function.
	float gaussian(int x, int y)
	{
		float sigmaSq = _Spread * _Spread;
        return (1 / sqrt(TWO_PI * sigmaSq)) * pow(E, -((x * x) + (y * y)) / (2 * sigmaSq));
	}

	ENDCG

    SubShader
    {
        Tags 
		{ 
			"RenderType" = "Opaque"
		}

        Pass
        {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			float4 frag(v2f_img i) : SV_Target
			{
				float3 col = float3(0.0, 0.0, 0.0);
				float kernelSum = 0.0;

				int upper = ((_KernelSize - 1) / 2);
				int lower = -upper;

				for (int x = lower; x <= upper; ++x)
				{
					for (int y = lower; y <= upper; ++y)
					{
						float gauss = gaussian(x, y);
						kernelSum += gauss;

						fixed2 offset = fixed2(_MainTex_TexelSize.x * x, _MainTex_TexelSize.y * y);
						col += gauss * tex2D(_MainTex, i.uv + offset);
					}
				}

				col /= kernelSum;
				return float4(col, 1.0);
			}
			ENDCG
        }
    }
}
