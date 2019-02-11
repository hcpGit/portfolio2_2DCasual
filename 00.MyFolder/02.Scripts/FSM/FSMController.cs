using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMController
{
    FSMState nowState;
    FSMState nextState;
    Stack<FSMState> stateStack = new Stack<FSMState>();

    public Stack<FSMState> GetStateStack()
    {
        return stateStack;
    }

    public void Setup(FSMState state)
    {
        if(stateStack==null) stateStack = new Stack<FSMState>();

        stateStack.Clear();
        stateStack.Push(state);

        nowState = state;
        nextState = null;
        nowState.BeginState();
        nowState.DoState();
    }

    public void UpdateFSM()
    {
        nowState.CheckTrans(this);

        if (nextState != null)
        {
            stateStack.Push(nextState);

            nowState.EndState();
            nowState = nextState;

            nextState = null;
            nowState.BeginState();
        }

        nowState.DoState();
        
    }

    public void SetNextState(FSMState state)
    {
        nextState = state;
    }
}
