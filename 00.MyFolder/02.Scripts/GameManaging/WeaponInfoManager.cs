using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInfoManager : Singleton<WeaponInfoManager>{
    Weapon_Info weapon_Info;
    public void Init()
    {
        weapon_Info = Resources.Load("Sheet/Weapon_Info") as Weapon_Info;
    }
    public Weapon CreateWeapon(E_Weapon weaponType)
    {
        int weaponNum = (int)weaponType;
        Weapon_InfoData data = weapon_Info.dataArray[weaponNum];
        E_Weapon type = weaponType;
        string name = "";
        float plusCap = data.Pluscapability;
        float brakeProb = data.Brakeprob;
        switch (LanguageManager.GetInstance().Language)
        {
            case E_Language.KOREAN:
                name = data.Namekor;
                break;

            case E_Language.ENGLISH:
                name = data.Nameeng;
                break;
        }

        return new Weapon(type, name, plusCap, brakeProb);
    }
    public Weapon CreateWeapon(E_Weapon weaponType,bool broken)
    {
        int weaponNum = (int)weaponType;
        Weapon_InfoData data = weapon_Info.dataArray[weaponNum];
        E_Weapon type = weaponType;
        string name = "";
        float plusCap = data.Pluscapability;
        float brakeProb = data.Brakeprob;
        switch (LanguageManager.GetInstance().Language)
        {
            case E_Language.KOREAN:
                name = data.Namekor;
                break;

            case E_Language.ENGLISH:
                name = data.Nameeng;
                break;
        }

        return new Weapon(type, name, plusCap, brakeProb,broken);
    }
}
