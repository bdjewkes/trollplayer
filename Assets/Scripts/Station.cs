using UnityEngine;
using System.Collections;
using SpryFox.Common;

public class Station : MonoBehaviour {
    public ActionEnum action;
    public int constant;

    public ParticleSystem[] FX;

    public GameObject[] MeasurementFX;

    public float timeToReact = 0.5f;
    public float timeToPlayFX = 1;

    [SerializeField]
    int testSubstanceState;

    public bool[] measuresBits = new bool[4];

    public void Awake() {
        TurnOffReactionFX();
        TurnOffMeasurementFX();
    }

    void TurnOffReactionFX() {
        foreach (var particle in FX) {
            var em = particle.emission;
            em.enabled = false;
        }
    }

    void TurnOffMeasurementFX() {
        foreach (var obj in MeasurementFX) {
            obj.SetActive(false);
        }
    }

    public virtual IEnumerator PerformAction(Substance substance) {
        yield return new WaitForSeconds(timeToReact);

        foreach (var particle in FX) {
            var em = particle.emission;
            em.enabled = true;
        }

        substance.Combine(action, new Substance(constant));

        yield return new WaitForSeconds(timeToPlayFX);

        TurnOffReactionFX();

        yield return StartCoroutine(Measure(substance));   
    }

    public IEnumerator Measure(Substance substance) {
        for (int i = 0; i < 4; i++) {
            if (measuresBits[i]) {
                bool bitSet = ((1 << i) & substance.State) > 0;
                Jot.Out("Measured bit", i, bitSet);
                if (bitSet) {
                    MeasurementFX[i].SetActive(true);
                }
            }
        }
        yield return new WaitForSeconds(1f);

        TurnOffMeasurementFX();
        Jot.Out("Done measuring");
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


