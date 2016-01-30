using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SpryFox.Common
{
    public static class DebugPlus  {
        /// <summary>Draws an X at the location.  The color, size, and time
        /// displayed are configurable.  The X has a random tilt applied so
        /// that multiple ticks drawn at the same location are less likely to
        /// overlap.</summary>
        public static void DrawTick(Vector3 v, Color c, float size = 0.2f, float time = 0) {
            float wiggle = (Random.value - 0.5f) * 22;
            DrawTickA(v, c, size, time, wiggle);
        }

        public static void DrawTickA(Vector3 v, Color c, float size = 0.2f, float time = 0, float angle = 0) {
            Vector3 ur = Quaternion.AngleAxis(angle, Vector3.forward) * new Vector3(size, size, 0);
            Debug.DrawLine(v + ur, v - ur, c, time, false);
            Debug.DrawLine(v + ur.Left2D(), v + ur.Right2D(), c, time, false);
        }

        /// <summary>Draws a tilted square at the location.  The color, size, and time
        /// displayed are configurable.  The square has a random tilt applied so
        /// that multiple squares drawn at the same location are less likely to
        /// overlap.</summary>
        public static void DrawSquare(Vector3 v, Color c, float size = 0.2f, float time = 0) {
            float wiggle = 45 + (Random.value - 0.5f) * 22;
            DrawSquareA(v, c, size, time, wiggle);
        }

        public static void DrawSquareA(Vector3 v, Color c, float size = 0.2f, float time = 0, float angle = 0) {
            var rot = Quaternion.AngleAxis(angle, Vector3.forward);
            Vector3 ur = rot * new Vector3(size, size, 0);
            Vector3 ul = ur.Left2D();
            Vector3 bl = -ur;
            Vector3 br = -ul;
            Debug.DrawLine(v + ul, v + ur, c, time, false);
            Debug.DrawLine(v + ur, v + br, c, time, false);
            Debug.DrawLine(v + br, v + bl, c, time, false);
            Debug.DrawLine(v + bl, v + ul, c, time, false);
        }

        public static void DrawCross(Vector2 worldPos, Color col, float radius = 0.2f, 
                                     float time = 0f) {
            
            var upRight = (Vector2.up + Vector2.right).normalized * radius;
            var downRight = upRight.Right2D();

            var corners = new Vector2[] { 
                upRight,
                downRight,
                -upRight, 
                -downRight,
            };

            foreach(var corner in corners)
            {
                Debug.DrawRay(worldPos, corner, col, time, false);
            }            
        }

        /// <summary>Draws an arrow (with a little arrowhead) in the same
        /// manner as a ray.  The *size* parameter controls the size of the
        /// arrowhead.</summary>
        public static void DrawArrow(Vector3 pos, Vector3 dir, Color c, float size = 0.2f, float time = 0) {
            Debug.DrawRay(pos, dir, c, time, false);
            Vector3 leftDir = Quaternion.AngleAxis(45, Vector3.forward) * dir.normalized.Left2D() * size;
            Debug.DrawLine(pos + dir + leftDir, pos + dir, c, time, false);
            Debug.DrawLine(pos + dir + leftDir.Left2D(), pos + dir, c, time, false);
        }

        /// <summary>Draws an arrow (with a square for an arrowhead) in the same
        /// manner as a ray.  The *size* parameter controls the size of the
        /// arrowhead.</summary>
        public static void DrawSquareArrow(Vector3 pos, Vector3 dir, Color c, float size = 0.2f, float time = 0) {
            Vector3 n = dir.normalized * size;
            Vector3 w = n.Left2D();
            Vector3 e = -w;
            Vector3 s = -n;
            Vector3 v = pos + dir;
            Debug.DrawRay(pos, dir - n, c, time, false);
            Debug.DrawLine(v + n, v + e, c, time, false);
            Debug.DrawLine(v + e, v + s, c, time, false);
            Debug.DrawLine(v + s, v + w, c, time, false);
            Debug.DrawLine(v + w, v + n, c, time, false);
        }
	
        /// <summary>Draws a rectangle.  Since Rects are 2D, Z is 0.</summary>
        public static void DrawRect(Rect r, Color c, float time = 0) {
            Debug.DrawLine(new Vector3(r.x, r.y, 0), new Vector3(r.x + r.width, r.y), c, time, false);
            Debug.DrawLine(new Vector3(r.x, r.y, 0), new Vector3(r.x, r.y + r.height), c, time, false);
            Debug.DrawLine(new Vector3(r.x + r.width, r.y), new Vector3(r.x + r.width, r.y + r.height), c, time, false);
            Debug.DrawLine(new Vector3(r.x, r.y + r.height), new Vector3(r.x + r.width, r.y + r.height), c, time, false);
        }

        /// <summary>Draws a 3D cube around the Bounds object.</summary>
        public static void DrawBounds(Bounds b, Color c, float time = 0) {
            var ulf = new Vector3(b.min.x, b.max.y, b.min.z);
            var ulb = new Vector3(b.min.x, b.max.y, b.max.z);
            var urf = new Vector3(b.max.x, b.max.y, b.min.z);
            var urb = new Vector3(b.max.x, b.max.y, b.max.z);
            var blf = new Vector3(b.min.x, b.min.y, b.min.z);
            var blb = new Vector3(b.min.x, b.min.y, b.max.z);
            var brf = new Vector3(b.max.x, b.min.y, b.min.z);
            var brb = new Vector3(b.max.x, b.min.y, b.max.z);
            Debug.DrawLine(ulf, urf, c, time);
            Debug.DrawLine(ulf, blf, c, time);
            Debug.DrawLine(ulf, ulb, c, time);

            Debug.DrawLine(brb, brf, c, time);
            Debug.DrawLine(brb, urb, c, time);
            Debug.DrawLine(brb, blb, c, time);

            Debug.DrawLine(urb, urf, c, time);
            Debug.DrawLine(urb, ulb, c, time);
            Debug.DrawLine(blf, blb, c, time);
            Debug.DrawLine(blf, brf, c, time);
            Debug.DrawLine(ulb, blb, c, time);
            Debug.DrawLine(urf, brf, c, time);
        }

        /// <summary>Draws a circle at a position, as a series of line
        /// segments.  The *radius* controls the radius, and *segments*
        /// controls how many line segments are drawn.  Fewer than 10 looks
        /// pretty rough.</summary>
        public static void DrawCircle(Vector3 center, 
                                      float radius, 
                                      Color color, 
                                      int segments = 20, 
                                      float time = 0) {

            float angleCoefficent = Mathf.PI * 2 / segments;
            for (int i = 0; i < segments; i++) {
                float angle = (i) * angleCoefficent;
                Vector3 pt = 
                    new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius 
                    + center;

                float nextAngle = (i + 1) * angleCoefficent;
                Vector3 next = 
                    new Vector3(Mathf.Cos(nextAngle), Mathf.Sin(nextAngle), 0) * radius 
                    + center;

                Debug.DrawLine(pt, next, color, time);
            }
        }

        public static void DrawArc(Vector3 center, 
                                      float radius,
                                      float angleA,
                                      float angleB,
                                      Color color, 
                                      int segments = 20, 
                                      float time = 0) {
            angleA = Mathf.Deg2Rad * angleA;
            angleB = Mathf.Deg2Rad * angleB;
            float angleCoefficent = (angleB - angleA) / segments;
            for (int i = 0; i < segments; i++) {
                float angle = (i) * angleCoefficent + angleA;
                Vector3 pt = 
                    new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius 
                    + center;

                float nextAngle = (i + 1) * angleCoefficent + angleA;
                Vector3 next = 
                    new Vector3(Mathf.Cos(nextAngle), Mathf.Sin(nextAngle), 0) * radius 
                    + center;

                Debug.DrawLine(pt, next, color, time);
            }
        }
	
        /// <summary>Draws a "hull", which is a closed series of vertices.
        /// The *len* parameter allows you to specify a subset of the input
        /// collection.  *Offset* is a constant addition to the positions drawn
        /// <(useful if you have multiple hulls in the same place), and
        /// *displacement* is an increasing value that is added to the
        /// positions of the vertices (useful to see whether the vertices
        /// overlap themselves).</summary>
        
        public static void DrawHull(IEnumerable<Vector3> hull, int len, Vector3? offset = null, Color? color = null, float duration = 0, Vector3? displacement = null) {
            if(offset == null) {
                offset = new Vector3(0,0,0);
            }
            if(color == null) {
                color = new Color(0, 0, 1, 1);
            }
            if (displacement == null) displacement = new Vector3(0, 0, 0);
            if(hull == null) { return; }
            bool first = true;
            Vector3 firstV = Vector3.zero;
            Vector3 prev = Vector3.zero;
            IEnumerator<Vector3> iter = hull.GetEnumerator();
            for(int i = 0; i < len; i++) {
                iter.MoveNext();
                Vector3 vert = iter.Current;
                if(first) { 
                    prev = vert;
                    firstV = vert;
                    first = false;
                } else {
                    float t = ((float)i - 1) / len;
                    float nt = ((float)i) / len;
                    Debug.DrawLine(displacement.Value * t + prev + offset.Value,
                                   displacement.Value * nt + vert + offset.Value,
                                   color.Value, duration);
                    prev = vert;
                }
            }
            Debug.DrawLine(displacement.Value + firstV + offset.Value, 
                           (len - 1f)/len * displacement.Value + prev + offset.Value, color.Value, duration);
        }

        /// <summary>Draw a list of line segments.  It also draws a small tick
        /// at each vertex.  The z coordinate can be specified for the entire
        /// drawing</summary>
        public static void DrawLineSegments(List<Vector2> pts, Color color, float time = 0, float z = 0) {
            Vector2 prev = pts[0];
            for (int i = 1; i < pts.Count; i++) {
                Debug.DrawLine(prev.XYZ().SetZ(z), pts[i].XYZ().SetZ(z), color, time);
                prev = pts[i];
            }
            foreach (var pt in pts) {
                DebugPlus.DrawTick(pt, color, 0.005f, time);
            }
        }

        /// <summary>Does a vaguely reasonable job of drawing a mesh.  It's
        /// not completely correct.  The color can vary from first triangle
        /// to the last, and an increasing displacement can find otherwise
        /// invisible overlaps.</summary>
        public static void DrawMesh(Mesh m, Vector3? offset = null, Color? start = null, Color? end = null, float time = 0, Vector3? displacement = null) {
            if(offset == null) offset = Vector3.zero;
            if(start == null) start = new Color(0.5f, 0.5f, 1f, 1f);
            if(end == null) end = new Color(1f, 0.5f, 0.5f, 1f);
            if (displacement == null) displacement = new Vector3(0, 0, 0);

            Vector3[] vertices = m.vertices;
            int[] triangles = m.triangles;
            for(int i = 0; i < triangles.Length - 1; i++) {
                float t = ((float)i)/triangles.Length;
                float nt = ((float)i + 1) / triangles.Length;
                Debug.DrawLine(displacement.Value * t + offset.Value + vertices[triangles[i]],
                               displacement.Value * nt + offset.Value + vertices[triangles[i + 1]],
                               Color.Lerp(start.Value, end.Value, t),
                               time);
            }
        }

        /// <summary>Draws a list of vertices triangle-by-triangle (i.e., the
        /// way that it would be rendered as a mesh).  The color can vary from
        /// first triangle to the last.</summary>
        public static void DrawTris(List<Vector2> verts, List<int> tris, Color start, Color end, float time) {
            float shrinkBy = 0.02f;
            float totalTris = Mathf.Floor(tris.Count/3);
            for(int i = 0; i < totalTris; i++) {
                var t = i / totalTris;
                var color = Color.Lerp(start, end, t);
                var off = i * 3;
                var a = verts[tris[off]];
                var b = verts[tris[off + 1]];
                var c = verts[tris[off + 2]];
                var centroid = (a + b + c)/3;
                a = a + (centroid - a).normalized * shrinkBy;
                b = b + (centroid - b).normalized * shrinkBy;
                c = c + (centroid - c).normalized * shrinkBy;
                Debug.DrawLine(a, b, color, time);
                Debug.DrawLine(b, c, color, time);
                Debug.DrawLine(c, a, color, time);
            }
        }

        /// <summary>Draws a single triangle from a list of vertices.  The
        /// color can vary based on position in the list.  *whichTri* is the
        /// index of the triangle in the list, so it should be proportional 
        /// to tris.Count/3.</summary>
        public static void DrawSingleTri(List<Vector2> verts, List<int> tris, int whichTri, Color start, Color end, float time) {
            float totalTris = Mathf.Floor(tris.Count/3);
            whichTri = whichTri % Mathf.RoundToInt(totalTris);
            for(int i = 0; i < totalTris; i++) {
                if(i != whichTri) continue;
                var t = i / totalTris;
                var color = Color.Lerp(start, end, t);
                var off = i * 3;
                Debug.DrawLine(verts[tris[off]], verts[tris[off + 1]], color, time);
                Debug.DrawLine(verts[tris[off + 1]], verts[tris[off + 2]], color, time);
                Debug.DrawLine(verts[tris[off + 2]], verts[tris[off]], color, time);
            }
        }

        public static void DrawCapsule(Vector2 start, float radius, Vector2 dir, Color color, int numSegments = 20, float time = 0) {
            var angle = dir.Angle2D();
            DrawArc(start, radius, angle + 90, angle + 270, color, numSegments, time);
            DrawArc(start + dir, radius, angle + 90, angle - 90, color, numSegments, time);

            var leftDisplacement = dir.Left2D().normalized * radius;
            Debug.DrawLine(start + leftDisplacement, start + dir + leftDisplacement, color, time);
            Debug.DrawLine(start - leftDisplacement, start + dir - leftDisplacement, color, time);
        }
    }
}