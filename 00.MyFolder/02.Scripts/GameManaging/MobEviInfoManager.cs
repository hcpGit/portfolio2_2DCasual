using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobEviInfoManager : Singleton<MobEviInfoManager> {
 
    Monster_Info mobInfo;
    Evidence_Info eviInfo;

   public void Init()
    {
        mobInfo = Resources.Load("Sheet/Monster_Info") as Monster_Info;
        eviInfo = Resources.Load("Sheet/Evidence_Info") as Evidence_Info;
    }

    public string GetMobName(E_Monster mob)
    {
        string result="";
        switch (LanguageManager.GetInstance().Language)
        {
            case E_Language.KOREAN:
                result = mobInfo.dataArray[(int)mob].Kor;
                break;

            case E_Language.ENGLISH:
                result = mobInfo.dataArray[(int)mob].Eng;
                break;
        }
        return result;
    }
    public string GetEvidenceName(E_Evidence evi)
    {
        string result = "";
        switch (LanguageManager.GetInstance().Language)
        {
            case E_Language.KOREAN:
                result = eviInfo.dataArray[(int)evi].Kor;
                break;

            case E_Language.ENGLISH:
                result = eviInfo.dataArray[(int)evi].Eng;
                break;
        }
        return result;
    }
    public int GetMobWeight(E_Monster mob)
    {
        return mobInfo.dataArray[(int)mob].Weight;
    }
    public int GetEviWeight(E_Evidence evi)
    {
        return eviInfo.dataArray[(int)evi].Weight;
    }
    public int GetMonsterShowUpDay(E_Monster mob)   //몬스터 출몰 날짜
    {
        return mobInfo.dataArray[(int)mob].Showupday;
    }
}
