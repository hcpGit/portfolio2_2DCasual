using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSMState  //<TEnum> where TEnum : System.IConvertible, System.IComparable, System.IFormattable  //enum 표현이 안되서.
{
    FSMTransition[] trans= new FSMTransition[0];
    //TEnum thisEnumState;

    protected System.Action doAction;

    public virtual void BeginState() { }
    public virtual void DoState()
    {
        doAction();
    }
    public virtual void EndState() { }

    //public TEnum GetThisEnum(){ return thisEnumState; }


    public void AddTransitions(params FSMTransition[] tran)
    {
        trans = tran;
    }
    public void ClearTransitions()
    {
        trans = null;
    }

    public void CheckTrans(FSMController controller)
    {
        for (int i = 0; i < trans.Length; i++)
        {
            if (trans[i].transCheck())
            {
                if (trans[i].TrueState != null)
                {
                    controller.SetNextState(trans[i].TrueState);
                    break;
                }
            }
            else
            {
                if (trans[i].FalseState != null)
                {
                    controller.SetNextState(trans[i].FalseState);
                    break;
                }
            }
        }
    }
}

public class TalkState : FSMState
{
    public TalkState(string charaText , params string[] playerTexts)
    {
        doAction = () =>
        {
            InteractiveManager.GetInstance().ShowTalk(charaText , playerTexts);
        };
    }
}
public class MyState : FSMState
{
    public MyState(System.Action doAction)
    {
        this.doAction = doAction;
    }
}
