using UnityEngine;

namespace SpryFox.Common {

    public static class Vector3Extensions {
        public static Vector2 XY(this Vector3 self) {
            return new Vector2(self.x, self.y);
        }

        public static Vector3 ProjectOntoXZ(this Vector3 self) {
            return new Vector3(self.x, 0, self.z);
        }

        public static Vector3 ProjectOntoXY(this Vector3 self) {
            return new Vector3(self.x, self.y);
        }

        public static Vector3 InvertScreenY(this Vector3 self) {
            return new Vector3(self.x, Screen.height - self.y, self.z);
        }

        public static Vector3 Left2D(this Vector3 self) {
            return new Vector3(-self.y, self.x, 0);
        }

        public static Vector3 Right2D(this Vector3 self) {
            return new Vector3(self.y, -self.x, 0);
        }

        // because we can't do Vector3? as it's a struct
        public struct VectorResult {

            public static VectorResult CreateValid(Vector3 result) {
                return new VectorResult {
                    Result = result,
                    IsValid = true,
                };
            }

            public static readonly VectorResult INVALID
            = new VectorResult { IsValid = false, };

            public Vector3 Result;
            public bool IsValid;
        }

        public static VectorResult IntersectionWithPlane(
            this Ray ray, Vector3 planeOrigin, Vector3 planeNormal) {

            float rayPlaneParallelness = Vector3.Dot(planeNormal, ray.direction);
            float rayOriginPlaneOffset = Vector3.Dot((planeOrigin - ray.origin), planeNormal);

            if (Mathf.Approximately(rayPlaneParallelness, 0f)) {
                // ray parallel to plane. can only intersect if starts inside plane!

                if (Mathf.Approximately(rayOriginPlaneOffset, 0f)) {
                    return VectorResult.CreateValid(ray.origin);
                } else {
                    return VectorResult.INVALID;
                }

            } else {
                float rayT = rayOriginPlaneOffset / rayPlaneParallelness;
                var intersectionPoint = ray.origin + ray.direction * rayT;
                return VectorResult.CreateValid(intersectionPoint);
            }
        }

        // Tells you which side of the directed line the point inhabits.
        // > 0: left
        // == 0: on
        // < 0: right
        public static float LineSide(this Vector3 self, Vector3 l0, Vector3 l1) {
            return (l1.x - l0.x) * (self.y - l0.y) - (self.x - l0.x) * (l1.y - l0.y);
        }

        public static Vector3 ClosestPointOnLine(this Vector3 self, Vector3 l0, Vector3 l1) {
            Vector3 vp = self - l0;
            Vector3 vl = l1 - l0;
            float t = Vector3.Dot(vp, vl) / vl.sqrMagnitude;
            return l0 + vl * t;
        }

        public static Vector3 ClosestPointOnSegment(this Vector3 self, Vector3 l0, Vector3 l1) {
            Vector3 vp = self - l0;
            Vector3 vl = l1 - l0;
            float t = Mathf.Clamp01(Vector3.Dot(vp, vl) / vl.sqrMagnitude);
            return l0 + vl * t;
        }

        public const float layerResolution = 64;
        public const float layerDelta = 1f / layerResolution;
        public static Vector3 SetLayer(this Vector3 self, int layer) {
            self.z = 0 + (layerDelta * layer);
            return self;
        }

        public static Vector3 IncrLayer(this Vector3 self, int incr) {
            self.z = self.z + (layerDelta * incr);
            return self;
        }

        public static int GetLayer(this Vector3 self) {
            return Mathf.RoundToInt(self.z * layerResolution);
        }

        public static Vector3 RotateBy2D(this Vector3 self, float angle) {
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            return q * self;
        }

        public static float Angle2D(this Vector3 self) {
            return Mathf.Atan2(self.y, self.x) * Mathf.Rad2Deg;
        }

        public static Vector3 MulPointwise(this Vector3 self, Vector3 o) {
            return new Vector3(self.x * o.x, self.y * o.y, self.z * o.z);
        }

        public static Vector3 DivPointwise(this Vector3 self, Vector3 o) {
            return new Vector3(self.x / o.x, self.y / o.y, self.z / o.z);
        }

        public static Vector3 SetX(this Vector3 self, float x) {
            return new Vector3(x, self.y, self.z);
        }

        public static Vector3 SetY(this Vector3 self, float y) {
            return new Vector3(self.x, y, self.z);
        }

        public static Vector3 SetZ(this Vector3 self, float z) {
            return new Vector3(self.x, self.y, z);
        }

        public static string ToJSON(this Vector3 self) {
            return "[" + self.x + ", " + self.y + ", " + self.z + "]";
        }

        public static Vector3 FromAngle(float angle) {
            // angle is in degrees
            return new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0);
        }

        public static Vector3 SnapTo45(this Vector3 self) {
            return new Vector3(Mathf.Sign(self.x), Mathf.Sign(self.y), Mathf.Sign(self.z)).normalized;
        }
    }

}