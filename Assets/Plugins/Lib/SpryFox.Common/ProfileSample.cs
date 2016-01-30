using UnityEngine;

namespace SpryFox.Common {

    // easy way of profiling blocks, without needing a Profile.BeginSample/EndSample pair.
    // http://docs.unity3d.com/Documentation/ScriptReference/Profiler.BeginSample.html
    // using targetting sampling is better than a deep profile because that's reaaaaaly slow.
    // the Profiler calls compile out in release builds, so these become nops (and maybe the 
    // whole thing is optimised away?)
    struct ProfileSample : System.IDisposable {
        
        public ProfileSample(string sampleName) {
            Profiler.BeginSample(sampleName);
        }

        public void Dispose() {
            Profiler.EndSample();
        }
    }
}