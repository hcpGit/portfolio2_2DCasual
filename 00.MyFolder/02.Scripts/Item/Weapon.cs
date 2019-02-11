using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Weapon {
    public Weapon(E_Weapon weaponType , string name, float plusCap , float brakeProb)
    {
        this.weaponType = weaponType;
        this.name = name;
        this.plusCapability = plusCap;
        this.brakeProb = brakeProb;
    }
    public Weapon(E_Weapon weaponType, string name, float plusCap, float brakeProb,bool broken)
    {
        this.weaponType = weaponType;
        this.name = name;
        this.plusCapability = plusCap;
        this.brakeProb = brakeProb;
        this.broken = broken;
    }
    [SerializeField]
    public E_Weapon weaponType;
    [SerializeField]
    bool broken = false;



    [System.NonSerialized]
    public string name;



    [System.NonSerialized]
    public float plusCapability;
    public float PlusCapability { get { return plusCapability; } }
    [System.NonSerialized]
    public float brakeProb;

    public void BrakeGamble()  
    {
        if (broken) return;

        if (Random.Range(0f, 100f) < brakeProb)
        {
            broken = true;
        }
    }

    //내구도 정해두기.
    public bool IsBroken()
    {
        return broken;
    }
    public override string ToString()
    {
        return "무기[" + weaponType.ToString() + "/" + name + "/" + plusCapability.ToString() + "/부러짐?=" + broken.ToString() + "확률=" + brakeProb.ToString();
    }
}
