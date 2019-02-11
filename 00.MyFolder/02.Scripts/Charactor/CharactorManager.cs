using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CharactorManager : Singleton<CharactorManager> {
    public class VisitChara
    {
        public VisitChara(uint st, CharactorIdea id)
        {
            comeTimeStamp = st; one = id;
        }
        public uint comeTimeStamp;
        public CharactorIdea one;
    }

    List<CharactorIdea> charaList = new List<CharactorIdea>();  //전체 캐릭터 리스트?
    public List<CharactorIdea> CharaList
    {
        get {
            return charaList;
        }
    }
    Queue<VisitChara> expireCharaQue = new Queue<VisitChara>();
    public Queue<VisitChara> ExpireCharaQue
    {
        get
        {
            return expireCharaQue;
        }
    }


    List<CharactorIdea> visitedToday = new List<CharactorIdea>();   //오늘 하루 방문했던 아이.
    public List<CharactorIdea> VisitedToday
    {
        get
        {
            return visitedToday;
        }
    }
    public void InitForNewStart()
    {
        charaList.Clear();
        expireCharaQue.Clear();
        visitedToday.Clear();
    }

    public void DayOff()    //하루 종료. 리스트들 취합.
    {
        CharactorIdea temp;
        while (expireCharaQue.Count > 0)
        {
            temp = expireCharaQue.Dequeue().one;
            charaList.Add(temp);
        }
        charaList.AddRange(visitedToday);

        visitedToday.Clear();
        expireCharaQue.Clear();

        //의뢰를 가지지 못한 애는 삭제.
        int idx = 0;
        while (idx < charaList.Count)
        {
            if (false == charaList[idx].hasCommission)
                charaList.RemoveAt(idx);
            else idx++;
        }
    }

    public CharactorIdea GetNextCharactor() //새로 부르는 것. 선택된 애는 visitedparty에 들어감.
    {
        GoBackExpires();    //기다리던 애들 돌아감.

        CharactorIdea selectedIdea;

        if (expireCharaQue.Count >= 1)
        {
            selectedIdea = expireCharaQue.Dequeue().one;
            visitedToday.Add(selectedIdea);
            return selectedIdea;
        }
        

        for (int i = 0; i < charaList.Count; i++)
        {
            if (charaList[i].hasCommission && expired(charaList[i]))
            {
                selectedIdea = charaList[i];
                charaList.RemoveAt(i);  //하나만 딱 고르는 거니까 for문을 썼지만 리스트 순환 검색과 삭제 동시 운용시 주의할것.
                visitedToday.Add(selectedIdea);
                return selectedIdea;
            }
        }

        selectedIdea = GambleGetNewbie();
        if (selectedIdea == null) return null;

        visitedToday.Add(selectedIdea);

        return selectedIdea;
    }

    public void SelectExpireVisit()
    {
        GoBackExpires();
        SelectExpires();
    }

    void GoBackExpires() //만기일 온 애들 기다리다가 그냥 가는 애들.
    {
        string leaveMsg = UIGeneralTextsManager.GetUIGeneralText("quickMessage", "leaved");
        if (expireCharaQue.Count <= 0) return;

        uint nowTime = InGameTimeManager.GetInstance().GetNowInGameTimeStamp();

        VisitChara visited;

        visited = expireCharaQue.Peek();

        while (visited!=null)  //1시간 기다리다 감.
        {
            if (visited.comeTimeStamp + 60 >= nowTime)
            {
                CharactorIdea goback = expireCharaQue.Dequeue().one;
                goback.haveComeBeforeExpire = true;
                visitedToday.Add(goback);
                QuickMessageUIManager.GetInstance().ShowQuickMessage(goback.CharaName + leaveMsg);
                Debug.Log(goback.CharaName + "만기 기다리다 그냥 돌아감 현재 시각 " + InGameTimeManager.GetInstance().GetNowTime()
                    + "캐릭터의 만기는" + goback.GetExpire() + "였음  비지트 타음스탬프 =" + visited.comeTimeStamp + "현재시각 타임스탬프 ="
                    + nowTime
                    );
                if (expireCharaQue.Count <= 0) return;
                visited = expireCharaQue.Peek();
            }
            else
                return;
        }
    }

    void SelectExpires() //만기일 온 애들 큐에 집어넣는 과정.
    {
        string visitMsg = UIGeneralTextsManager.GetUIGeneralText("quickMessage", "visited");
        int idx = 0;

        while (idx < charaList.Count)
        {
            if (charaList[idx].hasCommission && !charaList[idx].haveComeBeforeExpire)   //기본 조건.
            {
                if (!expired(charaList[idx]))
                {
                    idx++;
                    continue;
                }

                CharactorIdea expireOne = charaList[idx];
                charaList.RemoveAt(idx);
                expireCharaQue.Enqueue(new VisitChara(InGameTimeManager.GetInstance().GetNowInGameTimeStamp(), expireOne));
                QuickMessageUIManager.GetInstance().ShowQuickMessage(expireOne.CharaName + visitMsg);
                Debug.Log(expireOne.CharaName + "만기 큐에 들어옴. 현재시각 - " + InGameTimeManager.GetInstance().GetNowTime()
              + "캐릭터의 만기는" + expireOne.GetExpire() + "였음"
              );

            }
            else {
                idx++;
            }
        }
    }

    bool expired(CharactorIdea idea)
    {
        uint nowTime = InGameTimeManager.GetInstance().GetNowInGameTimeStamp();
        uint expireTime = idea.GetExpire().InGameTimeMinuteStamp;
        if (expireTime <= nowTime) return true;
        //나중에 만기시각 몇 시간 정도 까지 랜덤? 하게 봐줄지 결정하기.
        return false;
    }

    CharactorIdea GambleGetNewbie()
    {
        bool makeNewbie = (Random.Range(0, 2) == 1) ? makeNewbie = true : makeNewbie = false;

        E_Charactor charaType = (E_Charactor)(Random.Range(0,(int)E_Charactor.MAX));

        if (QuestManager.GetInstance().QuestDic.Count == 0) //현재 퀘스트가 한 개도 없으면 확정으로 클라이언트 생성.
        {
            charaType = E_Charactor.CLIENT;
            makeNewbie = true;
        }

        if (!makeNewbie) return null;   //캐릭터를 새로 생성 안하는 경우도 있음.

        string charaName = TextManager.GetInstance().GetNewbieName();

        CharactorIdea newbie = null;
        int spriteNum = Random.Range(0, SpriteManager.GetInstance().charactorSprites.Length);   //캐릭터 스프라이트는... 그냥 랜덤으로...
        switch (charaType)
        {
            case E_Charactor.CLIENT:
                newbie = new ClientIdea(spriteNum, charaName);
                break;

            case E_Charactor.HUNTER:
                float nowQuestsAverageCap = Mathf.Floor( QuestManager.GetInstance().GetAverageQuestCap());
                //현재 퀘스트에 올라온 평균 능력치의 절반 부터 평균치의 1~2.5배 능력치 중에서 랜덤
                float hunterCap = Mathf.Floor( Random.Range(
                            nowQuestsAverageCap/2, 
                            checked(
                            nowQuestsAverageCap *Random.Range(1f,2.5f)
                            )));

                newbie = new HunterIdea(spriteNum, charaName, hunterCap);
                break;
        }
        return newbie;

       
        /*

        CharactorIdea tempforDebugNewbie;
#if UNITY_EDITOR
        //모킹 
        if(ds.GetInstance().clientBorn)
        tempforDebugNewbie = new ClientIdea(0, tempName, E_Personality.CHEAP);
        //새로운 캐릭터 생성.
        else
            tempforDebugNewbie = new HunterIdea(1, tempName, E_Personality.CHEAP, hunterCap);
#endif
        return tempforDebugNewbie;
        */
    }
    
    public void RemoveCharactor(CharactorIdea nowChara) //리브 (딴 놈 콜 전에 호출됨.)
        //리브 애니메이션 전에 호출됨.
    {
        TextManager.GetInstance().HandInName(nowChara.CharaName);
        for (int i = 0; i < visitedToday.Count; i++)
        {
            if (visitedToday[i] == nowChara)
            {
                visitedToday.RemoveAt(i);   //얘는 뭐 하나니까.
                return;
            }
        }

        Debug.LogError("현재 나우 캐릭터 문제. 비지티드 파티에 없음." + nowChara.CharaName);
    }

    public List<ClientIdea> GetClientListSave()
    {
        List<ClientIdea> temp = new List<ClientIdea>();
        for (int i = 0; i < charaList.Count; i++)
        {
            ClientIdea client = charaList[i] as ClientIdea;
            if (client != null)
            {
                temp.Add(client);
            }
        }
        return temp;
    }
    public List<HunterIdea> GetHunterListSave()
    {
        List<HunterIdea> temp = new List<HunterIdea>();
        for (int i = 0; i < charaList.Count; i++)
        {
            HunterIdea hunter = charaList[i] as HunterIdea;
            if (hunter != null)
            {
                temp.Add(hunter);
                Debug.Log(hunter.CharaName + "이 무기를 들고 있는 지여부 = " + hunter.DidRentalWeapon());
            }
        }
        return temp;
    }
    public void Load(SaveStructure st) {

        List<ClientIdea> clist = st.charactorList_Client;
        List<HunterIdea> hlist = st.charactorList_Hunter;
    
        charaList.Clear();
        expireCharaQue.Clear();
        visitedToday.Clear();
        for (int i = 0; i < clist.Count; i++)
        {
            clist[i].SetCharaSprite();
            charaList.Add(clist[i]);
        }
        for (int i = 0; i < hlist.Count; i++)
        {
            hlist[i].SetCharaSprite();

            if (hlist[i].DidRentalWeapon()) //무기를 빌린게 이으면 무기를 정보에 따라 뽑아 만듦.
            {
                for (int j = 0; j < hlist[i].RentalWeapon.Count; j++)
                {
                    Weapon weapon = WeaponInfoManager.GetInstance().CreateWeapon(
                        hlist[i].RentalWeapon[j].weaponType, 
                        hlist[i].RentalWeapon[j].IsBroken());
                   
                   hlist[i].RentalWeapon[j] = weapon;
                }
            }

            charaList.Add(hlist[i]);
        }
    }

    public List<HunterIdea> GetHuntersByQuestKey(string Qkey) {

        List<HunterIdea> temp = new List<HunterIdea>();

        for (int i = 0; i < charaList.Count; i++) {
            if (charaList[i].associatedQuestKey == Qkey && charaList[i].CharaType == E_Charactor.HUNTER)
            {
                temp.Add(charaList[i] as HunterIdea);
            }
        }
        for (int i = 0; i < visitedToday.Count; i++)
        {
            if (visitedToday[i].associatedQuestKey == Qkey && visitedToday[i].CharaType == E_Charactor.HUNTER)
            {
                temp.Add(visitedToday[i] as HunterIdea);
            }
        }

        Queue<VisitChara>.Enumerator enu =  expireCharaQue.GetEnumerator();

        while (enu.MoveNext())
        {
            CharactorIdea ci = enu.Current.one;
            if (ci.associatedQuestKey == Qkey && ci.CharaType == E_Charactor.HUNTER)
            {
                temp.Add(ci as HunterIdea);
            }
        }
        return temp;
    }
}
