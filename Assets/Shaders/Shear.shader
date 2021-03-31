Shader "Unlit/Shear"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _E1 ("E1", Float) = 0
        _E2 ("E2", Float) = 0
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
            float _E1;
            float _E2;
            static const float PI = 4 * atan(1);

            // float2 e2phiq(float e1, float e2)
            // {
            //     float phi = atan2(e2, e1) / 2;
            //     float c = min(sqrt(pow(e1, 2) + pow(e2, 2)), 0.9999);
            //     // float c = min(distance(e1, e2), 0.9999);
            //     float q = (1 - c) / (1 + c);
            //     return float2(phi, q);
            // }

            // float atanh(float x)
            // {
            //     float result = 0;
            //     if (abs(x) < 1)
            //     {
            //         result = 0.5 * (log(1 + x) - log(1 - x));
            //     }
            //     return result;
            //     // return 0.5 * (log(1 + x) - log(1 - x));
            // }

            float2 rotate(float2 pos, float angle)
            {
                float newX =  pos.x * cos(angle) + pos.y * sin(angle);
                float newY = -pos.x * sin(angle) + pos.y * cos(angle);
                return float2(newX, newY);
            }

			float4 frag(v2f_img i) : SV_Target
			{
                // Screen space (pixel) position
                float2 pos = (i.uv - 0.5) / _MainTex_TexelSize.xy;
                
                // Rotate
                // float angle = atan2(_E2, _E1) / 2;
                // float2 newPos = rotate(pos, angle);

                // Shear
                float2 newPos = float2((1 - _E1) * pos.x - _E2 * pos.y, -_E2 * pos.x + (1 + _E1) * pos.y);

                float3 color = tex2D(_MainTex, newPos * _MainTex_TexelSize + 0.5);
                return float4(color, 1.0);

                // float r = sqrt(pow(pos.x, 2) + pow(pos.y, 2));
                // if (abs(pos.y - 1 / _MainTex_TexelSize.y / 2) > 20)
                // {
                //     pos -= 10 * pos / r;
                // }
                // float3 color = tex2D(_MainTex, pos * _MainTex_TexelSize.xy + 0.5);
                // return float4(color, 1.0);
			}

            // Proper SIE
            // float4 frag(v2f_img i) : SV_Target
			// {
            //     // Parameter conversions
            //     float2 phiq = e2phiq(_E1, _E2);
            //     float q2 = pow(phiq.y, 2);
            //     float thetaEeff = _ThetaE / sqrt((1 + q2) / (2 * phiq.y));
            //     float b = thetaEeff * sqrt(0.5 * (1 + q2));
            //     float s = 0.001 * sqrt((1 + q2) / (2 * q2));

            //     // Texture space (pixel) position
            //     float2 pos = (i.uv - 0.5) / _MainTex_TexelSize.xy;
                
            //     // Rotate
            //     pos = rotate(pos, phiq.x);
                
            //     float psi = sqrt(q2 * (pow(s, 2) + pow(pos.x, 2)) + pow(pos.y, 2));
            //     float p = sqrt(1 - q2);
            //     float alphaX = b / p * atan2(p * pos.x, psi + s);
            //     float alphaY = b / p * atanh(p * pos.y / (psi + q2 * s));
            //     // Rotate back
            //     float2 alpha = rotate(float2(alphaX, alphaY), -phiq.x);
                
            //     // Deflection angle back in normalized UV space
			// 	float3 color = tex2D(_MainTex, i.uv - alpha * _MainTex_TexelSize.xy);
			// 	return float4(color, 1.0);
			// }
			ENDCG
        }
    }
}
