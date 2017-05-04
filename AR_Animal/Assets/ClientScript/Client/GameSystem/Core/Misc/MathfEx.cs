//-----------------------------------------------------------------------
// <copyright file="MathfEx.cs" company="Taomee Inc.">
//     Copyright (c) 2011 Taomee Inc. All rights reserved.
// </copyright>
// <author email="alexsu@taomee.com">Su Yong</author>
//-----------------------------------------------------------------------

namespace Misc
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// This class provides some math functions which relates single precision float arithmetic.
    /// </summary>
    public class MathfEx
    {
        /// <summary>
        /// Return a square of the float.
        /// </summary>
        /// <param name="x">A float number.</param>
        /// <returns>A square of the input float number.</returns>
        public static float Square(float x)
        {
            return x * x;
        }

        /// <summary>
        /// The binomial random function.
        /// </summary>
        /// <returns>A random number between -1.0 and 1.0.</returns>
        public static float RandomBinomial()
        {
            return UnityEngine.Random.value - UnityEngine.Random.value;
        }

        /// <summary>
        /// The random method return true or false.
        /// </summary>
        /// <returns>True or false.</returns>
        public static bool RandomBoolean()
        {
            int val = UnityEngine.Random.Range(0, 2);
            return val == 0;
        }

        /// <summary>
        /// Return true in the probability of the threshold.
        /// </summary>
        /// <param name="threshold">The threshold which is a value between 0.0 and 1.0.</param>
        /// <returns>True or false.</returns>
        public static bool RandomTrue(float threshold)
        {
            return UnityEngine.Random.value < threshold;
        }

        /// <summary>
        /// Generates a vector in the disc specified by a given radius.
        /// </summary>
        /// <param name="radius">The radius of the disc.</param>
        /// <returns>The generated vector.</returns>
        public static Vector2 RandomDisc(float radius)
        {
            float distance = UnityEngine.Random.Range(0, radius);
            float angle = UnityEngine.Random.Range(-Mathf.PI, Mathf.PI);

            return distance * new Vector2(CheapMathf.Cos(angle), CheapMathf.Sin(angle));
        }

        /// <summary>
        /// Generates a vector in a rectangle.
        /// </summary>
        /// <param name="top">The top of the rectangle.</param>
        /// <param name="bottom">The bottom of the rectangle.</param>
        /// <param name="left">The left of the rectangle.</param>
        /// <param name="right">The right of the rectangle.</param>
        /// <returns>The generated vector.</returns>
        public static Vector2 RandomRect(float top, float bottom, float left, float right)
        {
            float x = UnityEngine.Random.Range(left, right);
            float y = UnityEngine.Random.Range(top, bottom);

            return new Vector2(x, y);
        }

        /// <summary>
        /// Clamps a value by a maximum absolute value..
        /// </summary>
        /// <param name="value">The value to be clamped.</param>
        /// <param name="maxAbs">The maximum absolute value. It should not be negative.</param>
        /// <returns>The clamped value.</returns>
        public static float Clamp(float value, float maxAbs)
        {
            return Mathf.Clamp(value, -maxAbs, maxAbs);
        }

        /// <summary>
        /// Clamps a value by a maximum absolute value..
        /// </summary>
        /// <param name="value">The value to be clamped.</param>
        /// <param name="maxAbs">The maximum absolute value. It should not be negative.</param>
        /// <returns>The clamped value.</returns>
        public static int Clamp(int value, int maxAbs)
        {
            return Mathf.Clamp(value, -maxAbs, maxAbs);
        }

        public static void Clamp(ref float value, float maxAbs)
        {
            value = Clamp(value, maxAbs);
        }

        /// <summary>
        /// Gets simple reverse of a value.
        /// </summary>
        /// <param name="q">The given value.</param>
        /// <param name="maxAbsQ">The maximum absolute value of Q.</param>
        /// <param name="maxAbsP">The maximum absolute value of the reserve of Q.</param>
        /// <returns>The simple reversed value.</returns>
        public static float SimpleReverse(float q, float maxAbsQ, float maxAbsP)
        {
            return maxAbsP * SimpleReverse(q, maxAbsQ);
        }

        /// <summary>
        /// Gets simple reverse ratio of a value.
        /// </summary>
        /// <param name="q">The given value.</param>
        /// <param name="maxAbsQ">The maximum absolute value of Q.</param>
        /// <returns>The simple reversed ratio.</returns>
        public static float SimpleReverse(float q, float maxAbsQ)
        {
            return -Clamp(q, maxAbsQ) / maxAbsQ;
        }

        /// <summary>
        /// Checks to see if a value is in the given range.
        /// </summary>
        /// <param name="value">The given value.</param>
        /// <param name="maxAbs">The maximum absolute value to define the range.</param>
        /// <returns>True or false.</returns>
        public static bool InRange(float value, float maxAbs)
        {
            return value < maxAbs && -maxAbs < value;
        }

        /// <summary>
        /// Checks to see if a value is on the given range.
        /// </summary>
        /// <param name="value">The given value.</param>
        /// <param name="maxAbs">The maximum absolute value to define the range.</param>
        /// <returns>True or false.</returns>
        public static bool OnRange(float value, float maxAbs)
        {
            return value <= maxAbs && -maxAbs <= value;
        }

        /// <summary>
        /// Checks to see if a value is in the given range.
        /// </summary>
        /// <param name="value">The given value.</param>
        /// <param name="maxAbs">The maximum absolute value to define the range.</param>
        /// <returns>True or false.</returns>
        public static bool InRange(int value, int maxAbs)
        {
            return value < maxAbs && -maxAbs < value;
        }

        /// <summary>
        /// Checks to see if a value is on the given range.
        /// </summary>
        /// <param name="value">The given value.</param>
        /// <param name="maxAbs">The maximum absolute value to define the range.</param>
        /// <returns>True or false.</returns>
        public static bool OnRange(int value, int maxAbs)
        {
            return value <= maxAbs && -maxAbs <= value;
        }

        public static float Min(IEnumerable<float> list)
        {
            float min = Mathf.Infinity;
            foreach (var item in list)
            {
                if (item < min)
                {
                    min = item;
                }
            }

            return min;
        }

        public static float Floor(float value, float errorLength)
        {
            return errorLength * Mathf.Floor(value / errorLength);
        }

        public static Vector2 Floor(Vector2 value, float errorLength)
        {
            return new Vector2(Floor(value.x, errorLength), Floor(value.y, errorLength));
        }

        public static float WeakDistance(Vector2 p1, Vector2 p2)
        {
            Vector2 dp = p1 - p2;
            return Mathf.Max(Mathf.Abs(dp.x), Mathf.Abs(dp.y));
        }

        public static float WeakDistance(Vector3 p1, Vector3 p2)
        {
            Vector3 dp = p1 - p2;
            return Mathf.Max(Mathf.Abs(dp.x), Mathf.Abs(dp.y), Mathf.Abs(dp.z));
        }
    }
}
