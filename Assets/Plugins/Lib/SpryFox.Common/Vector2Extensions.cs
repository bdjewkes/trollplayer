using UnityEngine;


namespace SpryFox.Common {

    public static class Vector2Extensions {
        public static Vector2 SetX(this Vector2 self, float x) {
            return new Vector2(x, self.y);
        }

        public static Vector2 SetY(this Vector2 self, float y) {
            return new Vector2(self.x, y);
        }

        public static Vector2 InvertY(this Vector2 self) {
            return new Vector2(self.x, -self.y);
        }

        public static Vector2 InvertScreenY(this Vector2 self) {
            return new Vector2(self.x, Screen.height - self.y);
        }

        public static Vector2 MulPointwise(this Vector2 self, Vector2 o) {
            return new Vector2(self.x * o.x, self.y * o.y);
        }

        public static Vector3 XYZ(this Vector2 self) {
            return self;
        }

        public static Vector2 FromAngle(float angle) {
            // angle is in degrees
            return new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));
        }

        public static Point FloorToPoint(this Vector2 self) {
            return new Point(Mathf.FloorToInt(self.x), Mathf.FloorToInt(self.y));
        }

        public static Point CeilToPoint(this Vector2 self) {
            return new Point(Mathf.CeilToInt(self.x), Mathf.CeilToInt(self.y));
        }

        // Tells you which side of the directed line the point inhabits.
        // > 0: left
        // == 0: on
        // < 0: right
        public static float LineSide(this Vector2 self, Vector2 l0, Vector2 l1) {
            return (l1.x - l0.x) * (self.y - l0.y) - (self.x - l0.x) * (l1.y - l0.y);
        }

        public static Vector2 Left2D(this Vector2 self) {
            return new Vector2(-self.y, self.x);
        }

        public static Vector2 Right2D(this Vector2 self) {
            return new Vector2(self.y, -self.x);
        }

        // removes negative signs from components
        public static Vector2 Abs(this Vector2 self) {
            return new Vector2(Mathf.Abs(self.x), Mathf.Abs(self.y));
        }

        // angle is in degrees
        public static Vector2 RotateBy2D(this Vector2 self, float angle) {
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            return q * self;
        }

        public static float Angle2D(this Vector2 self) {
            return Mathf.Atan2(self.y, self.x) * Mathf.Rad2Deg;
        }

        // always returns with normalized length
        public static Vector2 Slerp(Vector2 fromV, Vector2 toV, float t) {

            // oh god this little knot has so many hidden trig and sqrts.
            var fromAng = fromV.Angle2D();
            var toAng = toV.Angle2D();
            float angleDelta = toAng - fromAng;

            // make sure we're taking the shortest route
            float flipper = angleDelta < 0f ? 360f : -360f;
            float maybeFlip = Mathf.Abs(angleDelta) > 180f ? flipper : 0f;
            float shortestDelta = angleDelta + maybeFlip;

            float angleToMove = shortestDelta * t;
            var newDir = fromV.RotateBy2D(angleToMove).normalized;
            return newDir;
        }

        // if we've already computed the magnitude, can normalize with this for cheaper
        public static Vector2 Normalized(this Vector2 self, float magnitude) {
            if (magnitude == 0) return self;
            return self / magnitude;
        }

        public static Vector2 ClosestPointOnLine(this Vector2 self, Vector2 l0, Vector2 l1) {
            Vector2 vp = self - l0;
            Vector2 vl = l1 - l0;
            float t = Vector2.Dot(vp, vl) / vl.sqrMagnitude;
            return l0 + vl * t;
        }

        public static Vector2 ClosestPointOnSegment(this Vector2 self, Vector2 l0, Vector2 l1) {
            Vector2 vp = self - l0;
            Vector2 vl = l1 - l0;
            float t = Mathf.Clamp01(Vector2.Dot(vp, vl) / vl.sqrMagnitude);
            return l0 + vl * t;
        }

        public static Vector2 SnapToCardinal(this Vector2 self) {
            if(Mathf.Abs(self.x) > Mathf.Abs(self.y)) {
                if(self.x > 0) return new Vector2(1, 0);
                else return new Vector2(-1, 0);
            } else {
                if(self.y > 0) return new Vector2(0, 1);
                else return new Vector2(0, -1);
            }
        }

        // returns true if the point *pt* is within the ellipse centered at 0,0 and with radii of *ellipseRadii*
        public static bool IsWithinEllipse(this Vector2 pt, Vector2 ellipseRadii) {
            return new Vector2(pt.x, pt.y * ellipseRadii.x/ellipseRadii.y).sqrMagnitude < ellipseRadii.x * ellipseRadii.x;
        }
    }
}