using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestRoomSceneUIManager : Singleton<QuestRoomSceneUIManager> {
    public void OnlyShowMainUIOrigin()
    {
        QMakeUIManager.GetInstance().HideQMUI();
        HunterRewardUIManager.GetInstance().HideHRUI();
        HunterQIManager.GetInstance().HideHQIUI();
        QuestCheckManager.GetInstance().HideQCUI();

        MainUIManager.GetInstance().ShowOrigin();   //대화창만 가리는 거 보여줌,.
    }
    public void OnlyShowMainUITalk()
    {
        QMakeUIManager.GetInstance().HideQMUI();
        HunterRewardUIManager.GetInstance().HideHRUI();
        HunterQIManager.GetInstance().HideHQIUI();
        QuestCheckManager.GetInstance().HideQCUI();

        MainUIManager.GetInstance().ShowOrigin();
        MainUIManager.GetInstance().ShowCharaTextPanel();
        MainUIManager.GetInstance().ShowPlayerTextPanel();
    }


    public void OnlyShowQMUI()
    {
        HunterRewardUIManager.GetInstance().HideHRUI();
        HunterQIManager.GetInstance().HideHQIUI();
        QuestCheckManager.GetInstance().HideQCUI();
        MainUIManager.GetInstance().HideMainCanvas();
        QMakeUIManager.GetInstance().ShowQMUI();
    }
    public void OnlyShowQCUI()
    {
        QMakeUIManager.GetInstance().HideQMUI();
        HunterRewardUIManager.GetInstance().HideHRUI();
        HunterQIManager.GetInstance().HideHQIUI();
        MainUIManager.GetInstance().ShowOrigin();   //그 인벤토리 패널 같은 거 어떻게 해줄깡.
        QuestCheckManager.GetInstance().ShowQCUI();
    }
    public void OnlyShowHIQUI()
    {
        QMakeUIManager.GetInstance().HideQMUI();
        HunterRewardUIManager.GetInstance().HideHRUI();
        HunterQIManager.GetInstance().ShowHunterInquireQuestUI();
        MainUIManager.GetInstance().ShowOnlyTimePanel();   //그 인벤토리 패널 같은 거도 가려주기.
        QuestCheckManager.GetInstance().HideQCUI();
    }
    public void OnlyShowHRUI()
    {
        QMakeUIManager.GetInstance().HideQMUI();
        HunterRewardUIManager.GetInstance().ShowHRUI();
        HunterQIManager.GetInstance().HideHQIUI();
        MainUIManager.GetInstance().ShowOrigin();   //그 인벤토리 패널 같은 거도 가려주기.
        QuestCheckManager.GetInstance().HideQCUI();
    }
    public void HideAllUI()
    {
        HunterRewardUIManager.GetInstance().HideHRUI();
        HunterQIManager.GetInstance().HideHQIUI();
        QuestCheckManager.GetInstance().HideQCUI();
        MainUIManager.GetInstance().HideMainCanvas();
        QMakeUIManager.GetInstance().HideQMUI();
    }
}
