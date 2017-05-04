//-----------------------------------------------------------------------
// <copyright file="Vector2Ex.cs" company="Taomee Inc.">
//     Copyright (c) 2011 Taomee Inc. All rights reserved.
// </copyright>
// <author email="alexsu@taomee.com">Su Yong</author>
//-----------------------------------------------------------------------

namespace Misc
{
    using System;
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// The Vector2Ex struct provides some extra functions to Unity's Vector2 struct.
    /// </summary>
    public struct Vector2Ex
    {
        /// <summary>
        /// The bias of the side of line test.
        /// </summary>
        public const float SideOfLineBias = 0.01f;

        /// <summary>
        /// Converts an angle in radians to a 2D vector.
        /// </summary>
        /// <param name="angle">The angle in radians.</param>
        /// <returns>The 2D vector.</returns>
        public static Vector2 FromRadian(float angle)
        {
            return new Vector2(CheapMathf.Cos(angle), CheapMathf.Sin(angle));
        }

        /// <summary>
        /// Creates a 2D vector from an angle in radians and a radius.
        /// </summary>
        /// <param name="angle">The angle in radians.</param>
        /// <param name="r">The radius.</param>
        /// <returns>The 2D vector.</returns>
        public static Vector2 FromRadian(float angle, float r)
        {
            return r * FromRadian(angle);
        }

        /// <summary>
        /// Converts an angle in degrees to a 2D vector.
        /// </summary>
        /// <param name="angle">The angle in degrees.</param>
        /// <returns>The 2D vector.</returns>
        public static Vector2 FromDegree(float angle)
        {
            return FromRadian(angle * Mathf.Deg2Rad);
        }

        /// <summary>
        /// Creates a 2D vector from an angle in degrees and a radius.
        /// </summary>
        /// <param name="angle">The angle in degrees.</param>
        /// <param name="r">The radius.</param>
        /// <returns>The 2D vector.</returns>
        public static Vector2 FromDegree(float angle, float r)
        {
            return r * FromDegree(angle);
        }

        /// <summary>
        /// Rotates the vector around the origin.
        /// </summary>
        /// <param name="v">A 2D vector need to rotate.</param>
        /// <param name="angle">An angle in radians.</param>
        /// <returns>The 2D vector as the result of rotation.</returns>
        public static Vector2 Rotate(Vector2 v, float angle)
        {
            float s = CheapMathf.Sin(angle);
            float c = CheapMathf.Cos(angle);

            return new Vector2((c * v.x) + -(s * v.y), (s * v.x) + (c * v.y));
        }

        /// <summary>
        /// Rotates a vector according a rotation which represents the rotation.
        /// </summary>
        /// <param name="v">The given vector to rotate.</param>
        /// <param name="rotation">The vector which represents the rotation.</param>
        /// <returns>The vector rotated.</returns>
        public static Vector2 Rotate(Vector2 v, Vector2 rotation)
        {
            return new Vector2((rotation.x * v.x) - (rotation.y * v.y), (rotation.y * v.x) + (rotation.x * v.y));
        }

        /// <summary>
        /// Rotates a vector according a vector which represents the rotation, then translates it by an offset.
        /// </summary>
        /// <param name="v">The given vector to rotate.</param>
        /// <param name="rotation">The vector which represents the rotation.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>The vector rotated.</returns>
        public static Vector2 RotateTranslate(Vector2 v, Vector2 rotation, Vector2 offset)
        {
            return offset + Rotate(v, rotation);
        }

        /// <summary>
        /// Rotates an array of vector according a vector which represents the rotation.
        /// </summary>
        /// <param name="vectors">The array of vector to rotate.</param>
        /// <param name="rotation">The vector which represents the rotation.</param>
        /// <returns>The array of vector rotated.</returns>
        public static Vector2[] Rotate(Vector2[] vectors, Vector2 rotation)
        {
            return Array.ConvertAll<Vector2, Vector2>(vectors, v => Rotate(v, rotation));
        }

        /// <summary>
        /// Translates an array of vector by an offset which represents the offset.
        /// </summary>
        /// <param name="vectors">The array of vector to translate.</param>
        /// <param name="offset">The vector which represents the offset.</param>
        /// <returns>The array of vector translated.</returns>
        public static Vector2[] Translate(Vector2[] vectors, Vector2 offset)
        {
            return Array.ConvertAll<Vector2, Vector2>(vectors, v => v + offset);
        }

        /// <summary>
        /// Rotates an array of vector according a rotation which represents the rotation
        /// and then translates them by an offset.
        /// </summary>
        /// <param name="vectors">The array of vector to rotate.</param>
        /// <param name="rotation">The vector which represents the rotation.</param>
        /// <param name="offset">The vector which represents the offset.</param>
        /// <returns>The array of vector transformed.</returns>
        public static Vector2[] RotateTranslate(Vector2[] vectors, Vector2 rotation, Vector2 offset)
        {
            return Translate(Rotate(vectors, rotation), offset);
        }

        /// <summary>
        /// Rotates a 2D vector to right.
        /// </summary>
        /// <param name="v">The vector to rotate.</param>
        /// <returns>The vector rotated.</returns>
        public static Vector2 RotateRight(Vector2 v)
        {
            return new Vector2(v.y, -v.x);
        }

        /// <summary>
        /// Rotates a 2D vector to left.
        /// </summary>
        /// <param name="v">The vector to rotate.</param>
        /// <returns>The vector rotated.</returns>
        public static Vector2 RotateLeft(Vector2 v)
        {
            return new Vector2(-v.y, v.x);
        }

        /// <summary>
        /// Checks to see if any of the components of a 2D vector is NaN.
        /// </summary>
        /// <param name="v">The 2D vector.</param>
        /// <returns>True or false.</returns>
        public static bool IsNaN(Vector2 v)
        {
            return float.IsNaN(v.x) || float.IsNaN(v.y);
        }

        /// <summary>
        /// Calculates the square of the distance between two points.
        /// </summary>
        /// <param name="from">The from point.</param>
        /// <param name="to">The to point.</param>
        /// <returns>The square of the distance.</returns>
        public static float SqrDistance(Vector2 from, Vector2 to)
        {
            return (to - from).sqrMagnitude;
        }

        /// <summary>
        /// Calculates the distance between two points.
        /// </summary>
        /// <param name="from">The from point.</param>
        /// <param name="to">The to point.</param>
        /// <returns>The distance.</returns>
        public static float Distance(Vector2 from, Vector2 to)
        {
            return (to - from).magnitude;
        }

        /// <summary>
        /// Converts a 2D vector to a 3D vector.
        /// </summary>
        /// <param name="v">The 2D vector.</param>
        /// <param name="y">The Y component of the 3D vector.</param>
        /// <returns>The 3D vector.</returns>
        public static Vector3 To3D(Vector2 v, float y)
        {
            return new Vector3(v.x, v.y, y);
        }

        /// <summary>
        /// Given 2 lines in 2D space AB, CD. This returns true if an
        /// p occurs and sets dist to the distance the p
        /// occurs along AB. Also sets the 2D vector point to the point of
        /// p.
        /// </summary>
        /// <param name="a">The first end of the line AB.</param>
        /// <param name="b">The second end of the line AB.</param>
        /// <param name="c">The first end of the line CD.</param>
        /// <param name="d">The second end of the line CD.</param>
        /// <param name="dist">The distance the p occurs along AB.</param>
        /// <param name="point">The intersect point.</param>
        /// <returns>The lines intersect or not.</returns>
        public static bool LineIntersection(
            Vector2 a,
            Vector2 b,
            Vector2 c,
            Vector2 d,
            ref float dist,
            ref Vector2 point)
        {
            float denominator = ((b.x - a.x) * (d.y - c.y)) - ((b.y - a.y) * (d.x - c.x));
            if (denominator == 0)
            {
                // lines are parallel
                return false;
            }

            float numerator1 = ((a.y - c.y) * (d.x - c.x)) - ((a.x - c.x) * (d.y - c.y));
            float numerator2 = ((a.y - c.y) * (b.x - a.x)) - ((a.x - c.x) * (b.y - a.y));

            float r = numerator1 / denominator;
            float s = numerator2 / denominator;

            if (r > 0 && r < 1 && s > 0 && s < 1)
            {
                dist = Vector2.Distance(a, b) * r;
                point = a + (r * (b - a));
                return true;
            }
            else
            {
                dist = 0;
                return false;
            }
        }

        /// <summary>
        /// Checks to see the relative position of the projection of a given point to a line segment.
        /// 1. The points FROM and TO are not interchangeable.
        /// 2. The function returns and integer M.
        ///   M is less than 0: The projection is not inside the segment and is on the side near FROM.
        ///   M = 0: The projection is inside the segment.
        ///   M is greater than 0: The projection is not inside the segment and is on the side near TO.
        /// </summary>
        /// <param name="v">The given 2D point V.</param>
        /// <param name="from">The FROM end of the line segment.</param>
        /// <param name="to">The TO end of the line segment.</param>
        /// <returns>The integer M.</returns>
        public static int ProjectPointOnLine(Vector2 v, Vector2 from, Vector2 to)
        {
            Vector2 p2 = v - from;
            Vector2 to2 = to - from;
            float dotProductor = Vector2.Dot(p2, to2);
            if (dotProductor < 0f)
            {
                return -1;
            }

            float sqrNorm = to2.sqrMagnitude;
            if (dotProductor > sqrNorm)
            {
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// Tries to get the projection P of a given point V onto a given line segment [FROM, TO].
        /// 1. The points FROM and TO are not interchangeable.
        /// 2. The function returns and integer M.
        ///   M is less than 0: The projection is not inside the segment and is on the side near FROM.
        ///   M = 0: The projection is inside the segment.
        ///   M is greater than 0: The projection is not inside the segment and is on the side near TO.
        /// 3. R is meaningful if and only if M = 0, say, the projection is inside the segment.
        /// 4. If the projection is inside the segment, the projection position P is indicated by the 
        ///   float number R with P = FROM + R*(TO-FROM).
        /// See also: [Minimum Distance between a Point and a Line](http://paulbourke.net/geometry/pointline/)
        /// </summary>
        /// <param name="v">The given 2D point V.</param>
        /// <param name="from">The FROM end of the line segment.</param>
        /// <param name="to">The TO end of the line segment.</param>
        /// <param name="r">The float number indicating the projection position.</param>
        /// <returns>The integer M.</returns>
        public static int ProjectPointOnLine(Vector2 v, Vector2 from, Vector2 to, ref float r)
        {
            Vector2 p2 = v - from;
            Vector2 to2 = to - from;
            float dotProductor = Vector2.Dot(p2, to2);
            if (dotProductor < 0f)
            {
                return -1;
            }

            float sqrNorm = to2.sqrMagnitude;
            if (dotProductor > sqrNorm)
            {
                return 1;
            }

            r = dotProductor / sqrNorm;

            return 0;
        }

        /// <summary>
        /// Tries to get the projection P of a given point V onto a given line segment [FROM, TO].
        /// 1. The points FROM and TO are not interchangeable.
        /// 2. The function returns and integer M.
        ///   M is less than 0: The projection is not inside the segment and is on the side near FROM.
        ///   M = 0: The projection is inside the segment.
        ///   M is greater than 0: The projection is not inside the segment and is on the side near TO.
        /// 3. P is meaningful if and only if M = 0, say, the projection is inside the segment.
        /// See also: [Minimum Distance between a Point and a Line](http://paulbourke.net/geometry/pointline/)
        /// </summary>
        /// <param name="v">The given 2D point V.</param>
        /// <param name="from">The FROM end of the line segment.</param>
        /// <param name="to">The TO end of the line segment.</param>
        /// <param name="p">The projection point P inside the segment.</param>
        /// <returns>The integer M.</returns>
        public static int ProjectPointOnLine(Vector2 v, Vector2 from, Vector2 to, ref Vector2 p)
        {
            Vector2 p2 = v - from;
            Vector2 to2 = to - from;
            float dotProductor = Vector2.Dot(p2, to2);
            if (dotProductor < 0f)
            {
                return -1;
            }

            float sqrNorm = to2.sqrMagnitude;
            if (dotProductor > sqrNorm)
            {
                return 1;
            }

            float ratio = dotProductor / sqrNorm;

            p = from + (ratio * to2);

            return 0;
        }

        /// <summary>
        /// Tries to get the square distance between a given point V and a given line segment [FROM, TO].
        /// 1. The points FROM and TO are not interchangeable.
        /// 2. The function returns and integer M.
        ///   M is less than 0: The projection is not inside the segment and is on the side near FROM.
        ///   M = 0: The projection is inside the segment.
        ///   M is greater than 0: The projection is not inside the segment and is on the side near TO.
        /// 3. The distance is meaningful if and only if M = 0, say, the projection of V onto [FROM, TO] is 
        ///   inside the segment.
        /// See also: [Minimum Distance between a Point and a Line](http://paulbourke.net/geometry/pointline/)
        /// </summary>
        /// <param name="v">The given 2D point V.</param>
        /// <param name="from">The FROM end of the line segment.</param>
        /// <param name="to">The TO end of the line segment.</param>
        /// <param name="sqrDistance">The square of the distance.</param>
        /// <returns>The integer M.</returns>
        public static int SquareDistanceFromPointToLine(Vector2 v, Vector2 from, Vector2 to, ref float sqrDistance)
        {
            Vector2 intersection = Vector2.zero;
            int ret = ProjectPointOnLine(v, from, to, ref intersection);
            if (ret != 0)
            {
                return ret;
            }

            sqrDistance = Vector2Ex.SqrDistance(v, intersection);
            return 0;
        }

        /// <summary>
        /// Tries to get the distance between a given point V and a given line segment [FROM, TO].
        /// 1. The points FROM and TO are not interchangeable.
        /// 2. The function returns and integer M.
        ///   M is less than 0: The projection is not inside the segment and is on the side near FROM.
        ///   M = 0: The projection is inside the segment.
        ///   M is greater than 0: The projection is not inside the segment and is on the side near TO.
        /// 3. The distance is meaningful if and only if M = 0, say, the projection of V onto [FROM, TO] is 
        ///   inside the segment.
        /// See also: [Minimum Distance between a Point and a Line](http://paulbourke.net/geometry/pointline/)
        /// </summary>
        /// <param name="v">The given 2D point V.</param>
        /// <param name="from">The FROM end of the line segment.</param>
        /// <param name="to">The TO end of the line segment.</param>
        /// <param name="distance">The distance.</param>
        /// <returns>The integer M.</returns>
        public static int DistanceFromPointToLine(Vector2 v, Vector2 from, Vector2 to, ref float distance)
        {
            Vector2 intersection = Vector2.zero;
            int ret = ProjectPointOnLine(v, from, to, ref intersection);
            if (ret != 0)
            {
                return ret;
            }

            distance = Vector2Ex.Distance(v, intersection);
            return 0;
        }

        /// <summary>
        /// Gets the distance with sign from a point to a line.
        /// </summary>
        /// <param name="v">The given point.</param>
        /// <param name="from">The FROM end of the given line.</param>
        /// <param name="to">The TO end of the given line.</param>
        /// <returns>The signed distance.</returns>
        public static float SignedDistanceFromPointToLine(Vector2 v, Vector2 from, Vector2 to)
        {
            Vector2 v2 = v - from;
            Vector2 to2 = (to - from).normalized;
            Vector3 to3 = RotateRight(to2);

            return Vector2.Dot(v2, to3);
        }

        /// <summary>
        /// Checks to see in which side of the line is the given point according to a bias
        /// defined by Vector2Ex.SideOfLineBias.
        /// The indices of the sides are:
        ///   1: in the right side;
        ///   0: on the line;
        ///   -1: in the left side.
        /// </summary>
        /// <param name="v">The given point.</param>
        /// <param name="from">The FROM end of the given line.</param>
        /// <param name="to">The TO end of the given line.</param>
        /// <returns>The index of the side.</returns>
        public static int SideOfLine(Vector2 v, Vector2 from, Vector2 to)
        {
            float signedDistance = SignedDistanceFromPointToLine(v, from, to);

            if (signedDistance > SideOfLineBias)
            {
                return 1;
            }
            else if (-signedDistance > SideOfLineBias)
            {
                return -1;
            }

            return 0;
        }

        /// <summary>
        /// Gets the cosine of the angle between 2 directions.
        /// </summary>
        /// <param name="direction1">A normalized 2D vector representing one direction.</param>
        /// <param name="direction2">A normalized 2D vector representing another direction.</param>
        /// <returns>The cosine.</returns>
        public static float CosineOfAngle(Vector2 direction1, Vector2 direction2)
        {
            return Vector2.Dot(direction1, direction2);
        }

        /// <summary>
        /// Gets the cosine of the angle between 2 directions.
        /// </summary>
        /// <param name="direction">A normalized 2D vector.</param>
        /// <param name="from">The FROM end of another vector.</param>
        /// <param name="to">The TO end of another vector.</param>
        /// <returns>The cosine.</returns>
        public static float CosineOfAngle(Vector2 direction, Vector2 from, Vector2 to)
        {
            return CosineOfAngle(direction, (to - from).normalized);
        }

#if false
        /// <summary>
        /// Gets the signed angle in radians from a given direction to a reference direction.
        /// </summary>
        /// <param name="direction">A normalized 2D vector.</param>
        /// <param name="referenceDirection">The reference normalized 2D vector.</param>
        /// <returns>The signed angle in radians.</returns>
        public static float SignedRadianOfDirections(Vector2 direction, Vector2 referenceDirection)
        {
            float cosine = CosineOfAngle(direction, RotateRight(referenceDirection));
            float angle = (Mathf.PI / 2) - Mathf.Acos(cosine);
            ////Debug.LogError("Mathf.Acos(1f): " + Mathf.Acos(1f).ToString());
            ////Debug.LogError("Mathf.Acos(-1f): " + Mathf.Acos(-1f).ToString());
            ////return Mathf.Clamp(angle, -Mathf.PI / 2, Mathf.PI / 2);
            return angle;
        }

        /// <summary>
        /// Gets the signed angle in radians from a given direction to a reference direction.
        /// </summary>
        /// <param name="direction">A normalized 2D vector.</param>
        /// <param name="from">The FROM end of the reference vector.</param>
        /// <param name="to">The TO end of the reference vector.</param>
        /// <returns>The signed angle in radians.</returns>
        public static float SignedRadianOfDirections(Vector2 direction, Vector2 from, Vector2 to)
        {
            ////float a = SignedRadianOfDirections(Vector2.right, Vector2.up); // test by alex
            ////Debug.LogError("a: " + a.ToString()); // test by alex

            return SignedRadianOfDirections(direction, (to - from).normalized);
        }

        /// <summary>
        /// Gets the signed angle in degrees from a given direction to a reference direction.
        /// </summary>
        /// <param name="direction">A normalized 2D vector.</param>
        /// <param name="referenceDirection">The reference normalized 2D vector.</param>
        /// <returns>The signed angle in degrees.</returns>
        public static float SignedDegreeOfDirections(Vector2 direction, Vector2 referenceDirection)
        {
            return Mathf.Rad2Deg * SignedRadianOfDirections(direction, referenceDirection);
        }

        /// <summary>
        /// Gets the signed angle in degrees from a given direction to a reference direction.
        /// </summary>
        /// <param name="direction">A normalized 2D vector.</param>
        /// <param name="from">The FROM end of the reference vector.</param>
        /// <param name="to">The TO end of the reference vector.</param>
        /// <returns>The signed angle in degrees.</returns>
        public static float SignedDegreeOfDirections(Vector2 direction, Vector2 from, Vector2 to)
        {
            return Mathf.Rad2Deg * SignedRadianOfDirections(direction, from, to);
        }
#endif

        /// <summary>
        /// Checks to see whether a point is fell into an edge-block.
        /// </summary>
        /// <param name="p">The given point.</param>
        /// <param name="from">The FROM end of the edge.</param>
        /// <param name="fromLeftWidth">The left width of the FROM end.</param>
        /// <param name="fromRightWidth">The right width of the FROM end.</param>
        /// <param name="to">The TO end of the edge.</param>
        /// <param name="toLeftWidth">The left width of the TO end.</param>
        /// <param name="toRightWidth">The right width of the TO end.</param>
        /// <returns>True or false.</returns>
        public static bool PointInEdgeBlock(Vector2 p, Vector2 from, float fromLeftWidth, float fromRightWidth, Vector2 to, float toLeftWidth, float toRightWidth)
        {
            int m = ProjectPointOnLine(p, from, to);
            if (0 != m)
            {
                ////Debug.LogWarning("ProjectPointOnLine != 0"); // test by alex
                return false;
            }

            Vector2 right = RotateRight(to - from).normalized;

            int s1 = SideOfLine(p, from + (fromRightWidth * right), to + (toRightWidth * right));
            if (s1 != -1)
            {
                // not at the left side of right edge of the block
                ////Debug.LogWarning("not at the left side of right edge of the block"); // test by alex
                return false;
            }

            int s2 = SideOfLine(p, from - (fromLeftWidth * right), to - (toLeftWidth * right));
            if (s2 != 1)
            {
                // not at the right side of left edge of the block
                ////Debug.LogWarning("not at the right side of left edge of the block"); // test by alex
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the radian of a 2D vector. The radian is in the range of [0, 2*Pi).
        /// </summary>
        /// <param name="v">The 2D vector.</param>
        /// <returns>The radian of the angle.</returns>
        public static float RadianOfVector(Vector2 v)
        {
            Vector2 direction = v.normalized;
            if (direction.y >= 0)
            {
                return Mathf.Acos(direction.x);
            }

            return (Mathf.PI * 2) - Mathf.Acos(direction.x);
        }

        /// <summary>
        /// Gets the degree of a 2D vector. The degree is in the range of [0, 360).
        /// </summary>
        /// <param name="v">The 2D vector.</param>
        /// <returns>The degree of the angle.</returns>
        public static float DegreeOfVector(Vector2 v)
        {
            return Mathf.Rad2Deg * RadianOfVector(v);
        }

        public static float PerpDot(Vector2 a, Vector2 b)
        {
            return (a.y * b.x) - (a.x * b.y);
        }

        public static float PerpDot(Vector2 a, Vector2 b, Vector2 x)
        {
            return PerpDot(a - x, b - x);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s">A normalized vector.</param>
        /// <param name="t">A normalized vector.</param>
        /// <returns></returns>
        public static Vector2 MeanDirection(Vector2 s, Vector2 t)
        {
            Vector2 m = (s + t).normalized;
            if (m.magnitude > 0.5f)
            {
                return m;
            }

            return t;
        }

        ////public static Vector2 MeanDirection(Vector2 a, Vector2 b, Vector2 c)
        ////{
        ////    Vector2 s = (a - b).normalized;
        ////    Vector2 t = (c - b).normalized;
        ////    return MeanDirection(s, t);
        ////}

        /// <summary>
        /// Converts a global vector to local according to a direction.
        /// </summary>
        /// <param name="globalVector">The vector in global.</param>
        /// <param name="direction">The direction which is a normalized vector.</param>
        /// <returns>The local vector</returns>
        public static Vector2 GlobalToLocalVector(Vector2 globalVector, Vector2 direction)
        {
            return new Vector2(Vector2.Dot(globalVector, direction), Vector2Ex.PerpDot(globalVector, direction));
        }

        /// <summary>
        /// Converts a global position to local vector according to a directed segment.
        /// </summary>
        /// <param name="globalPos">The position in global.</param>
        /// <param name="from">The FROM position of the segment.</param>
        /// <param name="to">The TO position of the segment.</param>
        /// <returns>The local vector.</returns>
        public static Vector2 GlobalToLocalVector(Vector2 globalPos, Vector2 from, Vector2 to)
        {
            Vector2 direction = (to - from).normalized;
            Vector2 globalVector = globalPos - from;

            return GlobalToLocalVector(globalVector, direction);
        }

        /// <summary>
        /// Converts a local vector to global according to a direction.
        /// </summary>
        /// <param name="localVector">The local vector.</param>
        /// <param name="xDirection">The direction of the x axis.</param>
        /// <returns></returns>
        public static Vector2 LocalToGlobalVector(Vector2 localVector, Vector2 xDirection)
        {
            return localVector.x * xDirection + localVector.y * RotateLeft(xDirection);
        }
    }
}
