using UnityEngine;
using System.Collections.Generic;
using System;
using DarkConfig;

namespace SpryFox.Common {
    [System.Serializable]
    public struct Point : IEquatable<Point> {

        // sometimes good as an error code
        public readonly static Point negative = new Point(-1,-1); 
        public readonly static Point zero = new Point(0, 0);
        public readonly static Point one = new Point(1, 1);
        public readonly static Point up = new Point(0,1);
        public readonly static Point right = new Point(1,0);

        public int x;
        public int y;
        public Point(int v) {
            x = v;
            y = v;
        }
        public Point(int x, int y) {
            this.x = x;
            this.y = y;
        }
        public Point(Vector2 v) {
            this.x = MathPlus.FastFloor(v.x);
            this.y = MathPlus.FastFloor(v.y);
        }
        public Point(Point p) {
            this.x = p.x;
            this.y = p.y;
        }

        public int Manhattan { get { return Mathf.Abs(x) + Mathf.Abs(y); } }
        public float Magnitude { get { return Mathf.Sqrt(SqrMagnitude); } }
        public int SqrMagnitude { get { return x * x + y * y; } }

        // returns magnitude of largest Abs(component)
        public int LargestComponent { 
            get { return Mathf.Abs(x) > Mathf.Abs(y) ? Mathf.Abs(x) : Mathf.Abs(y); } 
        }

        public Point Right() {
            return new Point(y, -x);
        }

        public Point InvertY() {
            return new Point(x, -y);
        }

        public static Point FromVector3(Vector3 v) {
            return new Point((int)v.x, (int)v.y);
        }
        public Vector3 ToVector3() {
            return new Vector3(x, y, 0);
        }
        public static implicit operator Vector2(Point p) {
            return new Vector2(p.x, p.y);
        }
        public override string ToString() {
            return "P(" + x + ", " + y + ")";
        }
        public static bool operator ==(Point a, Point b) {
            return (a.x == b.x) && (a.y == b.y);
        }
        public static bool operator !=(Point a, Point b) {
            return (a.x != b.x) || (a.y != b.y);
        }
        public static Point operator-(Point toNegate) {
            return new Point(-toNegate.x, -toNegate.y);            
        }
        public static Point operator +(Point a, Point b) {
            return new Point(a.x + b.x, a.y + b.y);
        }
        public static Point operator -(Point a, Point b) {
            return new Point(a.x - b.x, a.y - b.y);
        }
        public static Point operator /(Point a, int d) {
            return new Point(a.x/d, a.y/d);
        }
        public static Point operator *(Point a, int d) {
            return new Point(a.x*d, a.y*d);
        }

        public Point MulPointwise(Point b) {
            return new Point(x * b.x, y * b.y);
        }
        
        public bool Equals(Point other)  {
            return this == other;
        }

        public override bool Equals(object obj) {
            if (base.Equals(obj)) return true;
            if (obj is Point) {
                return Equals((Point)obj);
            } else {
                return false;
            }
        }
        public override int GetHashCode() {
            return (x << 16 | (y & 0xFFFF)) + (y >> 16 | x & unchecked((int)0xFFFF0000));
        }
        public Point Wrap(int capx, int capy) {
            return new Point(MathPlus.Wrap(x, capx), MathPlus.Wrap(y, capy));
        }
        public static Point FromDoc(Point existing, DarkConfig.DocNode doc) {
            int p1 = System.Convert.ToInt32(doc[0].StringValue);
            int p2 = p1;
            if (doc.Count >= 2) {
                p2 = System.Convert.ToInt32(doc[1].StringValue);
            }
            return new Point(p1, p2);
        }
    }

    public class PointComparer : IEqualityComparer<Point> {
       public bool Equals(Point p1, Point p2) {
          return p1.Equals(p2);
       }
     
       public int GetHashCode(Point p) {
          return p.GetHashCode();
       }
    }
}