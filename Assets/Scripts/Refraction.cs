using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace main
{
    public class Refraction
    {
        static public float air = 1.000292f;
        static public float glass = 1.4585f;
        static public float water = 1.330f;
        static public float water_glucose_60p = 1.4394f;
        static public float one_point_zero = 1f;
        static public float one_pont_five = 1.5f;

        /// <summary>
        /// 射影を使って求めてみたが、うまくいかない。
        /// </summary>
        /// <returns>The refraction use projection.</returns>
        /// <param name="incidentLight">Incident light.</param>
        /// <param name="normal">Normal.</param>
        /// <param name="n1">N1.</param>
        /// <param name="n2">N2.</param>
        static public Vector3 GetRefractionUseProjection(Vector3 incidentLight, Vector3 normal, float n1, float n2)
        {
            /*
                     /|
                    / |  adjacent(to down Vector)
       hypotenuse  /  |
                   ----
                 oposite <---(to left Vector)
            */

            float nInv = n1 / n2;
            Vector3 adjacent = CalculateProjection(normal, -incidentLight, false).normalized;

            float iAngle = Vector3.Angle(incidentLight, normal) * Mathf.Deg2Rad;
            float rAngle = Mathf.Asin(nInv * Mathf.Sin(iAngle));

            float adjacentLen = Mathf.Cos(rAngle);
            adjacent = adjacent * adjacentLen;

            Vector3 iHypotenuse = -incidentLight * (adjacentLen / Mathf.Cos(iAngle));
            float iOppositeLen = Mathf.Sin(iAngle);

            Vector3 oppostie = (iHypotenuse - adjacent).normalized;
            float rOppositeLen = Mathf.Sin(rAngle);

            Vector3 refracted = oppostie * rOppositeLen + adjacent;
         
            return -(refracted.normalized);
        }

        /// <summary>
        /// 射影ベクトルを求める
        /// </summary>
        static public Vector3 CalculateProjection(Vector3 v, Vector3 receive, bool isNormalized = false)
        {
            if (isNormalized)
            {
                return Vector3.Dot(v, receive) * v;
            }
            else
            {
                return Vector3.Dot(v, receive) / Mathf.Pow(v.magnitude, 2) * v;
            }
        }

        /// <summary>
        /// 以下のサイトの式を参考にさせていただいた。
        /// https://www.vcl.jp/~kanazawa/raytracing/?page_id=478
        /// </summary>
        /// <returns>The refraction2.</returns>
        /// <param name="incidentLight">Incident light.入射する方向のベクトル</param>
        /// <param name="normal">Normal.</param>
        /// <param name="n1">N1.</param>
        /// <param name="n2">N2.</param>
        static public Vector3 GetRefractionFormula(Vector3 incidentLight, Vector3 normal, float n1, float n2)
        {
            float shita = Vector3.Angle(-incidentLight, normal) * Mathf.Deg2Rad;
            Vector3 refracted = (n1 / n2) * (incidentLight - 
                ((Mathf.Sqrt((n2 / n1)* (n2 / n1)-(1- Mathf.Cos(shita) * Mathf.Cos(shita))) - Mathf.Cos(shita)) * normal));
            return refracted;
        }
    }
}
