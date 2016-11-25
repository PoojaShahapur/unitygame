using UnityEngine;
using System.Collections;

namespace Assets.BEP.Vol._2.Scripts
{
    public class MathToolManager
    {
        //mass = radius * radius * radius
        public static float getMassByRadius(float radius)
        {
            return Mathf.Pow(radius, 3.0f);
        }

        public static float getRadiusByMass(float mass)
        {
            return Mathf.Pow(mass, 1.0f / 3.0f);
        }

        public static float getSquare(float num)
        {
            return num * num;
        }
    }
}
