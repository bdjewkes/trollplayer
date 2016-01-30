using UnityEngine;
using System.Collections;

namespace SpryFox.Common {

    public static class RectExtensions {
        public static bool Intersects(this Rect a, Rect b) {
            return !((a.xMin > b.xMax) ||
                     (a.xMax < b.xMin) ||
                     (a.yMin > b.yMax) ||
                     (a.yMax < b.yMin));
        }

        public static Rect Join(this Rect a, Rect b) {
            float x = a.xMin < b.xMin ? a.xMin : b.xMin;
            float y = a.yMin < b.yMin ? a.yMin : b.yMin;
            var r = new Rect(
                x,
                y,
                (a.xMax > b.xMax ? a.xMax : b.xMax) - x,
                (a.yMax > b.yMax ? a.yMax : b.yMax) - y);

            // This deals with a strange case: due to the way that rects are 
            // stored as a position and extent and the rounding errors of 
            // floating-point math, it's possible for a joined rectangle to have
            // a Max value smaller than the corresponding Max value of the larger
            // of the two parent Rects.
            // The fix is to detect this case and to add the smallest possible
            // value to the offending extent that will push it over the line and
            // be larger than either one of its parents' Maxes.
            // It's two ulps because occasionally the rounding involved in
            // computing the Max obliterates one ulp
            if (r.xMax < a.xMax || r.xMax < b.xMax) {
                r.width += Mathf.Abs(MathPlus.Ulp(r.width)) * 2;
            }
            if (r.yMax < a.yMax || r.yMax < b.yMax) {
                r.height += Mathf.Abs(MathPlus.Ulp(r.height)) * 2;
            }
            return r;
        }

        public static bool ContainsStrict(this Rect a, Rect b) {
            return b.xMax < a.xMax &&
                b.xMin > a.xMin &&
                b.yMin > a.yMin &&
                b.yMax < a.yMax;
        }

        public static bool Contains(this Rect a, Rect b) {
            return b.xMin >= a.xMin &&
                b.xMax <= a.xMax &&
                b.yMin >= a.yMin &&
                b.yMax <= a.yMax;
        }

        // if pos is outside bounds, clamps pos to the edge
        public static Vector2 ClipPosition(this Rect bounds, Vector2 p) {
            float clampedX = Mathf.Clamp(p.x, bounds.xMin, bounds.xMax);
            float clampedY = Mathf.Clamp(p.y, bounds.yMin, bounds.yMax);
            var clampedPos = new Vector2(clampedX, clampedY);
            return clampedPos;
        }

        // returns how long ray can be and still remain in box.
        // if ray starts outside box then returns rayLength.
        // if ray starts inside box but stops before box edge, returns rayLength
        public static float ClipRay(this Rect box,
                                    Vector2 rayOrigin, Vector2 rayDir, float rayLength) {
            if (box.Contains(rayOrigin) == false) {
                return rayLength;
            }

            var rayEnd = rayOrigin + rayDir * rayLength;
            float clippedRayLength = rayLength;

            if (rayEnd.x > box.xMax) {
                float horizontalDistanceToEdge = box.xMax - rayOrigin.x;
                clippedRayLength = horizontalDistanceToEdge / rayDir.x;
            } else if (rayEnd.x < box.xMin) {
                float horizontalDistanceToEdge = box.xMin - rayOrigin.x;
                clippedRayLength = horizontalDistanceToEdge / rayDir.x;
            }

            if (rayEnd.y > box.yMax) {
                float verticalDistanceToEdge = box.yMax - rayOrigin.y;
                float clippedLengthToTopEdge = verticalDistanceToEdge / rayDir.y;
                clippedRayLength = Mathf.Min(clippedRayLength, clippedLengthToTopEdge);
            } else if (rayEnd.y < box.yMin) {
                float verticalDistanceToEdge = box.yMin - rayOrigin.y;
                float clippedLengthToBottomEdge = verticalDistanceToEdge / rayDir.y;
                clippedRayLength = Mathf.Min(clippedRayLength, clippedLengthToBottomEdge);
            }

            return clippedRayLength;
        }

    }

}