using UnityEngine;
using System.Collections;

public enum ActionEnum {
    ADD,
    SUB,
    XOR,
    OR,
    AND,
    NOT,
    NOP
}

[System.Serializable]
public class Substance {
    public int state;
	private int _initialState;
    public int State { get { return state; } }

    public const int NUM_BITS = 4;

    public const int STATE_MASK = 0xF;

    public Substance(int initialState) {
        state = initialState;
	    _initialState = initialState;
    }

    public void Clear() {
        state = 0;
    }

	public void ResetState()
	{
		state = _initialState;
	}

    public void Combine(ActionEnum action, Substance secondSubstance) {
        switch (action) {
            case ActionEnum.ADD: state = state + secondSubstance.state; break;
            case ActionEnum.SUB: state = state - secondSubstance.state; break;
            case ActionEnum.XOR: state = state ^ secondSubstance.state; break;
            case ActionEnum.OR: state = state | secondSubstance.state; break;
            case ActionEnum.AND: state = state & secondSubstance.state; break;
            case ActionEnum.NOT: state = ~state; break;
            case ActionEnum.NOP: break;
        }
        state = state & STATE_MASK;
    }

    public override string ToString() {
        return string.Format("Substance({0}, 0b{1})", state, System.Convert.ToString(state, 2));
    }
}
