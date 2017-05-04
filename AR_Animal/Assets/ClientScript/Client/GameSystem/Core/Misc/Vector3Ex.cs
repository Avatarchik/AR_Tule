//-----------------------------------------------------------------------
// <copyright file="Vector3Ex.cs" company="Taomee Inc.">
//     Copyright (c) 2011 Taomee Inc. All rights reserved.
// </copyright>
// <author email="alexsu@taomee.com">Su Yong</author>
//-----------------------------------------------------------------------

namespace Misc
{
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// The Vector3Ex struct provides some extra functions to Unity's Vector3 struct.
    /// </summary>
    public struct Vector3Ex
    {
        /// <summary>
        /// Calculates the square of the distance of two points.
        /// </summary>
        /// <param name="from">The from point.</param>
        /// <param name="to">The to point.</param>
        /// <returns>The square of the distance.</returns>
        public static float SqrDistance(Vector3 from, Vector3 to)
        {
            return (to - from).sqrMagnitude;
        }

        /// <summary>
        /// Adds a 3D vector and a 2D vector.
        /// </summary>
        /// <param name="v1">The 3D vector.</param>
        /// <param name="v2">The 2D vector.</param>
        /// <returns>The resulting 3D vector.</returns>
        public static Vector3 Add(Vector3 v1, Vector2 v2)
        {
            return new Vector3(v1.x + v2.x, v1.y, v1.z + v2.y);
        }

        /// <summary>
        /// Adds a 2D vector and a 3D vector.
        /// </summary>
        /// <param name="v1">The 2D vector.</param>
        /// <param name="v2">The 3D vector.</param>
        /// <returns>The resulting 3D vector.</returns>
        public static Vector3 Add(Vector2 v1, Vector3 v2)
        {
            return new Vector3(v1.x + v2.x, v2.y, v1.y + v2.z);
        }

        /// <summary>
        /// Converts a 3D vector to 2D.
        /// </summary>
        /// <param name="v">The 3D vector.</param>
        /// <returns>The 2D vector.</returns>
        public static Vector2 To2D(Vector3 v)
        {
            return new Vector2(v.x, v.z);
        }
    }
}
