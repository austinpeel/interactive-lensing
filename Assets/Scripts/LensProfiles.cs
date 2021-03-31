using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LensProfiles : MonoBehaviour
{
    public NIE nie;

    public struct NIE
    {
        public float Potential(float x, float y, float thetaE, float e1, float e2, float s_scale, float centerX, float centerY)
        {
            // Convert and unpack model parameters
            float[] parameters = ParamConv(thetaE, e1, e2, s_scale);
            float b = parameters[0];
            float s = parameters[1];
            float q = parameters[2];
            float phi = parameters[3];

            float xShifted = x - centerX;
            float yShifted = y - centerY;
            float[] rotatedXY = Rotate(xShifted, yShifted, phi);
            return PotentialMA(rotatedXY[0], rotatedXY[1], b, s, q);
        }

        public float[] Derivatives(float x, float y, float thetaE, float e1, float e2, float s_scale, float centerX, float centerY)
        {
            // Convert and unpack model parameters
            float[] parameters = ParamConv(thetaE, e1, e2, s_scale);
            float b = parameters[0];
            float s = parameters[1];
            float q = parameters[2];
            float phi = parameters[3];

            float xShifted = x - centerX;
            float yShifted = y - centerY;
            float[] rotatedXY = Rotate(xShifted, yShifted, phi);
            float[] rotatedDerivs = DerivativesMA(rotatedXY[0], rotatedXY[1], b, s, q);
            float[] derivs = Rotate(rotatedDerivs[0], rotatedDerivs[1], -phi);
            return derivs;
        }

        public float[][] Derivatives(float[] x, float[] y, float thetaE, float e1, float e2, float s_scale, float centerX, float centerY)
        {
            float[][] result = new float[2][];
            result[0] = new float[x.Length];
            result[1] = new float[x.Length];
            for (int i = 0; i < x.Length; i++)
            {
                float[] deriv = Derivatives(x[i], y[i], thetaE, e1, e2, s_scale, centerX, centerY);
                result[0][i] = deriv[0];
                result[1][i] = deriv[1];
            }
            return result;
        }

        public float PotentialMA(float x, float y, float b, float s, float q)
        {
            float psi = Psi(x, y, q, s);
            float[] derivs = DerivativesMA(x, y, b, s, q);
            float term1 = x * derivs[0] + y * derivs[1];
            float term2 = b * s * Mathf.Log(Mathf.Pow(psi + s, 2) + (1 - Mathf.Pow(q, 2)) * Mathf.Pow(x, 2)) / 2;
            return term1 - term2;
        }

        public float[] DerivativesMA(float x, float y, float b, float s, float q)
        {
            q = Mathf.Clamp(q, 0, 0.99999999f);
            float psi = Psi(x, y, q, s);
            float p = Mathf.Sqrt(1 - Mathf.Pow(q, 2));
            float fX = (b / p) * Mathf.Atan2(p * x, psi + s);
            float fY = (b / p) * ATanh(p * y / (psi + Mathf.Pow(q, 2) * s));
            return new float[2] { fX, fY };
        }

        public float[][] DerivativesMA(float[] x, float[] y, float b, float s, float q)
        {
            float[][] result = new float[2][];
            result[0] = new float[x.Length];
            result[1] = new float[x.Length];
            for (int i = 0; i < x.Length; i++)
            {
                float[] deriv = DerivativesMA(x[i], y[i], b, s, q);
                result[0][i] = deriv[0];
                result[1][i] = deriv[1];
            }
            return result;
        }

        float Psi(float x, float y, float q, float s)
        {
            return Mathf.Sqrt(Mathf.Pow(q, 2) * (Mathf.Pow(s, 2) + Mathf.Pow(x, 2)) + Mathf.Pow(y, 2));
        }

        float[] Psi(float[] x, float[] y, float q, float s)
        {
            float[] result = new float[x.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Psi(x[i], y[i], q, s);
            }
            return result;
        }
    }

    // Utility functions
    public static float[] ParamConv(float thetaE, float e1, float e2, float sScale)
    {
        float[] phi_q = E2PhiQ(e1, e2);
        float phi = phi_q[0];
        float q = phi_q[1];
        float thetaEnew = ThetaEQConvert(thetaE, q);
        float b = thetaEnew * Mathf.Sqrt((1 + Mathf.Pow(q, 2)) / 2);
        float s = sScale * Mathf.Sqrt((1 + Mathf.Pow(q, 2)) / (2 * Mathf.Pow(q, 2)));
        return new float[4] { b, s, q, phi };
    }

    public static float ATanh(float x)
    {
        if (Mathf.Abs(x) > 1)
        {
            throw new System.ArgumentException("Invalid input.");
        }
        return (Mathf.Log(1 + x) - Mathf.Log(1 - x)) / 2;
    }

    public static float[] Rotate(float x, float y, float angle)
    {
        float[] result = new float[2];
        float newX = x * Mathf.Cos(angle) + y * Mathf.Sin(angle);
        float newY = -x * Mathf.Sin(angle) + y * Mathf.Cos(angle);
        result[0] = newX;
        result[1] = newY;
        return result;
    }

    public static float[][] Rotate(float[] x, float[] y, float angle)
    {
        float[][] result = new float[2][];
        result[0] = new float[x.Length];
        result[1] = new float[x.Length];
        for (int i = 0; i < x.Length; i++)
        {
            float[] newXY = Rotate(x[i], y[i], angle);
            result[0][i] = newXY[0];
            result[1][i] = newXY[1];
        }
        return result;
    }

    public static float[] E2PhiQ(float e1, float e2)
    {
        float phi = Mathf.Atan2(e2, e1) / 2;
        float c = Mathf.Sqrt(Mathf.Pow(e1, 2) + Mathf.Pow(e2, 2));
        c = Mathf.Min(c, 0.9999f);
        float q = (1 - c) / (1 + c);
        return new float[2] { phi, q };
    }

    public static float ThetaEQConvert(float thetaE, float q)
    {
        return thetaE / Mathf.Sqrt((1 + Mathf.Pow(q, 2)) / (2 * q));
    }
}
