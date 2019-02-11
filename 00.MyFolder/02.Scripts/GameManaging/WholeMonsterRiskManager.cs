using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WholeMonsterRiskManager : Singleton<WholeMonsterRiskManager>
{
    [System.Serializable]
    public class MonsterRisk
    {
        [SerializeField]
        public E_Monster mob;
        [SerializeField]
        public int nowNum;
        public MonsterRisk(E_Monster mob)
        {
            this.mob = mob;
            nowNum = 0;
        }
        public MonsterRisk(E_Monster mob,int num)
        {
            this.mob = mob;
            nowNum = num;
        }
    }

    List<MonsterRisk> monsterRiskList;
    public List<MonsterRisk> MonsterRiskList
    {
        get { return monsterRiskList; }
    }
    List<HunterIdea> huntersWhoHaveQuest;
    public List<HunterIdea> HuntersWhohaveQuest
    {
        get {
            return huntersWhoHaveQuest;
        }
    }

    public bool IsThreatHitsMax()
    {
        for (int i = 0; i < (int)E_Monster.MAX; i++)
        {
            if (monsterRiskList[i].nowNum >= maxThreatDic[(E_Monster)i])
                return true;
        }
        return false;
    }


    Dictionary<E_Monster, int> maxThreatDic = new Dictionary<E_Monster, int>(); //몬스터 별 최대 위험 숫자를 표시함.
    public int GetMaxMobThreat(E_Monster mob)
    {
        return maxThreatDic[mob];
    }
    public void Init()
    {
        monsterRiskList = new List<MonsterRisk>();
        huntersWhoHaveQuest = new List<HunterIdea>();

        maxThreatDic.Clear();
        MaxMonsterThreat mt = Resources.Load("Sheet/MaxMonsterThreat") as MaxMonsterThreat;   //나중에 바꿔주기.
        for (int i = 0; i < mt.dataArray.Length; i++)
        {
            maxThreatDic.Add((E_Monster)mt.dataArray[i].Mob, mt.dataArray[i].Maxthreatnumber);
        }

        for (int i = 0; i < (int)E_Monster.MAX; i++)
        {
            monsterRiskList.Add(new MonsterRisk((E_Monster)i));
        }
        
    }

    public List<MonsterRisk> IncreaseThreat()
    {
        //Debug.Log("밤 인크리스 뜨리트");
        List<MonsterRisk> increaseAtNight = new List<MonsterRisk>();
        //밤에 사냥감 들의 수를 올림/
        //결국 원하는 건. 밤에 정산 할때 몹 전체 위협도 그래프가
        //줄어들었다가
        //다시 올라가는 효과를 원해서 이 짓을 하고 있다.
        for (int i = 0; i < (int)E_Monster.MAX; i++)    //출몰이랑 섞어서.
        {
            if ( false == PhaseManager.GetInstance().IsMobShowUP((E_Monster)i)) //출몰 안한 몹이면 몹 젠에서 패쓰
            {
                increaseAtNight.Add(new MonsterRisk((E_Monster)i));
                continue; 
            }
            int rand = Random.Range(7, 12);
            int increaseNum = maxThreatDic[(E_Monster)i] / rand;

            if (monsterRiskList[i].nowNum + increaseNum > maxThreatDic[(E_Monster)i])
            {
                increaseNum = maxThreatDic[(E_Monster)i] - monsterRiskList[i].nowNum;
            }
            
            increaseAtNight.Add(new MonsterRisk( (E_Monster)i, increaseNum));
            Debug.Log("위험도 추가" + ((E_Monster)i).ToString() + "몇 "+increaseNum);
        }
        IADMRList(increaseAtNight, true);
        return increaseAtNight;
    }

    public List<MonsterRisk> DecreaseThreat(List<QuestPerMob> tonightHunted)
    {
        //헌터가 잡아서 줄여주는 효과 띄우기!!! 애니메이션으로!!!!
        //애니메이션 효과는 그냥 ui단으로 뱄음.
        //증거품마다 몹을 또 잡는거임! 고블린 손가락 고블린 이빨 이렇게 잡아도 고블린은 결국 2명을 잡는거야!!
        List<MonsterRisk> huntedMobList =  MobRiskListByQPMList(tonightHunted);
        IADMRList(huntedMobList, false);
        
        return huntedMobList;
        
    }

    public void AddHunter(HunterIdea hunter)
    {
        if (hunter == null) Debug.LogError("헌터가 없음.");
        if(QuestManager.GetInstance().GetQuest(hunter.associatedQuestKey)==null) Debug.LogError("연관된 퀘스트가 존재하지 않음.");
        huntersWhoHaveQuest.Add(hunter);
    }

    public List<QuestPerMob> LetsHuntingAtEndOfDay()
    {
        List<QuestPerMob> tonightHunted = new List<QuestPerMob>();
     
        int idx = 0;
        HunterIdea hunter;

        while (idx < huntersWhoHaveQuest.Count)
        {
            bool isLastDayHunt = false;
            hunter = huntersWhoHaveQuest[idx];
            List<QuestPerMob> hunterHunt = hunter.HuntingAtNight(out isLastDayHunt);

            tonightHunted.AddQPMList(hunterHunt); //헌팅앳나이트 함수가 다 알아서 처리해주고 오늘 밤에 이 헌터가 잡은 애들만 딱 돌려줌.

            
            if (isLastDayHunt)
            {
                Debug.Log(idx + "번째 헌터 " + hunter.CharaName + "삭제 , 사냥한 리스트는 " + hunterHunt.DebugString());
                huntersWhoHaveQuest.RemoveAt(idx);
                continue;
            }
            else
            {
                idx++;
            }
        }
        Debug.Log(tonightHunted.DebugString() + "d으로 헌팅앳 나이트 종료.");
        return tonightHunted;
    }

    List<MonsterRisk> MobRiskListByQPMList(List<QuestPerMob> list)
    {
        List<MonsterRisk> temp = new List<MonsterRisk>();
        for (int i = 0; i < (int)E_Monster.MAX; i++)
        {
            temp.Add(new MonsterRisk((E_Monster)i));
        }

        for (int i = 0; i < list.Count; i++)
        {
            E_Monster mob = list[i].mob;
            temp[(int)mob].nowNum += list[i].number;
        }
        return temp;
    }

    void IADMRList(List<MonsterRisk> subject, bool inc)
    {
        Debug.Log("위협도 변경 전 "+DebugString());
        if (inc)
        {
            for (int i = 0; i < (int)E_Monster.MAX; i++)
            {
                monsterRiskList[i].nowNum += subject[i].nowNum;
            }
        }
        else {
            for (int i = 0; i < (int)E_Monster.MAX; i++)
            {
                int nowNumber = monsterRiskList[i].nowNum - subject[i].nowNum;
                if (nowNumber < 0) nowNumber = 0;
                monsterRiskList[i].nowNum = nowNumber;
            }

        }
        Debug.Log("위협도 변경 후 " + DebugString());
    }

    public void Load(SaveStructure st)
    {
        maxThreatDic.Clear();
        MaxMonsterThreat mt = Resources.Load("Sheet/MaxMonsterThreat") as MaxMonsterThreat;  
        for (int i = 0; i < mt.dataArray.Length; i++)
        {
            maxThreatDic.Add((E_Monster)mt.dataArray[i].Mob, mt.dataArray[i].Maxthreatnumber);
        }


        monsterRiskList = st.threatState;
        huntersWhoHaveQuest = st.threatHunters;
    }
    public string DebugString()
    {
        string result="몬스터 위협도 상황 ";
        for (int i = 0; i < (int)E_Monster.MAX; i++)
        {
            result += "[" + monsterRiskList[i].mob.ToString() + "-" + monsterRiskList[i].nowNum.ToString() + "] ";
        }
        return result;
    }
}