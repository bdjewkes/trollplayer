using UnityEngine;
using System.Collections;
using SpryFox.Common;

public class StationSubmit : Station {
    public GameObject[] FailureFX;

    protected override void Awake() {
        base.Awake();
        ShowFailureFX(false);
    }

    protected void ShowFailureFX(bool show) {
        foreach (var obj in FailureFX) {
            obj.SetActive(show);
        }
    }

    public override IEnumerator PerformAction(Substance substance) {
        yield return new WaitForSeconds(timeToReact);

        if (substance.State == constant) {
            Jot.Out("Submission successful!");
            ShowReactionFX(true);
        } else {
            ShowFailureFX(true);
        }
        
        yield return new WaitForSeconds(timeToPlayFX);

        ShowReactionFX(false);
        ShowFailureFX(false);
    }
}


