using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSMTransition
{
    protected FSMDecision decision;
    protected FSMState trueState;
    protected FSMState falseState;

    public FSMState TrueState
    {
        get { return trueState; }
    }
    public FSMState FalseState
    {
        get { return falseState; }
    }

    public virtual bool transCheck()
    {
        return decision.Decide();
    }
}
public class TriggerTransition : FSMTransition
{
    public TriggerTransition(FSMState state)
    {
        decision = new TriggerDecision();
        trueState = state;
        falseState = null;
    }
}
public class PlayerChoiceTransition : FSMTransition
{
    public PlayerChoiceTransition(FSMState trueState, FSMState falseState)
    {
        decision = new PlayerChoiceDecision();
        this.trueState = trueState;
        this.falseState = falseState;
    }
}
public class MyTransition : FSMTransition
{
    public MyTransition(System.Func<bool> decideCondition, FSMState trueState, FSMState falseState)
    {
        decision = new MyDecision(decideCondition);
        this.trueState = trueState;
        this.falseState = falseState;
    }
}