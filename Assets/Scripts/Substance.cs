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

}
