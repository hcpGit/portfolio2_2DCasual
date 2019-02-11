using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ClientIdea : CharactorIdea {
    [SerializeField]
    Quest originOrderedQuest;
    public Quest OriginOrderedQuest
    {
        get {
            return originOrderedQuest;
        }
    }
    [SerializeField]
    int daysExpire; //몇일 후를 만기로 할 것인가.
    public int DaysExpire
    {
        get {
            return daysExpire;
        }
    }

    public override void AnamnesisToCharactorFrame()
    {
        base.AnamnesisToCharactorFrame();
        CharactorFrame.GetInstance().SetClientIdea(this);
    }

    public ClientIdea(int charaSpriteIdx ,  string charaName)
    {
        this.charaSpriteIdx = charaSpriteIdx;
        SetCharaSprite();

        this.charaType = E_Charactor.CLIENT;
        this.charaName = charaName;
        this.personality = (E_Personality)(Random.Range(0, (int)E_Personality.MAX)) ;
        this.hasCommission = false;
        this.associatedQuestKey = "";
        this.haveComeBeforeExpire = false;
        this.indivisualDifference = 0; //이걸 생성 부 부터 랜덤으로 할까?

        originOrderedQuest = MakeRandomOrderQuest();//얘를 생성부터 랜덤으로 만들어줘야하는 건지 어쩐 건지 모르겠어.
        //오리진 퀘스트의 가중치에 다라서 만기 텀의 기간을 설정?

        daysExpire = Random.Range(3,
            //모킹
            4);
            //모킹 종료
            
           // 10);   //2~9일 까지의 만기를 주는.
    }

    public override void SetHasCommission(string Qkey)
    {
        base.SetHasCommission(Qkey);
        originOrderedQuest.SetOrderDateAtNowTime();
        originOrderedQuest.SetExpireAfterDays(daysExpire);
    }

    public override void SetupAI(FSMController cont)
    {
        cont.Setup(ClientFSMFactory.GetInstance().GetState(E_ClientState.IDLE));
    }

    public override InGameTime GetExpire()
    {
        if (false == hasCommission) return null;
        if (originOrderedQuest == null) Debug.LogError("클라이언트의 오리진 퀘스트가 존재하지 않음."+this.charaName);

        return originOrderedQuest.ExpireDate;
    }

    public float GetCompletenessClientOriginQuest(List<QuestPerMob> compare)
    {
        return originOrderedQuest.GetCompleteness(compare);
    }

    Quest MakeRandomOrderQuest()//랜던으로 퀘스트 만들어주기.
    {
        Quest quest = new Quest();
        //모킹
        quest.ClientName = charaName;
        List<E_Monster> showedMob = new List<E_Monster>();
        for (int i = 0; i < (int)E_Monster.MAX; i++)
        {
            if(PhaseManager.GetInstance().IsMobShowUP(  (E_Monster)i) )
                showedMob.Add((E_Monster)i);
        }
        List<E_Evidence> opendEvi = PhaseManager.GetInstance().GetOpendEviDataList();
        
        int kinds = Random.Range(1, Constant.clientMaxOrderNum+1);    //qpm종류로 몇개를 할 건지.

        for (int i = 0; i < kinds; i++)
        {
            E_Monster mob =         showedMob[Random.Range(0, showedMob.Count)];
            E_Evidence evidence =   opendEvi[Random.Range(0, opendEvi.Count)];
            int number = GetNumOfQPM();

            quest.AddQuestMob(new QuestPerMob( mob, evidence, number));
        }
        
        return quest;
    }

    int GetNumOfQPM()
    {
        int basic = 9;
        int days = InGameTimeManager.GetInstance().GetDaysAfterGameStart();
        int plus = (int)(days/7); //일주일 마다 하나씩 올림.
        if (plus > 90)
        {
            plus = 90;  //그래도 한 번에 최대는 99
        }
        return Random.Range(1, basic + plus+1); //(1~9)  ~ (1~99)
    }
}
