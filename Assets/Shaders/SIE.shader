Shader "Unlit/SIE"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ThetaE ("ThetaE", Float) = 0
        _E1 ("E1", Float) = 0
        _E2 ("E2", Float) = 0
        _X0 ("X0", Float) = 0
        _Y0 ("Y0", Float) = 0
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
            float _ThetaE;
            float _E1;
            float _E2;
            float _X0;
            float _Y0;
            static const float PI = 4 * atan(1);

            float2 e2phiq(float e1, float e2)
            {
                float phi = (e1 == 0 && e2 == 0) ? 0 : atan2(e2, e1) / 2;
                float c = min(sqrt(pow(e1, 2) + pow(e2, 2)), 0.9999);
                float q = (1 - c) / (1 + c);
                return float2(phi, min(q, 0.999999));
            }

            float atanh(float x)
            {
                float result = 0;
                if (abs(x) < 1)
                {
                    result = 0.5 * (log(1 + x) - log(1 - x));
                }
                return result;
                // return 0.5 * (log(1 + x) - log(1 - x));
            }

            float2 rotate(float2 pos, float angle)
            {
                float newX =  pos.x * cos(angle) + pos.y * sin(angle);
                float newY = -pos.x * sin(angle) + pos.y * cos(angle);
                return float2(newX, newY);
            }

            // SIS
			// float4 frag(v2f_img i) : SV_Target
			// {
            //     // Texture space (pixel) position
            //     float2 pos = (i.uv - 0.5) / _MainTex_TexelSize.xy;
            //     float r = sqrt(pow(pos.x, 2) + pow(pos.y, 2));
            //     float2 direction = pos / r;
            //     // Deflection angle back in normalized UV space
            //     float2 alpha = -_ThetaE * direction * _MainTex_TexelSize.xy;
            //     // Shift pixels keeping colors the same
			// 	float3 col = tex2D(_MainTex, i.uv + alpha);
			// 	return float4(col, 1.0);
			// }

            // Proper SIE
            float4 frag(v2f_img i) : SV_Target
			{
                // Convert _ThetaE to pixel space, where 1 (max value) corresponds to
                // 1/6 of the field of view
                float pixelScale = max(_MainTex_TexelSize.x, _MainTex_TexelSize.y);
                float thetaEpix = _ThetaE / _MainTex_TexelSize.y / 6;

                // Parameter conversions
                float2 phiq = e2phiq(_E1, _E2);
                float q2 = pow(phiq.y, 2);
                float thetaEeff = thetaEpix / sqrt((1 + q2) / (2 * phiq.y));
                float b = thetaEeff * sqrt(0.5 * (1 + q2));
                float s = 0.001 * sqrt((1 + q2) / (2 * q2));

                // Texture space (pixel) position
                float2 pos = (i.uv - 0.5 - float2(_X0, _Y0)) / _MainTex_TexelSize.xy;
                
                // Rotate
                pos = rotate(pos, phiq.x);
                
                // Compute deflection angle in pixel space
                float psi = sqrt(q2 * (pow(s, 2) + pow(pos.x, 2)) + pow(pos.y, 2));
                float p = sqrt(1 - q2);
                float alphaX = b / p * atan2(p * pos.x, psi + s);
                float alphaY = b / p * atanh(p * pos.y / (psi + q2 * s));

                // Rotate back
                float2 alpha = rotate(float2(alphaX, alphaY), -phiq.x);
                
                // Convert deflection angle back to normalized UV space
				float3 color = tex2D(_MainTex, i.uv - alpha * _MainTex_TexelSize.xy);
				return float4(color, 1.0);
			}
			ENDCG
        }
    }
}
