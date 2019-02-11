using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMStateFactory<TEnum> where TEnum :  System.IConvertible ,  System.IComparable,  System.IFormattable  //enum 표현이 안되서.
{
    protected Dictionary<TEnum, FSMState> stateDic = new Dictionary<TEnum, FSMState>();

    public virtual void Init()
    {

    }
    protected virtual void MakeTransitions(FSMState state, TEnum e)
    {

    }

    protected virtual FSMState createState(TEnum e)
    {
        return null;
    }


    public FSMState GetState(TEnum e)
    {
        return stateDic[e];
    }
}
