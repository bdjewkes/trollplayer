using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationRunner
{
    public bool running
    {
        get;
        private set;
    }

    public IEnumerator RunAnimation(float animationTime, System.Action<float> action)
    {
        running = true;
        float t = 0;
        while (t < animationTime && running)
        {
            float normalizedTime = t / animationTime;
            action(normalizedTime);
            yield return null;
            t += Time.deltaTime;
        }
        running = false;
    }

    public AnimationRunner()
    {
        running = false;
    }
    public void Cancel()
    {
        running = false;
    }
}