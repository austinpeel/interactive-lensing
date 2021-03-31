Shader "Hidden/Starlet"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Scale("Scale", Int) = 1
    }

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

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float2 _MainTex_TexelSize;
            float _Scale;

            float starlet1d(float r, float rs)
            {
                float y = r / rs;
                float A = 1.0 / 9.0 / rs / rs;
                float term1 = 93.0 * pow(abs(y), 3) - 64.0 * pow(abs(0.5 - y), 3) + pow(abs(0.5 + y), 3);
                float term2 = 18.0 * pow(abs(1.0 - y), 3) + pow(abs(1.0 + y), 3);
                float term3 = 0.5 * pow(abs(2.0 - y), 3) + pow(abs(2.0 + y), 3);
                return A * (term1 + term2 - term3);
            }

            float starlet2d(int x, int y)
            {
                float radius = sqrt(x * x + y * y);
                float xs = pow(2, _Scale + 1);
                return starlet1d(radius, xs);
            }

			float4 frag(v2f_img i) : SV_Target
			{
				float3 col = float3(0.0, 0.0, 0.0);
				float kernelSum = 0.0;
                
                int kernelSize = pow(2, 2 + _Scale) + 1;
				int upper = ((kernelSize - 1) / 2);
				int lower = -upper;

				for (int x = lower; x <= upper; ++x)
				{
					for (int y = lower; y <= upper; ++y)
					{
						float starlet = starlet2d(x, y);
						kernelSum += starlet;

						fixed2 offset = fixed2(_MainTex_TexelSize.x * x, _MainTex_TexelSize.y * y);
						col += starlet * tex2D(_MainTex, i.uv + offset);
					}
				}

				col /= kernelSum;
				return float4(col, 1.0);
			}
			ENDCG
        }
    }
}