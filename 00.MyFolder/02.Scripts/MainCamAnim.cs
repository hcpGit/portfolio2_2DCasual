using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamAnim : MonoBehaviour
{
    static MainCamAnim instance;
    Animator mainCameraAnimator;
    void Awake()
    {
        instance = this;
        mainCameraAnimator = Camera.main.GetComponent<Animator>();
    }
    public static MainCamAnim GetInstance()
    {

        return instance;
    }

    public void CamToPosMaking()    //1
    {
        QuestRoomSceneUIManager.GetInstance().HideAllUI();
        mainCameraAnimator.SetTrigger("cameraToQuestMaking");
    }

    public void PosToMakingEnd()    //1
    {
        //퀘스트 메이킹 ui 띄워주기?
        QuestRoomSceneUIManager.GetInstance().OnlyShowQMUI();
    }

    public void CamToPosNormal()    //1
    {
        QuestRoomSceneUIManager.GetInstance().HideAllUI();
        mainCameraAnimator.SetTrigger("cameraToNormalPos");
    }

    System.Action normalAct;//카메라가 노멀로 돌아온 후에 띄워주기 위해서.
    public void CamToPosNormal(System.Action action)    //1
    {
        normalAct = new System.Action(action);

        QuestRoomSceneUIManager.GetInstance().HideAllUI();
        mainCameraAnimator.SetTrigger("cameraToNormalPos");
    }

    public void PosToNormalEnd()
    {
        QuestRoomSceneUIManager.GetInstance().OnlyShowMainUIOrigin();
        normalAct();
    }
}
