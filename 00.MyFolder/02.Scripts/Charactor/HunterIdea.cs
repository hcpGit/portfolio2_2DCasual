using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[System.Serializable]
public class HunterIdea : CharactorIdea {
    [SerializeField]
    int equipCap;
    public int EquipCap
    {    get{return equipCap;}
    }
    [SerializeField]
    float huntingCapabillity;
    public float HuntingCapabillity
    {
        get { return huntingCapabillity; }
    }
    [SerializeField]
    List<Weapon> rentalWeapon;
    public List<Weapon> RentalWeapon
    {
        set {
            rentalWeapon = value;
        }
        get {
            return rentalWeapon;
        }
    }

    [SerializeField]
    List<QuestPerMob> huntedList;   //사냥한 애들.
    public List<QuestPerMob> HuntedList
    {
        get {
            return huntedList;
        }
    }
    [SerializeField]
    InGameTime questTakeOnDay;
    public InGameTime QuestTakeOnDay
    {
        get { return questTakeOnDay; }
    }
    [SerializeField]
    InGameTime questHuntExpireDay;

    public override void AnamnesisToCharactorFrame()
    {
        base.AnamnesisToCharactorFrame();
        CharactorFrame.GetInstance().SetHunterIdea(this);
    }



    public HunterIdea(int charaSpriteIdx, string charaName, float huntCap)
    {
        this.charaSpriteIdx = charaSpriteIdx;
        SetCharaSprite();

        this.charaType = E_Charactor.HUNTER;
        this.charaName = charaName;
        this.personality = (E_Personality)(Random.Range(0, (int)E_Personality.MAX));
        this.hasCommission = false;
        this.associatedQuestKey = "";
        this.haveComeBeforeExpire = false;
        this.indivisualDifference = 0; //이걸 생성 부 부터 랜덤으로 할까?
        this.huntingCapabillity = huntCap;
        this.rentalWeapon = null;
        this.equipCap = Random.Range(0,Constant.hunterMaxEquipNum+1);   //0~3개 까지.
    }

    #region -헌터 퀘스트 수임

    public override void SetHasCommission(string Qkey)
    {
        base.SetHasCommission(Qkey);
        questTakeOnDay = InGameTimeManager.GetInstance().GetNowTime();
    }
    public void SetHuntTakeOnDay(InGameTime huntTake)
    {
        questTakeOnDay = huntTake;
    }
    public void SetHuntExpireDay(InGameTime huntExpire)
    {
        questHuntExpireDay = huntExpire;
    }
    public int GetHunterExpireGap()
    {
        return questHuntExpireDay.GetDaysGap(questTakeOnDay);
    }

    #endregion


    #region -무기 렌탈
    public bool DidRentalWeapon()
    {
        if (rentalWeapon == null || rentalWeapon.Count == 0)
            return false;
        return true;
    }

    public void SetRentalTheseWeapon(List<Weapon> rentaled)
    {
        if (equipCap < rentaled.Count)
        {
            Debug.LogError("무기 렌탈 허용 가능 수치를 넘어섰음!");
            return;
        }
        rentalWeapon = rentaled;
    }

    public bool IsBrokenRental()    //렌탈한 것중 하나라도 부러진 게 있으면 트루
    {
        if (rentalWeapon == null) return false;
        foreach (Weapon weapon in rentalWeapon)
        {
            if (weapon.IsBroken()) return true;
        }
        return false;
    }

    public float GetHuntCapabillityForRealhunt()
    {
        float res = huntingCapabillity;
        if (rentalWeapon == null || rentalWeapon.Count==0) return res;
        //이제 무기를 빌렸을 때 무기랑 헌팅 능력이랑 짬뽕해서 리턴해줄것,

        for (int i = 0; i < rentalWeapon.Count; i++)    //부서진 무기를 사냥 도중에 처리하는 건 자제.
        {
            res += rentalWeapon[i].PlusCapability;
        }
        
        return res;
    }

    public List<Weapon> HandInRentaledWeaponsNotBroken()
    {
        if (rentalWeapon == null || rentalWeapon.Count == 0)
        {
            Debug.LogError("렌탈 할 것이 없는데 렌발 반납 함수가 불려짐");
            return null;
        }

        List<Weapon> temp = new List<Weapon>();
        for (int i = 0; i < rentalWeapon.Count; i++)
        {
            if (!rentalWeapon[i].IsBroken())
            {
                temp.Add(rentalWeapon[i]);
            }
        }

        rentalWeapon = null;

        return temp;
    }

    void BrakeGambleRentalWeapons()
    {
        if (rentalWeapon == null) return;
        for (int i = 0; i < rentalWeapon.Count; i++)
        {
            rentalWeapon[i].BrakeGamble();
        }
    }

    #endregion


    public bool IsSulked()
    {
        Quest writtenQuest = QuestManager.GetInstance().GetQuest(associatedQuestKey);
        //자기가 가져온 의뢰품이랑, 연관된 퀘스트랑 짬짜뽕해서 삐칠지 말지를 결정함.
        float completeness = writtenQuest.GetCompleteness(huntedList);

        if (completeness > 100) completeness = 100;
        if (completeness < 0) completeness = 0;

        if (Random.Range(0f, 100f) < completeness)  //완성도 보다 작으면
                                                    //즉 완성도가 크면 삐칠 확률이 커짐. 이빠이 해왔는데 쪼잔하게 다 안준다고 삐치는 느낌
        {
            return true;
        }
        else
        {
            return false;   //완성도가 작았으므로 ㅇㅈ 하고 부분지불을 받아들임.
        }
    }
    
    public override void SetupAI(FSMController cont)
    {
        cont.Setup(HunterFSMFactory.GetInstance().GetState(E_HunterState.IDLE));
    }
    public override InGameTime GetExpire()
    {
        //연관된 퀘스트 키로 부터 받아와 만기를 추출.
        //associatedQuestKey 와 haveComeBeforeExpire 을 잘 쓰까서 추출할것.
        if (false == hasCommission) return null;
        if(QuestManager.GetInstance().GetQuest(associatedQuestKey) == null) Debug.LogError("클라이언트의 오리진 퀘스트가 존재하지 않음." + this.charaName+" 키 = "+associatedQuestKey);
        if (questHuntExpireDay == null) Debug.LogError("헌터 만기가 존재하지 않음." + this.charaName + " 키 = " + associatedQuestKey);
        return questHuntExpireDay;
    }
    #region -몹 위협도 에서 불러지는 헌팅 

    public List<QuestPerMob> HuntingAtNight(out bool isLastDayHunt)   //위협 매니저에서 밤에 얘를 호출.
    {
        Quest quest = QuestManager.GetInstance().GetQuest(associatedQuestKey);

        if (quest == null || !hasCommission)
        {
            isLastDayHunt = true;
            Debug.LogError("헌팅 나이트 불려오면 안됨.");
            return null;
        }

        InGameTime nowTime = InGameTimeManager.GetInstance().GetNowTime();
        InGameTime questExpireDay = questHuntExpireDay;
        int wholeQTerm = questExpireDay .GetDaysGap(questTakeOnDay);
        int daysAfterTakeQuest = nowTime.GetDaysGap(questTakeOnDay);    //0~ 부터 기간-1 까지 나옮.

        if (wholeQTerm <= 0)
        {
            isLastDayHunt = true;
            Debug.LogError("헌터"+charaName+" 헌팅나이트 수임 부터 만기 까지 0일 이하임."+wholeQTerm+"퀘스트 만기 = "+ questExpireDay+"퀘스트 입기"+ questTakeOnDay);
            return null;
        }

        if (daysAfterTakeQuest >= wholeQTerm)
        {
            isLastDayHunt = true;
            Debug.Log(charaName+" 헌터는 이미 만기일이 지났으므로 사냥을 하지 않아용");
            return null;
        }



        if (huntedList == null) huntedList = new List<QuestPerMob>();

        List<QuestPerMob> orderedQuest = quest.QuestList;

        List<QuestPerMob> tonightHunted;

        float huntCapTillTonight = (GetHuntCapabillityForRealhunt() / wholeQTerm)*(daysAfterTakeQuest+1) ;
        //능력치를 전체 기간으로 나누고 수임한 날부터 이 밤까지 곱해서 나오는 값.

        Debug.LogFormat("{0}의 밤사냥 {1} 일 기간 , 수임날 부터 {2} 지남 , 전체 헌터 무기포함 능력치 = {3} , 오늘 사용할 수 있는 능력치 = {4} , 지금까지 잡은 목록 = {5}",

            charaName, wholeQTerm, daysAfterTakeQuest, GetHuntCapabillityForRealhunt(), huntCapTillTonight, huntedList.DebugString());
        

        if (daysAfterTakeQuest == wholeQTerm - 1)   //마지막 날임.
        {
            isLastDayHunt = true;
            tonightHunted = HuntCompactOrder(orderedQuest , huntCapTillTonight);
            Debug.LogFormat("{0}이 오늘 잡은 목록 - 컴팩트 오더 {1}", charaName, tonightHunted.DebugString());

            huntedList.AddQPMList(tonightHunted);
            BrakeGambleRentalWeapons(); //사냥 할때 마다 갬블을 한 번씩.
            return tonightHunted;
        }

        isLastDayHunt = false;

        //수임~ 만기 까지의 마지막 밤이 아닌 밤임.
        tonightHunted = HuntBigOrder(orderedQuest,  huntCapTillTonight);
        Debug.LogFormat("{0}이 오늘 잡은 목록 - 빅 오더 {1}", charaName, tonightHunted.DebugString());
        huntedList.AddQPMList(tonightHunted);
        BrakeGambleRentalWeapons();
        return tonightHunted;
    }

    List<QuestPerMob> HuntCompactOrder(List<QuestPerMob> orderedQuestList,  float capTillTonight)
    {
        if (huntedList == null) Debug.LogError("사냥하던 리스트가 없음.");

        //큰거 부터 잡되 안 되면 다음 놈으로 넘어가서 또 잡는 거.

        float usedCap = 0;
        for (int i = 0; i < huntedList.Count; i++)
        {
            usedCap += huntedList[i].GetWeight();
        }

        float tonightHuntingCap = capTillTonight - usedCap; //실제 오늘 사용할 수 있는 능력치.
        if (tonightHuntingCap <= 0)
        {
            return null;
        }


        List<QuestPerMob> tonightHunt = new List<QuestPerMob>();    //오늘 잡을 애들 리스트

        var bo = from qpm in orderedQuestList
                 orderby qpm.GetMobEviTypeWeight() descending
                 select qpm;

        List<QuestPerMob> bigOrderedList = bo.ToList<QuestPerMob>();//가중치가 큰 애들 순서로 리스트를 맹긂

        bool matchedByHuntedAlready = false;

        for (int i = 0; i < bigOrderedList.Count; i++)
        {
            matchedByHuntedAlready = false;

            QuestPerMob bigOne = bigOrderedList[i];
            E_Monster mob = bigOne.mob;
            E_Evidence evidence = bigOne.evidence;

            float bigOneWeightForOne = bigOne.GetMobEviTypeWeight();    //지금 선택된 놈을 한 번 잡는데 소요되는 능력치.

            if (tonightHuntingCap < bigOneWeightForOne) //제일 큰 놈을 하나 잡을 여유도 없으면.
            {
                continue;   //다음놈을 잡을 기회
            }
            

            for (int j = 0; j < huntedList.Count; j++)  //잡고 있던 녀석인지 체크 컴팩트 오더 경우 어떻게든 다음 빅 원으로 넘어가는 것.
            {
                if (huntedList[j].IsIt(mob, evidence))    //이미 잡고 있는 혹은 잡은 종류라면.
                {
                    if (huntedList[j].number >= bigOne.number)  //다 잡은 종류라면.
                    {
                        matchedByHuntedAlready = true;
                        break;
                    }
                    else //잡다가 말은 경우라면.
                    {
                        int remainNum = bigOne.number - huntedList[j].number;
                        while (remainNum > 0)
                        {
                            tonightHunt.AddMobEviOnce(mob, evidence);
                            remainNum--;
                            tonightHuntingCap -= bigOneWeightForOne;
                            if (tonightHuntingCap < bigOneWeightForOne && remainNum > 0)//더 못잡는데 아직 남아있음.
                            {
                                matchedByHuntedAlready = true;
                                break;
                            }
                        }
                        matchedByHuntedAlready = true;  //뭐가 됐든 할 수 있는 한은 다 한것.
                    }
                }
            }

            if (matchedByHuntedAlready) continue;   //이미 잡던 놈이라, 처리가 끝남.

            //잡고 있던 애가 아니라면

            int remainNumOfNewOne = bigOne.number;
            while (remainNumOfNewOne > 0)
            {
                tonightHunt.AddMobEviOnce(mob, evidence);
                remainNumOfNewOne--;
                tonightHuntingCap -= bigOneWeightForOne;
                if (tonightHuntingCap < bigOneWeightForOne && remainNumOfNewOne > 0)//더 못잡는데 아직 남아있음.
                {
                    break;//다음 빅 원으로 넘어감.
                }
            }
        }
        return tonightHunt;
    }

    List<QuestPerMob> HuntBigOrder(List<QuestPerMob> orderedQuestList,  float capTillTonight)
    {
        if (huntedList == null) Debug.LogError("사냥하던 리스트가 없음.");

        float usedCap = 0;
        for (int i = 0; i < huntedList.Count; i++)
        {
            usedCap += huntedList[i].GetWeight();
        }

        float tonightHuntingCap = capTillTonight - usedCap; //실제 오늘 사용할 수 있는 능력치.
        if (tonightHuntingCap <= 0)
        {
            return null;
        }


        List<QuestPerMob> tonightHunt = new List<QuestPerMob>();    //오늘 잡을 애들 리스트

        var bo = from qpm in orderedQuestList
                              orderby qpm.GetMobEviTypeWeight() descending
                 select qpm;

        List<QuestPerMob> bigOrderedList = bo.ToList<QuestPerMob>();//가중치가 큰 애들 순서로 리스트를 맹긂

        Debug.Log("빅오더 정렬 리스트 = "+bigOrderedList.DebugString());

        bool matchedByHuntedAlready = false;

        for (int i = 0; i < bigOrderedList.Count; i++)
        {
            matchedByHuntedAlready = false;

            QuestPerMob bigOne = bigOrderedList[i];
            E_Monster mob = bigOne.mob;
            E_Evidence evidence = bigOne.evidence;

            float bigOneWeightForOne = bigOne.GetMobEviTypeWeight();    //지금 선택된 놈을 한 번 잡는데 소요되는 능력치.

            if (tonightHuntingCap < bigOneWeightForOne) //제일 큰 놈을 하나 잡을 여유도 없으면.
            {
                return tonightHunt;
            }

            for (int j = 0; j < huntedList.Count; j++)  //잡고 있던 녀석인지 체크
            {
                if (huntedList[j].IsIt(mob ,evidence))    //이미 잡고 있는 혹은 잡은 종류라면.
                {
                    if (huntedList[j].number >= bigOne.number)  //다 잡은 종류라면.
                    {
                        matchedByHuntedAlready = true;
                        break;
                    }
                    else //잡다가 말은 경우라면.
                    {
                        int remainNum = bigOne.number - huntedList[j].number;
                        while (remainNum > 0)
                        {
                            tonightHunt.AddMobEviOnce(mob, evidence);
                            remainNum--;
                            tonightHuntingCap -= bigOneWeightForOne;
                            if (tonightHuntingCap < bigOneWeightForOne && remainNum > 0)//더 못잡는데 아직 남아있으면 쫑
                            {
                                return tonightHunt;
                            }
                        }
                        matchedByHuntedAlready = true;  //다 잡았다.
                    }
                }
            }

            if (matchedByHuntedAlready) continue;   //다 잡았던 놈이라면

            //잡고 있던 애가 아니라면

            int remainNumOfNewOne = bigOne.number;
            while (remainNumOfNewOne > 0)
            {
                tonightHunt.AddMobEviOnce(mob, evidence);
                remainNumOfNewOne--;
                tonightHuntingCap -= bigOneWeightForOne;
                if (tonightHuntingCap < bigOneWeightForOne && remainNumOfNewOne > 0)//더 못잡는데 아직 남아있으면 쫑
                {
                    return tonightHunt;
                }
            }
            
            //새로운 애도 다 잡아버린 경우.
        }
        return tonightHunt;
    }

    #endregion
}
