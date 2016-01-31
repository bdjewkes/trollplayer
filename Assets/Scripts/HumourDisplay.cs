using UnityEngine;
using System.Collections;

public class HumourDisplay : MonoBehaviour {
    public GameObject[] onBits;

    [SerializeField]
    int testSubstanceState;

    void Awake() {
        DisplaySubstance(new Substance(0));
    }

    public void DisplaySubstance(Substance substance) {
        for (int i = 0; i < Substance.NUM_BITS; i++) {
            bool bitSet = ((1 << i) & substance.State) > 0;
            onBits[i].SetActive(bitSet);            
        }
    }

    [ContextMenu("Test Display Substance")]
    protected void TestDisplaySubstance() {
        var subs = new Substance(testSubstanceState);

        DisplaySubstance(subs);
    }
}
