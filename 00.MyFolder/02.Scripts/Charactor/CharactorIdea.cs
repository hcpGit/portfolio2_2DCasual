using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public abstract class CharactorIdea
{
    public virtual void SetupAI(FSMController cont) { }

    public virtual void AnamnesisToCharactorFrame()
    {
        CharactorFrame.GetInstance().ResetIdeas();
        SetupAI(CharactorFrame.GetInstance().CharaController);
        CharactorFrame.GetInstance().ChangeLook(charaSprite,charaName);
    }
    [SerializeField]
    public bool hasCommission;
    [SerializeField]
    public bool haveComeBeforeExpire;
    [SerializeField]
    public string associatedQuestKey;

    public virtual InGameTime GetExpire() { return null; }



    [System.NonSerialized]
    protected Sprite charaSprite;
    public Sprite CharaSprite
    {
        get { return charaSprite; }
    }
    [SerializeField]
    protected int charaSpriteIdx;   //스프라이트는 저장을 못하므로 스프라이트 매니저의 캐릭터 스프라이트 인덱스로 스프라이트를 접근
    public int CharaSpriteIdx
    {
        get {
            return charaSpriteIdx;
        }
    }
    public void SetCharaSprite()
    {
        charaSprite = SpriteManager.GetInstance().GetCharactorSprite(charaSpriteIdx);
    }



    [SerializeField]
    protected E_Personality personality;
    public E_Personality Personality
    {
        get { return personality; }
    }
    [SerializeField]
    protected E_Charactor charaType;
    public E_Charactor CharaType
    {
        get { return charaType; }
    }

    [SerializeField]
    public  string charaName;
    public string CharaName
    {
        get { return charaName; }
    }

    [SerializeField]
    protected int indivisualDifference;//성격 별로도 말하는 게 다르게
    public int IndivisualDifference
    {
        get { return indivisualDifference; }
    }

    public virtual void SetHasCommission(string Qkey) {
        hasCommission = true;
        associatedQuestKey = Qkey;
    }//클라든 헌터든 의뢰를 맡기거나 수임했을때 호출해줄 함수.


}
