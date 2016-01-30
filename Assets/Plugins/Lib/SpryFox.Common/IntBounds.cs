using UnityEngine;

namespace SpryFox.Common {

    [System.Serializable]
    public struct IntBounds {
        public Point p0;
        public Point p1;
        public Point Min {
            get { return p0; }
            set { p0 = value; }
        }
        public Point Max {
            get { return p1; }
            set { p1 = value; }
        }
        public IntBounds(Point p0, Point p1) {
            this.p0 = p0;
            this.p1 = p1;
            Assert.True(IsValid, "invalid bounds ", this);
        }
        public IntBounds(int xmin, int ymin, int xmax, int ymax) {
            this.p0 = new Point(xmin, ymin);
            this.p1 = new Point(xmax, ymax);
            Assert.True(IsValid, "invalid bounds ", this);
        }

        public bool IsValid {
            get {
                return Min.x <= Max.x && Min.y <= Max.y;
            }
        }

        public void Encapsulate(Point p) {
            if (p.x < p0.x) p0.x = p.x;
            if (p.y < p0.y) p0.y = p.y;
            if (p.x > p1.x) p1.x = p.x;
            if (p.y > p1.y) p1.y = p.y;
        }
        public void Expand(int i) {
            p0.x -= i;
            p0.y -= i;
            p1.x += i;
            p1.y += i;
        }
        public Point Size {
            get {
                return p1 - p0;
            }
        }
        public override string ToString() {
            return "IntBounds(" + p0.x + ", " + p0.y + ", " + p1.x + ", " + p1.y + ")";
        }

        public bool Contains(Point p) {
            var fromP0 = p - p0;
            var fromP1 = p - p1;
            return fromP0.x >= 0 && fromP0.y >= 0
                && fromP1.x <= 0 && fromP1.y <= 0;
        }

        // returns true if p >= min && p < max
        public bool ContainsIncExc(Point p) {
            return p.x >= p0.x && p.x < p1.x
                && p.y >= p0.y && p.y < p1.y;
        }

        public IntBounds TranslatedBy(Point translation) {
            return new IntBounds(p0 + translation, p1 + translation);
        }

        // returns IntBounds representing the overlapping area between these two bounds
        // returns null if no intersection
        public IntBounds? IntersectionWith(IntBounds other) {
            
            int minX = Mathf.Max(Min.x, other.Min.x);
            int maxX = Mathf.Min(Max.x, other.Max.x);
            if (maxX <= minX) {
                // horizontal seperation
                return null;
            }

            int minY = Mathf.Max(Min.y, other.Min.y);
            int maxY = Mathf.Min(Max.y, other.Max.y);
            if (maxY <= minY) {
                // vertical seperation
                return null;
            }

            return new IntBounds(minX, minY,
                                 maxX, maxY);
        }
    }
}
