using UnityEngine;
using System.Collections;

namespace SpryFox.Common {
    public static class MonoBehaviourExtensions {
        public static void Delay(this MonoBehaviour self, float time, System.Action action) {
            self.StartCoroutine(DelayCoro(time, action));
        }

        static IEnumerator DelayCoro(float time, System.Action action) {
            yield return new WaitForSeconds(time);
            action();
        }
    }
}
