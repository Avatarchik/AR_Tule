//-----------------------------------------------------------------------
// <copyright file="CheapMathf.cs" company="Taomee Inc.">
//     Copyright (c) 2012 Taomee Inc. All rights reserved.
// </copyright>
// <author email="alexsu@taomee.com">Su Yong</author>
//-----------------------------------------------------------------------

namespace Misc
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// The CheapMathf class defines ...
    /// </summary>
    public class CheapMathf
    {
        #region Fields

        public const int SineSampleCount = 360 * 4;

        public const float Radian2SineSampleIndex = SineSampleCount / (2 * Mathf.PI);

        private static float[] SineTable = null;

        #endregion

        #region Constructors
        #endregion

        #region Enumerators
        #endregion

        #region Properties
        #endregion

        #region Methods

        public static float Sin(float t)
        {
            return Sin((int)(t * Radian2SineSampleIndex));
        }

        public static float Cos(float t)
        {
            return Sin(t + (Mathf.PI / 2));
        }

        public static float SinOfDegree(float t)
        {
            return Sin(t * Mathf.Deg2Rad);
        }

        public static float CosOfDegree(float t)
        {
            return Cos(t * Mathf.Deg2Rad);
        }

        public static void Init()
        {
            SineTable = new float[SineSampleCount];
            float step = 2 * Mathf.PI / SineSampleCount;
            for (int i = 0; i < SineSampleCount; ++i)
            {
                float t = step * i;
                SineTable[i] = Mathf.Sin(t);
            }
        }

        private static float Sin(int sineSampleIndex)
        {
            int i = sineSampleIndex % SineSampleCount;
            if (i < 0)
            {
                i += SineSampleCount;
            }

            return SineTable[i];
        }

        #endregion
    }
}