using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
[System.Serializable]
public class SaveStructure
{
    //랜귀지 매니저.
    [SerializeField]
    public E_Language lang;

    //골드매니저
    [SerializeField]
    public int golds;

    //게임엔드젇지매니저
    [SerializeField]
    public int goldRunOutContinuityDays ;   //골드가 덜어졌던 날들.
    [SerializeField]
    public int threatMaxContinuityDays;    //위험도가 맥스를 연속으로 친 날들.

    //인게임타임매니저
    [SerializeField]
    public InGameTime savedDate; // 저장하는 그 날 밤의 날짜인 거임. 새 하루 시작 하기 전에 그 날짜.

    //캐릭터매니저
    [SerializeField]
    public List<ClientIdea> charactorList_Client ;
    public List<HunterIdea> charactorList_Hunter ;

    //퀘스트매니저
    [SerializeField]
    public Dictionary<string, Quest> questDic;

    //인벤토리
    [SerializeField]
    public Dictionary<string, int> inventoryMobEvi;
    [SerializeField]
    public Dictionary<E_Weapon,int> inventoryWeapon; //로드시 변환필요

    //위협도 매니저
    [SerializeField]
    public List<WholeMonsterRiskManager.MonsterRisk> threatState;
    [SerializeField]
    public List<HunterIdea> threatHunters ;

    //페이즈 매니저
    [SerializeField]
    public Dictionary<E_Monster, bool> phaseMobShowedUp ;    //몬스터 출몰여부
    [SerializeField]
    public Dictionary<E_Monster, bool> phaseMobOpen ;    //몬스터 허가증 여부
    [SerializeField]
    public Dictionary<E_Evidence, bool> phaseEvidenceOpen ; //증거품 허가증 여부
    [SerializeField]
    public Dictionary<E_Upgrade, bool> phaseUpgradeOpen ;    //업그레이드 여부
    [SerializeField]
    public Dictionary<E_Interior, bool> phaseInteriorOpen ;    //인테리어 산 여부 저장

    [SerializeField]
    public Dictionary<string, bool> distributedNames;

    public void SavePrepare()
    {
        lang = LanguageManager.GetInstance().Language;
        golds = GoldManager.GetInstance().Gold;
        goldRunOutContinuityDays = GameEndJudgeManager.GetInstance().GoldRunOutContinuityDays;
        threatMaxContinuityDays = GameEndJudgeManager.GetInstance().ThreatMaxContinuityDays;
        savedDate = InGameTimeManager.GetInstance().GetNowTime();

        charactorList_Client = CharactorManager.GetInstance().GetClientListSave();
        charactorList_Hunter = CharactorManager.GetInstance().GetHunterListSave();

        questDic =          QuestManager.GetInstance().             QuestDic;
        inventoryMobEvi =   Inventory.GetInstance().                MobEvidenceInven;
        inventoryWeapon =   Inventory.GetInstance().                GetWeaponInvenSave();
        threatState =       WholeMonsterRiskManager.GetInstance().  MonsterRiskList;
        threatHunters =     WholeMonsterRiskManager.GetInstance().  HuntersWhohaveQuest;
        phaseMobShowedUp =  PhaseManager.GetInstance().             MobShowedUp;
        phaseMobOpen =      PhaseManager.GetInstance().             MobOpen;
        phaseEvidenceOpen = PhaseManager.GetInstance().             EvidenceOpen;
        phaseUpgradeOpen =  PhaseManager.GetInstance().             UpgradeOpen;
        phaseInteriorOpen = PhaseManager.GetInstance().             InteriorOpen;

        distributedNames = TextManager.GetInstance().DistributedNames;
    }
}
