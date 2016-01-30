using UnityEngine;
using System.Collections;
using SpryFox.Common;

public class Station : MonoBehaviour {
    public ActionEnum action;
    public int constant;

    public ParticleSystem[] FX;

    public float timeToReact = 0.5f;
    public float timeToPlayFX = 1;

    [SerializeField]
    int testSubstanceState;

    public void Awake() {
        TurnOff();
    }

    void TurnOff() {
        foreach (var particle in FX) {
            var em = particle.emission;
            em.enabled = false;
        }
    }

    public IEnumerator PerformAction(Substance substance) {
        yield return new WaitForSeconds(timeToReact);

        foreach (var particle in FX) {
            var em = particle.emission;
            em.enabled = true;
        }

        substance.Combine(action, new Substance(constant));

        yield return new WaitForSeconds(timeToPlayFX);

        TurnOff();
    }

    [ContextMenu("Test Perform Action")]
    void TestPerformAction() {
        var subs = new Substance(testSubstanceState);
        StartCoroutine(PerformAction(subs));

        this.Delay(timeToReact + 0.01f, () => {
            Jot.Out("Result is", subs);
        });
    }
}


