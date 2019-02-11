using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : Singleton<PhaseManager> {
    //인가증이라던지, 업그레이드라던지, 그런 한번만 얻는 그런 애들을 저장함.
    
    Dictionary<E_Monster, bool> mobShowedUp = new Dictionary<E_Monster, bool>();    //몬스터 출몰여부 저장.
    public Dictionary<E_Monster, bool> MobShowedUp
    {
        get
        {
            return mobShowedUp;
        }
    }
    Dictionary<E_Interior, bool> interiorOpen = new Dictionary<E_Interior, bool>();    //인테리어 산 여부 저장
    public Dictionary<E_Interior, bool> InteriorOpen
    {
        get
        {
            return interiorOpen;
        }
    }



    Dictionary<E_Monster, bool> mobOpen = new Dictionary<E_Monster, bool>();    //허가증을 얻었냐의 여부임.
    public Dictionary<E_Monster, bool> MobOpen
    {
        get
        {
            return mobOpen;
        }
    }
    Dictionary<E_Evidence, bool> evidenceOpen = new Dictionary<E_Evidence, bool>();
    public Dictionary<E_Evidence, bool> EvidenceOpen
    {
        get
        {
            return evidenceOpen;
        }
    }
    Dictionary<E_Upgrade, bool> upgradeOpen = new Dictionary<E_Upgrade, bool>();
    public Dictionary<E_Upgrade, bool> UpgradeOpen
    {
        get
        {
            return upgradeOpen;
        }
    }
    public bool IsMobShowUP(E_Monster mob)
    {
        return mobShowedUp[mob];
    }
    public bool IsOpen(E_Monster mob)
    {
        return mobOpen[mob];
    }
    public bool IsOpen(E_Evidence evi)
    {
        return evidenceOpen[evi];
    }
    public bool IsOpen(E_Upgrade upgrade)
    {
        return upgradeOpen[upgrade];
    }
    public bool IsOpen(E_Interior inter)
    {
        return interiorOpen[inter];
    }

    public void InitForNewStart()
    {
        mobShowedUp = new Dictionary<E_Monster, bool>();
        for (int i = 0; i < (int)E_Monster.MAX; i++)
        {
            mobShowedUp.Add((E_Monster)i, false);
        }
        mobShowedUp[E_Monster.GOBLIN] = true;

        mobOpen = new Dictionary<E_Monster, bool>();
        for (int i = 0; i < (int)E_Monster.MAX; i++)
        {
            mobOpen.Add((E_Monster)i, false);
        }
        mobOpen[E_Monster.GOBLIN] = true;

        evidenceOpen = new Dictionary<E_Evidence, bool>();
        for (int i = 0; i < (int)E_Evidence.MAX; i++)
        {
            evidenceOpen.Add((E_Evidence)i, false);
        }
        evidenceOpen[E_Evidence.FINGER] = true;

        upgradeOpen = new Dictionary<E_Upgrade, bool>();
        for (int i = 0; i < (int)E_Upgrade.MAX; i++)
        {
            upgradeOpen.Add((E_Upgrade)i, false);
        }
        interiorOpen = new Dictionary<E_Interior, bool>();
        for (int i = 0; i < (int)E_Interior.MAX; i++)
        {
            interiorOpen.Add((E_Interior)i, false);
        }
    }

    public List<E_Monster> GetOpendMobDataList()
    {
        Dictionary<E_Monster, bool>.Enumerator enu = mobOpen.GetEnumerator();
        List<E_Monster> temp = new List<E_Monster>();
        while (enu.MoveNext())
        {
            if (enu.Current.Value)
            {
                temp.Add(enu.Current.Key);
            }
        }
        return temp;
    }
    public List<E_Evidence> GetOpendEviDataList()
    {
        Dictionary<E_Evidence, bool>.Enumerator enu = evidenceOpen.GetEnumerator();
        List<E_Evidence> temp = new List<E_Evidence>();
        while (enu.MoveNext())
        {
            if (enu.Current.Value)
            {
                temp.Add(enu.Current.Key);
            }
        }
        return temp;
    }
    public List<E_Upgrade> GetOpendUpgradeDataList()
    {
        Dictionary<E_Upgrade, bool>.Enumerator enu = upgradeOpen.GetEnumerator();
        List<E_Upgrade> temp = new List<E_Upgrade>();
        while (enu.MoveNext())
        {
            if (enu.Current.Value)
            {
                temp.Add(enu.Current.Key);
            }
        }
        return temp;
    }
    public void OpenMonsterCertificate(E_Monster mob)
    {
        mobOpen[mob] = true;
    }
    public void OpenEvidenceCertificate(E_Evidence evi)
    {
        evidenceOpen[evi] = true;
    }
    public void OpenUpgrade(E_Upgrade up)
    {
        upgradeOpen[up] = true;
    }
    public void OpenInterior(E_Interior inter)
    {
        interiorOpen[inter] = true;
    }

    public void MonsterShowUp(E_Monster mob)
    {
        mobShowedUp[mob] = true;
    }

    public void LogDebug()
    {
        string result = "출몰몹 [";
        for (int i = 0; i < (int)E_Monster.MAX; i++)
        {
            if (mobShowedUp[(E_Monster)i])
            {
                result += ((E_Monster)i).ToString()+",";
            }
        }

        result += "] 허가증 있는 몹 [";

        for (int i = 0; i < (int)E_Monster.MAX; i++)
        {
            if (mobOpen[(E_Monster)i])
            {
                result += ((E_Monster)i).ToString() + ",";
            }
        }
        result += "] 허가증 있는 증거품 [";

        for (int i = 0; i < (int)E_Evidence.MAX; i++)
        {
            if (evidenceOpen[(E_Evidence)i])
            {
                result += ((E_Evidence)i).ToString() + ",";
            }
        }
        
        result += "] 업그레이드 뚫은 목록 [";

        for (int i = 0; i < (int)E_Upgrade.MAX; i++)
        {
            if (upgradeOpen[(E_Upgrade)i])
            {
                result += ((E_Upgrade)i).ToString() + ",";
            }
        }

        result += "] 인테리어 뚫은 목록 [";

        for (int i = 0; i < (int)E_Interior.MAX; i++)
        {
            if (interiorOpen[(E_Interior)i])
            {
                result += ((E_Interior)i).ToString() + ",";
            }
        }
        Debug.Log(result+"]");
    }
    public void Load(SaveStructure st)
    {
        mobOpen = st.phaseMobOpen;
        evidenceOpen = st.phaseEvidenceOpen;
        upgradeOpen = st.phaseUpgradeOpen;
        interiorOpen = st.phaseInteriorOpen;
        mobShowedUp = st.phaseMobShowedUp;
    }
   
}
