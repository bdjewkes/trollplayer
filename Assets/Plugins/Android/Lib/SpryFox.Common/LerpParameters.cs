using System;
using UnityEngine;

namespace SpryFox.Common {
    
    // useful for darkconfig etc for storing lerp ranges 
    [Serializable]
    public class LerpParameters {
        
        // converts value to range 0-1 wrt inMin/inMax
        public float NormaliseIn(float value) {
            return Mathf.InverseLerp(InMin, InMax, value);
        }

        public float this[float valueToLerp] {
            get {
                float normValue = NormaliseIn(valueToLerp);
                float remappedValue = Mathf.Lerp(OutMin, OutMax, normValue);
                return remappedValue;
            }
        }

        //////////////////////////////////////////////////

        public float InMin = 0f;
        public float InMax = 1f;
        public float OutMin = 0f;
        public float OutMax = 1f;
    }
}