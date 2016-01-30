using UnityEngine;

namespace SpryFox.Common {
    public static class GameObjectExtensions {
        
        // returns null if not found
        public static T FindInMeOrParent<T>(this Component self) where T: Component {
            return FindInMeOrParent<T>(self.gameObject);
        }

        // returns null if not found
        public static T FindInMeOrParent<T>(this GameObject self) where T: Component {
            
            var t = self.GetComponent<T>();
            if (t != null) {
                return t;
            } else if (self.transform.parent != null) {
                return self.transform.parent.gameObject.FindInMeOrParent<T>();
            } else {
                return null;
            }
        }

        // like FindInMeOrParent<>, but asserts if not found
        public static T GetInMeOrParent<T>(this Component self) where T: Component {
            return GetInMeOrParent<T>(self.gameObject);
        }

        // like FindInMeOrParent<>, but asserts if not found
        public static T GetInMeOrParent<T>(this GameObject self) where T: Component {
            var result = self.FindInMeOrParent<T>();
            Assert.IsNotNull(result,
                             "Could not find ", typeof(T), " near ", self);
            return result;
        }

        // because regular GetComponent<T>() doesn't work with interfaces.
        // returns null if not found
        public static I GetComponentWithInterface<I>(this MonoBehaviour self) 
            where I : class {

            return self.gameObject.GetComponentWithInterface<I>();
        }

        // because regular GetComponent<T>() doesn't work with interfaces.
        // returns null if not found
        public static I GetComponentWithInterface<I>(this GameObject self)
            where I : class {

            foreach(var behaviour in self.GetComponents<MonoBehaviour>()) {
                if (behaviour is I) {
                    return behaviour as I;
                }
            }
            return null;
        }

        public static void SetLayerOnChildren(this GameObject self, int layer) {
            self.layer = layer;
            var iter = self.transform.GetEnumerator();
            while (iter.MoveNext()) {
                var trf = (Transform)iter.Current;
                trf.gameObject.layer = layer;
                trf.gameObject.SetLayerOnChildren(layer);
            }
        }
    }
}

// ignore me