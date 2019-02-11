using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public enum E_Upgrade
{
    TENTH_STAMP,
    MAX
}
[System.Serializable]
public enum E_Interior
{
    SHELF,
    CHANDELIER,
    CANDLESTICK,
    AXEDECO,
    BARRIORDECO,
    CHAIR,
    MAX
}
[System.Serializable]
public enum E_Paper
{
    CERTIFICATE_GARGOYIL    =1,
    CERTIFICATE_HELLHOUND,
    CERTIFICATE_WYVERN,
    CERTIFICATE_TROLL,
    CERTIFICATE_SABRETOOTH,
    CERTIFICATE_GRIFFON,
    CERTIFICATE_MINOTAURUS,

    CERTIFICATE_CANINE,
    CERTIFICATE_LEATHER,
    CERTIFICATE_CORE,
    MAX
}

[System.Serializable]
public enum E_Language
{
    KOREAN,
    ENGLISH,
    MAX
}

[System.Serializable]
public enum E_Weapon
{
    AXE,
    BOW,
    RING_LANTERN,
    COINS,
    RUN_BOOK,
    RUNE_STONE,
    DOUBLE_MM_AXE,//쌍도끼, 디아mm 패러디
    DOUBLE_GM_SWORD,    //간장 막야 패러디
    PANGU_AXE,  //반고-부 , 반고의 도끼
    ZEUS_THUNDERBOLT,//제우스의 번개 지팡이
    EX_GULLIVER,
    MAX
}

[System.Serializable]
public enum E_RewardType
{
    ALL_PAYMENT,
    PARTIAL_PAYMENT,
    PAYMENT_DENY,
    MAX
}

[System.Serializable]
public enum E_HunterState
{
    IDLE,

    INQUIRE_QUEST,
    INQUIRE_QUEST_SELECT_START, //퀘스트 고르는 ui show
    INQUIRE_QUEST_ACCEPT,//퀘스트 선정완료. 퀘스트의 요구 능력과 비교해서 무기 대여 시퀀스 집입 결정.

    /*  무기 렌탈을 퀘스트 인콰이어때 같이 하는 걸로 뻄
    INQUIRE_QUEST_RENTAL_REQUIRE,   //무기 빌려달라고

    INQUIRE_QUEST_RENTAL_START, //무기 고르는 ui show.
    */

    INQUIRE_QUEST_CANCEL,

    HUNT_REWARD,    //보상 받으러온 처음 인삿말.

    //무기 반납 시퀀스부터 하고
    HUNT_REWARD_RETURN_OF_RENTAL_NOT_BROKEN,
    HUNT_REWARD_RETURN_OF_RENTAL_BROKEN,

    //만기 언질하고

    HUNT_REWARD_EXPIRED,

    //의뢰품 ui시퀀스.

    HUNT_REWARD_START,

    HUNT_REWARD_ALL_PAYMENT,
    HUNT_REWARD_PARTIAL_PAYMENT,
    HUNT_REWARD_DENIED,
    
    LEAVE,
    MAX,
}

[System.Serializable]
public enum E_ClientState
{
    IDLE,

    COMMISSION, //의뢰 처음 인사
    COMMISSION_SAY_1,
    COMMISSION_SAY_2,
    COMMISSION_SAY_3,
    COMMISSION_MAKE_START,
    COMMISION_MAKE_DONE,//마지막 인사 용.

    COMMISSION_CANCEL,
    

    CHECK_NO_EXPIRE,
    CHECK_EXPIRED, //만기때 한번 왔다 간거.

    CHECK_START,    //qc ui show

   // CHECK_DONE, //큐씨 완성도 검사하는 단꼐

    CHECK_IMPERFACT_00,  //퀘스트 이행률 0퍼
    CHECK_IMPERFACT_50,    //퀘스트 이행률 1~ 50 퍼
    CHECK_IMPERFACT_80,    // ~80퍼
    CHECK_IMPERFACT_95,     //~95퍼
    CHECK_PERFACT,  //100퍼
    
    LEAVE,

    MAX
}



[System.Serializable]
public enum E_Charactor
{
    CLIENT,
    HUNTER,

    MAX
  //  OWL,//부엉이와 마녀는 결국 넣지를 못하네.
 //   WITCH
}
[System.Serializable]
public enum E_Personality
{
    MILD,   //온화한
    AGGRESSIVE, //호전적인 (말투가 거칢)
    CHEAP,//쪼잔함?
    TIMID,//소심한?

    MAX
}


[System.Serializable]
public enum E_Monster   //변경불가.
{
    GOBLIN,
    GARGOYLE,
    HELLHOUND,
    WYVERN,

    TROLL,
    SABRETOOTH,//검치호
    GRIFFON,    //그리폰
    MINOTAURUS,

    //8종류.
    MAX
}

[System.Serializable]
public enum E_Evidence  //변경불가.
{
    FINGER,
    CANINE,
    LEATHER,
    CORE,
    //4종류
    MAX
}

[System.Serializable]
public class QuestPerMob
{
    [SerializeField]
    public E_Monster mob;
    [SerializeField]
    public E_Evidence evidence;
    [SerializeField]
    public int number;

    public float GetMobEviTypeWeight()
    {
        if (number <= 0) return 0;

        float result = 0;
        switch (mob)
        {
            case E_Monster.GOBLIN:
                result = MobEviInfoManager.GetInstance().GetMobWeight(E_Monster.GOBLIN);
                break;
            case E_Monster.GARGOYLE:
                result = MobEviInfoManager.GetInstance().GetMobWeight(E_Monster.GARGOYLE);
                break;
            case E_Monster.HELLHOUND:
                result = MobEviInfoManager.GetInstance().GetMobWeight(E_Monster.HELLHOUND);
                break;
            case E_Monster.WYVERN:
                result = MobEviInfoManager.GetInstance().GetMobWeight(E_Monster.WYVERN);
                break;
            case E_Monster.TROLL:
                result = MobEviInfoManager.GetInstance().GetMobWeight(E_Monster.TROLL);
                break;
            case E_Monster.SABRETOOTH:
                result = MobEviInfoManager.GetInstance().GetMobWeight(E_Monster.SABRETOOTH);
                break;
            case E_Monster.GRIFFON:
                result = MobEviInfoManager.GetInstance().GetMobWeight(E_Monster.GRIFFON);
                break;
            case E_Monster.MINOTAURUS:
                result = MobEviInfoManager.GetInstance().GetMobWeight(E_Monster.MINOTAURUS);
                break;

        }

        switch (evidence)
        {
            case E_Evidence.FINGER:
                result *= MobEviInfoManager.GetInstance().GetEviWeight(E_Evidence.FINGER); 
                break;
            case E_Evidence.CANINE:
                result *= MobEviInfoManager.GetInstance().GetEviWeight(E_Evidence.CANINE);
                break;
            case E_Evidence.LEATHER:
                result *= MobEviInfoManager.GetInstance().GetEviWeight(E_Evidence.LEATHER);
                break;
            case E_Evidence.CORE:
                result *= MobEviInfoManager.GetInstance().GetEviWeight(E_Evidence.CORE);
                break;
        }
        return result;

    }

    public float GetWeight()
    {
        if (number <= 0) return 0;
        
        return GetMobEviTypeWeight() * number;
    }
    

    public QuestPerMob(E_Monster mob, E_Evidence evidence, int num)
    {
        this.mob = mob;
        this.evidence = evidence;
        number = num;
    }
    public bool IsIt(E_Monster monster, E_Evidence evidence1)
    {
        if (mob == monster && evidence == evidence1)
            return true;
        else return false;
    }
    public override string ToString()
    {
        return "qpm[" + mob.ToString() + "-" + evidence.ToString() + "-" + number.ToString() + "]";
    }
}


public class DataST
{

}
