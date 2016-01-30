using UnityEngine;
using System.Collections;

public class Station : MonoBehaviour {
    float timeToComplete;

    int constant;

    public ParticleSystem[] FX;

    public void Awake() {
        foreach (var particle in FX) {
            particle.enableEmission = false;
        }
    }

    public float PerformAction(Substance substance) {
        foreach (var particle in FX) {
            particle.enableEmission = true;
        }
            
        return timeToComplete;
    }
}


