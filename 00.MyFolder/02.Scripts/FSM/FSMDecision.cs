using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSMDecision
{
    protected System.Func<bool> decide;
    public virtual  bool Decide()
    {
        return decide();
    }
}
public class TriggerDecision : FSMDecision
{
    public override bool Decide()
    {
        return true;
    }
}
public class PlayerChoiceDecision : FSMDecision
{
    public PlayerChoiceDecision()
    {
        decide = () =>
        {
            return  EventParameterStorage.GetInstance().PlayerChoice;
        };
    }
}
public class MyDecision : FSMDecision
{
    public MyDecision(System.Func<bool> decide)
    {
        this.decide = decide;
    }
}
