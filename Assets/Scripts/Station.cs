using UnityEngine;
using System.Collections;
using SpryFox.Common;

public class Station : MonoBehaviour {
    public ActionEnum action;
    public int constant;

    public ParticleSystem[] FX;

    public Animator[] Animations;

    public GameObject[] MeasurementFX;

    public float timeToReact = 0.5f;
    public float timeToPlayFX = 1;
    
    public bool[] measuresBits = new bool[4];

    [SerializeField]
    protected int testSubstanceState;

    protected virtual void Awake() {
        ShowReactionFX(false);
        TurnOffMeasurementFX();
    }

    protected void ShowReactionFX(bool show) {
        foreach (var particle in FX) {
            var em = particle.emission;
            em.enabled = show;
        }

        if (show) {
            foreach (var anim in Animations) {
                if (anim == null) continue;
                anim.SetTrigger("Activate");
            }
        }
    }

    protected void TurnOffMeasurementFX() {
        foreach (var obj in MeasurementFX) {
            obj.SetActive(false);
        }
    }

    public virtual IEnumerator PerformAction(Substance substance) {
        yield return new WaitForSeconds(timeToReact);

        ShowReactionFX(true);

        substance.Combine(action, new Substance(constant));

        yield return new WaitForSeconds(timeToPlayFX);

        ShowReactionFX(false);

        yield return StartCoroutine(Measure(substance));   
    }

    public IEnumerator Measure(Substance substance) {
        for (int i = 0; i < 4; i++) {
            if (measuresBits[i]) {
                bool bitSet = ((1 << i) & substance.State) > 0;
                Jot.Out("Measured bit", i, bitSet);
                if (bitSet && MeasurementFX.Length > i && MeasurementFX[i]) {
                    MeasurementFX[i].SetActive(true);
                }
            }
        }
        yield return new WaitForSeconds(1f);

        TurnOffMeasurementFX();
        Jot.Out("Done measuring");
    }

    [ContextMenu("Test Perform Action")]
    protected void TestPerformAction() {
        var subs = new Substance(testSubstanceState);
        StartCoroutine(PerformAction(subs));

        this.Delay(timeToReact + 0.01f, () => {
            Jot.Out("Result is", subs);
        });
    }
}


