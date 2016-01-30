using UnityEngine;
using System.Collections;

public enum ActionEnum {
    ADD,
    SUB,
    XOR,
    OR,
    AND,
    NOT
}


public class Substance {
    int state;

    public Substance(int initialState) {
        state = initialState;
    }

    public void Combine(ActionEnum action, Substance secondSubstance) {
        switch (action) {
            case ActionEnum.ADD: state = state + secondSubstance.state; break;
            case ActionEnum.SUB: state = state - secondSubstance.state; break;
            case ActionEnum.XOR: state = state ^ secondSubstance.state; break;
            case ActionEnum.OR: state = state | secondSubstance.state; break;
            case ActionEnum.AND: state = state & secondSubstance.state; break;
            case ActionEnum.NOT: state = ~state; break;
        }
    }

    public override string ToString() {
        return string.Format("Substance({0}, 0b{1})", state, System.Convert.ToString(state, 2));
    }
}
