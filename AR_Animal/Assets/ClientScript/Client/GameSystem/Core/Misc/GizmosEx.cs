/********************************************************************
	created:	2015/01/04
	created:	4:1:2015   10:36
	filename: 	C:\XGame\Assets\XGameScript\Engine\Misc\GizmosEx.cs
	file path:	C:\XGame\Assets\XGameScript\Engine\Misc
	file base:	GizmosEx
	file ext:	cs
	author:		michael
	
	purpose:	Debug Draw Helper
*********************************************************************/

namespace Misc
{
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// This class provides some extra gizmos drawing functions.
    /// </summary>
    public class GizmosEx
    {
        /// <summary>
        /// The default value of Y.
        /// </summary>
        private const float DefaultY = 0f;

        /// <summary>
        /// A helper function to draw a sphere with gizmos.
        /// </summary>
        /// <param name="center">The 2D center position of the sphere.</param>
        /// <param name="radius">The radius of the sphere.</param>
        public static void DrawSphere(Vector2 center, float radius)
        {
            Gizmos.DrawSphere(Vector2Ex.To3D(center, DefaultY), radius);
        }

        /// <summary>
        /// A helper function to draw a circle with gizmos.
        /// </summary>
        /// <param name="center">The center position of the circle.</param>
        /// <param name="radius">The radius of the circle.</param>
        public static void DrawCircle(Vector3 center, float radius)
        {
            const int CountSegment = 64;

            float angle = 0;

            for (int i = 0; i < CountSegment; ++i)
            {
                Vector3 from = (new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius) + center;
                angle += Mathf.PI * 2 / CountSegment;
                Vector3 to = (new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius) + center;
                Gizmos.DrawLine(from, to);
            }
        }

        /// <summary>
        /// A helper function to draw a circle with gizmos.
        /// </summary>
        /// <param name="center">The 2D position of the center of the circle.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="y">The y coordinate of the center of the circle.</param>
        public static void DrawCircle(Vector2 center, float radius, float y)
        {
            DrawCircle(new Vector3(center.x, center.y, y), radius);
        }

        /// <summary>
        /// A helper function to draw a circle with gizmos.
        /// </summary>
        /// <param name="center">The 2D position of the center of the circle.</param>
        /// <param name="radius">The radius of the circle.</param>
        public static void DrawCircle(Vector2 center, float radius)
        {
            DrawCircle(center, radius, DefaultY);
        }

        /// <summary>
        /// A helper function to draw a ray starting at from to from + direction with gizmos.
        /// </summary>
        /// <param name="from">The starting position.</param>
        /// <param name="direction">The direction 2D vector.</param>
        public static void DrawRay(Vector3 from, Vector2 direction)
        {
            Gizmos.DrawRay(from, new Vector3(direction.x, direction.y, 0));
        }

        /// <summary>
        /// A helper function to draw a ray starting at from to from + direction with gizmos.
        /// </summary>
        /// <param name="from">The starting 2D position.</param>
        /// <param name="direction">The direction 2D vector.</param>
        /// <param name="y">The y coordinate of the center of the ray.</param>
        public static void DrawRay(Vector2 from, Vector2 direction, float y)
        {
            DrawRay(new Vector3(from.x, from.y, y), direction);
        }

        /// <summary>
        /// A helper function to draw a ray starting at from to from + direction with gizmos.
        /// </summary>
        /// <param name="from">The starting 2D position.</param>
        /// <param name="direction">The direction 2D vector.</param>
        public static void DrawRay(Vector2 from, Vector2 direction)
        {
            DrawRay(from, direction, DefaultY);
        }

        /// <summary>
        /// Draw line with gizmos.
        /// </summary>
        /// <param name="from">The 3D From position.</param>
        /// <param name="to">The 2D To position.</param>
        public static void DrawLine(Vector3 from, Vector2 to)
        {
            Gizmos.DrawLine(from, new Vector3(to.x, to.y, from.y));
        }

        /// <summary>
        /// Draw line with gizmos.
        /// </summary>
        /// <param name="from">The 2D From position.</param>
        /// <param name="to">The 2D To position.</param>
        public static void DrawLine(Vector2 from, Vector2 to)
        {
            DrawLine(new Vector3(from.x,  from.y, DefaultY), to);
        }

        public static void DrawLine(Vector2 from, Vector2 to, float y)
        {
            Gizmos.DrawLine(Vector2Ex.To3D(from, y), Vector2Ex.To3D(to, y));
        }

        public static void DrawRect(Vector2 center, float r)
        {
            Vector2 lt = new Vector2(center.x - r, center.y - r);
            Vector2 rt = new Vector2(center.x + r, center.y - r);
            Vector2 lb = new Vector2(center.x - r, center.y + r);
            Vector2 rb = new Vector2(center.x + r, center.y + r);
            GizmosEx.DrawLine(lt, rt);
            GizmosEx.DrawLine(lt, lb);
            GizmosEx.DrawLine(rt, rb);
            GizmosEx.DrawLine(lb, rb);
        }

        public static void DrawQuadrangle(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            DrawQuadrangle(a, b, c, d, DefaultY);
        }

        public static void DrawQuadrangle(Vector2 a, Vector2 b, Vector2 c, Vector2 d, float y)
        {
            Vector3 a3d = Vector2Ex.To3D(a, y);
            Vector3 b3d = Vector2Ex.To3D(b, y);
            Vector3 c3d = Vector2Ex.To3D(c, y);
            Vector3 d3d = Vector2Ex.To3D(d, y);

            Gizmos.DrawLine(a3d, b3d);
            Gizmos.DrawLine(b3d, c3d);
            Gizmos.DrawLine(c3d, d3d);
            Gizmos.DrawLine(d3d, a3d);
        }

        /// <summary>
        /// Draw text with gizmos.
        /// </summary>
        /// <param name="position">The 3D position of the text.</param>
        /// <param name="text">The text to draw.</param>
        public static void DrawText(Vector3 position, string text)
        {
            TextGizmos.Instance.Draw(position, text);
        }

        /// <summary>
        /// Draw text with gizmos.
        /// </summary>
        /// <param name="position">The 3D position of the text.</param>
        /// <param name="line">Offset by lines.</param>
        /// <param name="text">The text to draw.</param>
        public static void DrawText(Vector3 position, int line, string text)
        {
            // FIXME: Don't use hard coded value.
            position.z += line * 3f;
            DrawText(position, text);
        }

        public static void DrawText(Vector2 position, string text)
        {
            DrawText(Vector2Ex.To3D(position, DefaultY), text);
        }

        public static void DrawRect(float top, float bottom, float left, float right)
        {
            DrawRect(top, bottom, left, right, DefaultY);
        }

        public static void DrawRect(float top, float bottom, float left, float right, float y)
        {
            Vector2 tl = new Vector2(left, top);
            Vector2 tr = new Vector2(right, top);
            Vector2 bl = new Vector2(left, bottom);
            Vector2 br = new Vector2(right, bottom);

            DrawLine(tl, tr, y);
            DrawLine(bl, br, y);
            DrawLine(tl, bl, y);
            DrawLine(tr, br, y);
        }

        private static readonly Vector2[] arrowHeadPath = new Vector2[]
        {
            new Vector2( -2f, 1f),
            new Vector2(-2f, 2f),
            new Vector2(0f, 0f),
            new Vector2(-2f, -2f),
            new Vector2(-2f, -1f),
        };

        private static readonly Vector2[] arrowTailPath = new Vector2[]
        {
            new Vector2(0f, 2f),
            new Vector2(0f, -2f),
        };

        private static void DrawPath(Vector2[] path)
        {
            for (int i = 1; i < path.Length; ++i)
            {
                GizmosEx.DrawLine(path[i - 1], path[i]);
            }
        }

        public static void DrawArrow(Vector3 from, Vector3 to)
        {
            DrawArrow(Vector3Ex.To2D(from), Vector3Ex.To2D(to));
        }

        public static void DrawArrow(Vector3 from, Vector2 to)
        {
            DrawArrow(from, Vector2Ex.To3D(to, DefaultY));
        }

        public static void DrawArrow(Vector2 from, Vector2 to)
        {
            Vector2 direction = (to - from).normalized;

            var headPath = Vector2Ex.RotateTranslate(arrowHeadPath, direction, to);
            GizmosEx.DrawPath(headPath);

            var tailPath = Vector2Ex.RotateTranslate(arrowTailPath, direction, from);
            GizmosEx.DrawPath(tailPath);

            GizmosEx.DrawLine(tailPath[0], headPath[0]);
            GizmosEx.DrawLine(tailPath[tailPath.Length - 1], headPath[headPath.Length - 1]);
        }

        public static void DrawLocalOffset(Vector2 from, Vector2 localOffset, Vector2 xDirection)
        {
            Vector2 globalOffset = Vector2Ex.LocalToGlobalVector(localOffset, xDirection);
            Vector2 to = from + globalOffset;
            DrawLine(from, to);
            DrawSphere(to, 0.6f);
        }
    }
}
