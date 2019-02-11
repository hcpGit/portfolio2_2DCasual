using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveManager : Singleton<InteractiveManager> //ui 등 상호작용 총 매니져.
{
    #region -ForHunter    

#region inquireQuest
    public void HunterInquireQuest()
    {
        QuestRoomSceneUIManager.GetInstance().OnlyShowHIQUI();
    }
    public void HunterInquireQuestSubmit(string Qkey , bool didDetermined, List<Weapon> rentalSelected)//헌터에게 퀘스트를 맡기고 렌탈등과 퀘스트 등과 선택 했을 때.
    {
        if (!didDetermined)//ui에서 제출이면 true 인자로 넣어서 보내주기!
        {
            EventParameterStorage.GetInstance().PlayerChoice = false;
            CharactorFrame.GetInstance().UpdateFSM();
            return;
        }

        EventParameterStorage.GetInstance().PlayerChoice = true;
        HunterIdea hunter = CharactorFrame.GetInstance().hunterIdea;
        Quest selectedQuest = QuestManager.GetInstance().GetQuest(Qkey);
        
        //본래 퀘스트의 만기보다 하루 빨리. 그리고 10시~ 17시 사이를 랜덤으로 지정.
        InGameTime dayOneMinus = InGameTime.GetOnlyOneDayMinus(selectedQuest.ExpireDate);
        uint hour =  (uint)Random.Range(Constant.dayStartHour, Constant.dayEndHour -1 );  
        InGameTime hunterExpire = new InGameTime(dayOneMinus.Year, dayOneMinus.Month, dayOneMinus.Day, hour, 0);

        hunter.SetHasCommission(Qkey);
        hunter.SetHuntExpireDay(hunterExpire);
        
        if (rentalSelected != null)
        {
            //렌탈 처리 해주기.
            hunter.SetRentalTheseWeapon(rentalSelected);
            Inventory.GetInstance().RemoveWeaponsFromInven(rentalSelected);//렌탈해간 물건들 인벤에서 뺴주는 처리.
        }

        WholeMonsterRiskManager.GetInstance().AddHunter(hunter);    //수임을 받은날 위협도 매니져에서 일함.(그 날의 데이오프 부터)

        QuestRoomSceneUIManager.GetInstance().OnlyShowMainUIOrigin();
        
        int deposit = PayFormula.CalculateHuntDeposit(selectedQuest, hunter.GetHunterExpireGap());   //헌터 착수금 지불.
        GoldManager.GetInstance().AdjustGold(-1 * deposit , GoldManager.E_PayType.TO_HUNTER);

        CharactorFrame.GetInstance().UpdateFSM();
    }
  
    public void HunterReturnRental()    //온리 로직단이라고 보면됨. 렌탈했던 무기를 반납하는 거임.
    {
        //대여했던 무가 반납.(ui아니고 메소드로써 역할임.) 
        HunterIdea hunter = CharactorFrame.GetInstance().hunterIdea;
        List<Weapon> nowBrokenRentaledWeapons = hunter.HandInRentaledWeaponsNotBroken();    //알아서 안 부서진 것만 줌.
        Inventory.GetInstance().AddWeaponsToInven(nowBrokenRentaledWeapons);
    }
      #endregion

    #region reward

    public void HunterRewardUIShow()
    {
        //의뢰했던 퀘스트랑 헌터가 가져온 의뢰품들이랑 비교 ui 보여줌,
        //그 지불 방식 버튼 3개 나오도록.
        QuestRoomSceneUIManager.GetInstance().OnlyShowHRUI();
    }
    
    public void HunterRewardSubmit(int payment) //그냥 eps 처리를 이 단에서 해주려고 한거.
    {
        //완전지불 버튼이면 0 , 부분지불 버튼이면 1 , 거부면 2
        //버튼에서 인자 확실하게 넘겨줄것!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        EventParameterStorage.GetInstance().PlayerMultipleChoice = payment;

        QuestRoomSceneUIManager.GetInstance().OnlyShowMainUIOrigin();

        CharactorFrame.GetInstance().UpdateFSM();
    }
    
    public void HunterRewardCalculate(E_RewardType type) //헌터 보상 정산만.
    {
        HunterIdea hunter = CharactorFrame.GetInstance().hunterIdea;
        Quest writtenQuest = QuestManager.GetInstance().GetQuest(hunter.associatedQuestKey);
        List<QuestPerMob> huntedList = hunter.HuntedList;
        int hunterExpireTerm = hunter.GetHunterExpireGap();
        int hunterPayment = PayFormula.CalculateHuntLastPayment(writtenQuest, hunterExpireTerm, huntedList, type);
        GoldManager.GetInstance().AdjustGold( -1* hunterPayment , GoldManager.E_PayType.TO_HUNTER);

        if(type != E_RewardType.PAYMENT_DENY)
        Inventory.GetInstance().AddMobEviByList(CharactorFrame.GetInstance().hunterIdea.HuntedList);
    }
    #endregion


    #endregion

    #region -ForClient

    #region -QuestMaking
    public void MakingQuest()
    {
        AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.UNDERTAKE);
        //카메라 애니메이션 이동 문제로 인하여 ui 작업을 qm 에서는 메인캠 애님에서 처리하도록 한다.
        HandManager.GetInstance().HandlingMonsterHuntingCommission();
        MainCamAnim.GetInstance().CamToPosMaking();
    }
    public void QuestMakingDone(List<QuestPerMob> madeQPMList)//작성한 퀘스트.
    {
        if (madeQPMList == null || madeQPMList.Count == 0)
        {
            Debug.LogError("퀘스트 메이킹 작업물이 존재하지 않음.");
        }


        ClientIdea client = CharactorFrame.GetInstance().clientIdea;
        string newKey =  Quest.MakeQuestKey(client.CharaName);

        Quest writtenQuest = new Quest();   //작성한 퀘스트 생성.
        writtenQuest.SettingQuestMetaDataByToday(newKey, client.CharaName, client.DaysExpire , madeQPMList);
        QuestManager.GetInstance().AddQuest(newKey, writtenQuest); //퀘스트매니져에 작성한 퀘스트를 넣어둬
        client.SetHasCommission(newKey);    //여기서 의뢰인의 오리진 퀘스트 의뢰일과 만기가 설정됨.
        
        MainCamAnim.GetInstance().CamToPosNormal(
            ()=>{
                CharactorFrame.GetInstance().UpdateFSM();//메인 카메라가 원상태로 복귀한 후에 fsm 업뎃해주기 위해.

                int deposit =  PayFormula.CalculateClientRequestDeposit(client.OriginOrderedQuest,client.DaysExpire); //디포짓 얻는 것.
                GoldManager.GetInstance().AdjustGold(deposit , GoldManager.E_PayType.FROM_CLIENT);
            }); //메인카메라 원상태로.
        // CharactorFrame.GetInstance().UpdateFSM(); 메인 카메라가 원상태로 복귀한 후에 fsm 업뎃해주기 위해. 여기서 업뎃하면 안댐
    }
    #endregion

    #region -QC

    public void CheckCommissionUIShow()  //그 퀘스트 체크 유아이 띄우고
        //완성도 반환
    {
        Quest writtenQuest = QuestManager.GetInstance().GetQuest(
            CharactorFrame.GetInstance().clientIdea.associatedQuestKey
            );
        QuestRoomSceneUIManager.GetInstance().OnlyShowQCUI();
        QuestCheckManager.GetInstance().InitQCCanvas(writtenQuest);
    }

    public void QuestCheckIsDone(List<QuestPerMob> checkedList)
    {
        ClientIdea client = CharactorFrame.GetInstance().clientIdea;
       
        float completeness = client.GetCompletenessClientOriginQuest(checkedList);

        EventParameterStorage.GetInstance().QuestCompareCompleteness= completeness;
        
        CharactorFrame.GetInstance().UpdateFSM();
    }

    void ClientCommissionCheckCalculateLastincome()   //만기여부와 등등을 고려하여 정산할것!! 클라이언트가 qc 정산 하는 단계!
    {
        ClientIdea client = CharactorFrame.GetInstance().clientIdea;
        int outDue = 0;
        if (client.haveComeBeforeExpire)
        {
            InGameTime clientExpire = client.GetExpire();
            InGameTime nowTime = InGameTimeManager.GetInstance().GetNowTime();
            outDue = nowTime.GetDaysGap(clientExpire);
        }
        int lastClientPayment = PayFormula.CalculateClientRequestLastPayment(client.OriginOrderedQuest ,
            client.DaysExpire, 
            EventParameterStorage.GetInstance().QuestCompareCompleteness, outDue);

        GoldManager.GetInstance().AdjustGold(lastClientPayment,GoldManager.E_PayType.FROM_CLIENT);
    }
    #endregion

    #endregion
    
    public void CharactorLeave(E_Charactor charactor)
    {
        QuestRoomSceneUIManager.GetInstance().OnlyShowMainUIOrigin();
        Stack<FSMState> stateStack = CharactorFrame.GetInstance().CharaController.GetStateStack();
        CharactorIdea nowChara;
        //여기가 너무 안좋은데.
        switch (charactor)
        {
            case E_Charactor.CLIENT:
                 nowChara = CharactorFrame.GetInstance().clientIdea;

                if(stateStack.Contains(ClientFSMFactory.GetInstance().GetState(E_ClientState.COMMISSION_CANCEL)))
                {
                    CharactorManager.GetInstance().RemoveCharactor(nowChara);
                    break;
                }
                if (stateStack.Contains(ClientFSMFactory.GetInstance().GetState(E_ClientState.CHECK_IMPERFACT_00))
                    ||
                    stateStack.Contains(ClientFSMFactory.GetInstance().GetState(E_ClientState.CHECK_IMPERFACT_50))
                    ||
                    stateStack.Contains(ClientFSMFactory.GetInstance().GetState(E_ClientState.CHECK_IMPERFACT_80))
                    ||
                    stateStack.Contains(ClientFSMFactory.GetInstance().GetState(E_ClientState.CHECK_IMPERFACT_95))
                    ||
                    stateStack.Contains(ClientFSMFactory.GetInstance().GetState(E_ClientState.CHECK_PERFACT))
                    )
                {
                    ClientCommissionCheckCalculateLastincome();
                    CharactorManager.GetInstance().RemoveCharactor(nowChara);
                }

                break;

            case E_Charactor.HUNTER:
                 nowChara = CharactorFrame.GetInstance().hunterIdea;

                if (stateStack.Contains(HunterFSMFactory.GetInstance().GetState(E_HunterState.INQUIRE_QUEST_CANCEL)))
                {
                    CharactorManager.GetInstance().RemoveCharactor(nowChara);
                    break;
                }
                if (stateStack.Contains(HunterFSMFactory.GetInstance().GetState(E_HunterState.HUNT_REWARD_ALL_PAYMENT))
                    ||
                    stateStack.Contains(HunterFSMFactory.GetInstance().GetState(E_HunterState.HUNT_REWARD_PARTIAL_PAYMENT))
                    ||
                    stateStack.Contains(HunterFSMFactory.GetInstance().GetState(E_HunterState.HUNT_REWARD_DENIED))
                    )
                {
                    CharactorManager.GetInstance().RemoveCharactor(nowChara);
                    break;
                }
                break;
        }
        CharactorFrame.GetInstance().LeaveAnim();
        Debug.Log("캐릭터 리브");
    }

    public void ShowTalk(string charaText, params string[] playerText)
    {
        QuestRoomSceneUIManager.GetInstance().OnlyShowMainUITalk();
        MainUIManager.GetInstance().SetCharaText(charaText);
        if (playerText.Length == 1)
        {
          //  Debug.Log("인터렉티브 매니져 - 쇼토크" + charaText + playerText[0]);
            MainUIManager.GetInstance().ShowPlayerText1Btn(playerText[0]);
        }
        else if (playerText.Length == 2)
        {
          //  Debug.Log("인터렉티브 매니져 - 쇼토크" + charaText + playerText[0] + playerText[1]);
            MainUIManager.GetInstance().ShowPlayerText2Btn(playerText[0], playerText[1]);
        }
        else {
            string result="";
            foreach (string str in playerText)
            {
                result += str+"/";
            }
            Debug.Log("쇼토크 에러" + result);
        }
    }
}
