using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEventManager : MonoBehaviour {
    static RoomEventManager instance;
    
    void Awake()
    {
        instance = this;
    }
    public static RoomEventManager GetInstance()
    {
        return instance;
    }

    bool isNowWineAndDine = false;  //접대 중 여부
    public bool IsNowWineAndDine
    {
        set { isNowWineAndDine = value; }
        get {
            return isNowWineAndDine;
        }
    }

    public void PlayerChoice(bool playerChoice) //버튼 콜백 메서드
    {
        EventParameterStorage.GetInstance().PlayerChoice = playerChoice;
        CharactorFrame.GetInstance().UpdateFSM();
    }

    public void EndOfComeAnimation()
    {
        QuestRoomSceneUIManager.GetInstance().OnlyShowMainUIOrigin();
        MainUIManager.GetInstance().SetCharaNameText(CharactorFrame.GetInstance().CharaName);
        CharactorFrame.GetInstance().UpdateFSM();
    }
    public void EndOfLeaveAnimation()   //캐릭터가 리브되면 불려짐 (인터렉티브 - 프레임 - 요기)
    {
        isNowWineAndDine = false;
    }
}
