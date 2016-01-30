using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace SpryFox.Common {

    public static class MathPlus {
        public static Vector2 LineLineIntersection(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2) {
            float ay = a2.y - a1.y;
            float ax = a2.x - a1.x;

            float by = b2.y - b1.y;
            float bx = b2.x - b1.x;
            float d =  by * ax - ay * bx;
            if (d == 0) {
                return new Vector2(Mathf.Infinity, Mathf.Infinity);
            }
            float cx = b1.x - a1.x;
            float cy = b1.y - a1.y;
            float t = (cx * by - cy * bx) / d;

            return new Vector2(a1.x + t * ax, a1.y + t * ay);
        }

        public static float ProportionPtOnLine(Vector2 l1, Vector2 l2, Vector2 pt) {
            var lineDir = l2 - l1;
            if (Mathf.Abs(lineDir.x) > Mathf.Abs(lineDir.y)) {
                return (pt.x - l1.x) / lineDir.x;
            } else {
                return (pt.y - l1.y) / lineDir.y;
            }
        }

        public static float RaySegmentIntersection(Vector2 rayStart, Vector2 rayDir, Vector2 seg1, Vector2 seg2) {
            var lineInter = LineLineIntersection(rayStart, rayStart + rayDir, seg1, seg2);
            if (!(lineInter.x < Mathf.Infinity && lineInter.y < Mathf.Infinity)) {
                return Mathf.Infinity;
            }
            float segT = ProportionPtOnLine(seg1, seg2, lineInter);
            if (segT < 0 || segT > 1) return Mathf.Infinity;
            float rayT = ProportionPtOnLine(rayStart, rayStart + rayDir, lineInter);
            return rayT;
        }

        // if negative, makes more negative. if positive, makes more positive.
        public static float IncreaseMagnitude(this float val, float absoluteIncrease) {
            if (val < 0f) {
                return val - absoluteIncrease;
            }
            else {
                return val + absoluteIncrease;
            }
        }

        // projects value from one range to another range
        public static float MapValue(float value,
                                     float inMin, float inMax,
                                     float outMin, float outMax) {

            float t = Mathf.InverseLerp(inMin, inMax, value);
            float outValue = Mathf.Lerp(outMin, outMax, t);
            return outValue;        
        }

        // Constrain angle to [-180, 180)
        public static float NormalizeAngle(float angle) {
            while (angle >= 180) angle -= 360;
            while (angle < -180) angle += 360;
            return angle;
        }

        // Constrain angle to [0, 360)
        public static float NormalizeAngle0To360(float angle) {
            var normalized = NormalizeAngle(angle);
            if(normalized < 0) {
                normalized += 360;
            }
            if(normalized >= 360) {
                normalized -= 360;
            }
            return normalized;
        }

        public static int Wrap(int i, int cap) {
            while (i < 0) i = cap + i;
            i = i % cap;
            return i;
        }

        public static int CircularAdd(int a, int b, int cap) {
            return Wrap(a + b, cap);
        }

        public static int CircularDifference(int a, int b, int cap) {
            Assert.True(a >= 0 && b >= 0 && a < cap && b < cap, "Attempting to circular difference two indices that aren't within the cap", a, b, cap);
            if (a - b == -1 || a - b == 1 || a - b == 0) return a - b;
            if (a < b) {
                a += cap;
                return a - b;
            } else {
                b += cap;
                return a - b;
            }
        }

        public static int Clamp(int i, int min, int max) {
            if(i < min) return min;
            if(i > max) return max;
            return i;
        }

        public const long defaultFraction = 1024;
        public static float LimitFractional(float x, long fraction = defaultFraction) {
            return ((long)(x * fraction)) / (float)fraction;
        }

        public static int NextPowerOf2(int val) {
            // http://acius2.blogspot.com/2007/11/calculating-next-power-of-2.html
            val--;
            val = (val >> 1) | val;
            val = (val >> 2) | val;
            val = (val >> 4) | val;
            val = (val >> 8) | val;
            val = (val >> 16) | val;
            val++;
            return val;
        }

        public static float MovingAvg(float source, float newValue, float increment) {
            if (source == 0) source = newValue; // initial value
            return Mathf.Lerp(source, newValue, increment);
        }

        // this is a sample-rate-independent version of the MovingAvg function
        public static float MovingAvgInd(float source, float newValue, float increment, float tunedForTimeDelta, float actualTimeDelta) {
            increment = increment * actualTimeDelta / tunedForTimeDelta;
            if (source == 0) source = newValue; // initial value
            return Mathf.Lerp(source, newValue, increment);
        }

        public static Vector3 MovingSlerpInd(Vector3 source, Vector3 newValue, float increment, float tunedForTimeDelta, float actualTimeDelta) {
            increment = increment * actualTimeDelta / tunedForTimeDelta;
            return Vector3.Slerp(source, newValue, increment);
        }

        // returns how much two amounts disagree, as a fraction of the value of b
        public static float PctDifference(float a, float b) {
            return Mathf.Abs(a - b) / b;
        }


        public const double DEpsilon = 1e-45;
        public static bool Approximately(double a, double b) {
            if (a < b) {
                return (b - a) < DEpsilon;
            } else {
                return (a - b) < DEpsilon;
            }
        }

        public static void MeanAndStdev(IEnumerable<double> samples, out double mean, out double stdev) {
            // Welford
            mean = 0;
            double svar = 0;
            int n = 1;
            foreach (double d in samples) {
                double old_mean = mean;
                mean += (d - old_mean) / n;
                svar += (d - old_mean) * (d - mean);
                n++;
            }
            if (n > 1) {
                stdev = System.Math.Sqrt(svar / (n - 1));
            } else {
                stdev = 0;
            }
        }

        // note that the ulp/nextfloat/prevfloat functions only work well on "normal"
        // floats, which is to say nonzero noninfinite nonspecial
        public static float Ulp(float value) {
            return NextFloat(value) - value;
        }

        public static float NextFloat(float value) {
            long bits = System.BitConverter.ToInt32(System.BitConverter.GetBytes(value), 0);
            return System.BitConverter.ToSingle(System.BitConverter.GetBytes(bits + 1), 0);
        }

        public static float PrevFloat(float value) {
            long bits = System.BitConverter.ToInt32(System.BitConverter.GetBytes(value), 0);
            return System.BitConverter.ToSingle(System.BitConverter.GetBytes(bits - 1), 0);
        }

        // this function is faster than Mathf.FloorToInt
        public static int FastFloor(float x) {
            return (x > 0) ? ((int)x) : (((int)x) - 1);
        }

        // bilinearly interpolate vector2s
        public static Vector2 BilinearInterpolate(float t, float b, float l, float r,
                                    Vector2 valueTL, Vector2 valueTR, Vector2 valueBL, Vector2 valueBR,
                                    Vector2 samplePos) {
            Vector2 valueTS = (r - samplePos.x) / (r - l) * valueTL + (samplePos.x - l) / (r - l) * valueTR;
            Vector2 valueBS = (r - samplePos.x) / (r - l) * valueBL + (samplePos.x - l) / (r - l) * valueBR;
            Vector2 sampleValue = (b - samplePos.y) / (b - t) * valueTS + (samplePos.y - t) / (b - t) * valueBS;
            return sampleValue;
        }

        public static float RoundToMultiple(float multiple, float value) {
            if(multiple == 0) return value;
            return multiple * Mathf.Round(value/multiple);
        }
    }

    public struct LineSegment {
        public Vector3 S;
        public Vector3 E;
        public LineSegment(Vector3 s, Vector3 e) {
            S = s;
            E = e;
        }
    }
}