using UnityEngine;
using System.Collections;

namespace SpryFox.Common {

    public static class RectTransformExtensions {
        public static Canvas GetRootCanvas(this RectTransform self) {
            Canvas canvas = null;
            while (true) {
                if (self == null) break;
                canvas = self.GetComponent<Canvas>();
                if (canvas != null) break;
                if (self.parent == null) break;
                self = self.parent as RectTransform;
            }
            return canvas;
        }
    }

}