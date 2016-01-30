using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class TransformExtensions {
    /// <summary>
    /// Same as Transform.Find, but recurses through all children.
    /// </summary>
    public static Transform FindDescendant(this Transform self, string name) {
        if (self.name == name) return self;
        var enumerator = self.GetEnumerator();
        while (enumerator.MoveNext()) {
            var child = ((Transform)enumerator.Current).FindDescendant(name);
            if (child != null) return child;
        }
        return null;
    }

    /// <summary>
    /// Transform *b* from local space to world space.
    /// </summary>
    public static Bounds TransformBounds(this Transform self, Bounds b) {
        var result = new Bounds();
        result.Encapsulate(self.TransformPoint(b.min));
        result.Encapsulate(self.TransformPoint(b.max));
        result.Encapsulate(self.TransformPoint(new Vector3(b.min.x, b.min.y, b.max.z)));
        result.Encapsulate(self.TransformPoint(new Vector3(b.min.x, b.max.y, b.max.z)));
        result.Encapsulate(self.TransformPoint(new Vector3(b.max.x, b.min.y, b.max.z)));
        result.Encapsulate(self.TransformPoint(new Vector3(b.max.x, b.min.y, b.min.z)));
        result.Encapsulate(self.TransformPoint(new Vector3(b.max.x, b.max.y, b.min.z)));
        result.Encapsulate(self.TransformPoint(new Vector3(b.min.x, b.max.y, b.min.z)));
        return result;
    }

    /// <summary>
    /// Works like GetComponentsInChildren, returning all the children of type T in the hierarchy rooted at *root*.  
    /// This function orders them breadth-first.
    /// </summary>
    public static List<T> GetComponentsInChildrenBFS<T>(this Transform root) where T : Component {
        var result = new List<T>();
        GetComponentsInChildrenBFS<T>(root, result);
        return result;
    }

    // non-allocating
    public static void GetComponentsInChildrenBFS<T>(this Transform root, List<T> result) where T : Component {
        s_bfsQueue.Clear();
        s_bfsQueue.Enqueue(root);

        while (s_bfsQueue.Count > 0) {
            var node = s_bfsQueue.Dequeue();
            if(node == null) continue;
            result.AddRange(node.GetComponents<T>());
            var iter = node.GetEnumerator();
            while(iter.MoveNext()) {
                s_bfsQueue.Enqueue((Transform)iter.Current);
            }
        }
    }

    static Queue<Transform> s_bfsQueue = new Queue<Transform>();

    static List<Renderer> s_tmpBoundsRenderers = new List<Renderer>();
    public static Bounds GetFullRenderedBounds(this Transform trf) {
        Bounds fullBounds = new Bounds();
        bool initialized = false;
        s_tmpBoundsRenderers.Clear();
        trf.GetComponentsInChildrenBFS<Renderer>(s_tmpBoundsRenderers);
        for(int i = 0; i < s_tmpBoundsRenderers.Count; i++) {
            var rend = s_tmpBoundsRenderers[i];
            if(rend is TrailRenderer) continue;
            if(rend is ParticleSystemRenderer) continue;
            if(!initialized) {
                fullBounds = rend.bounds;
                initialized = true;
            } else {
                fullBounds.Encapsulate(rend.bounds);
            }
        }
        return fullBounds;
    }

    public static string GetNonNullName(this Transform trf) {
        if(string.IsNullOrEmpty(trf.name)) return "[unnamed]";
        return trf.name;
    }
}
