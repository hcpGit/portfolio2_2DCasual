using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorFrame : MonoBehaviour {

    static CharactorFrame instance;

    public bool ForDebugClient=false;

    Animator animator;
    SpriteRenderer charaSpriteRd;

    string charaName;
    public string CharaName
    {
        get {
            return charaName;
        }
    }
    
    HunterIdea hunterIdeaFrame;
    public HunterIdea hunterIdea
    {
        get {
            return hunterIdeaFrame;
        }
    }
    ClientIdea clientIdeaFrame;
    public ClientIdea clientIdea
    {
        get {
            return clientIdeaFrame;
        }
    }
    FSMController charaController = new FSMController();

    public FSMController CharaController
    {
        get {
            return charaController;
        }
    }

    void Awake()
    {
        instance = this;
        charaSpriteRd = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    public static CharactorFrame GetInstance()
    {
        return instance;
    }



    public void UpdateFSM()
    {
        charaController.UpdateFSM();
    }

    #region 이식단
    public void ResetIdeas()
    {
        clientIdeaFrame = null;
        hunterIdeaFrame = null;
    }
    public void SetClientIdea(ClientIdea cd)
    {
        clientIdeaFrame = cd;
    }
    public void SetHunterIdea(HunterIdea hd)
    {
        hunterIdeaFrame = hd;
    }
    public void ChangeLook(Sprite sp , string name)
    {
        /*
         스프라이트 크기 비율에 맞춰서 캐릭터 스프라이트가 의도된 y값 만이라도 지키게 
         */
        var factor = 3f/ sp.bounds.size.y;
        transform.localScale = new Vector3(factor, factor, factor);

        charaSpriteRd.sprite = sp;
        charaName = name;
    }

    public void ComeAnim()
    {
        animator.SetTrigger("comeToRoom");
        AudioThing.GetInstance().StartFootSteps(true);

        RoomEventManager.GetInstance(). IsNowWineAndDine = true;
    }
    public void EndOfComeAnimation()
    {
        RoomEventManager.GetInstance().EndOfComeAnimation();
       
    }

    #endregion

    public void LeaveAnim()
    {
        GetComponent<Animator>().SetTrigger("leave");
        AudioThing.GetInstance().StartFootSteps(false);
    }
    public void EndOfLeaveAnimation()
    {
        RoomEventManager.GetInstance().EndOfLeaveAnimation();
        ResetIdeas();
    }

    private void Update()
    {
        if (ForDebugClient)
        {
            ForDebugClient = false;
            Debug.Log(clientIdeaFrame.OriginOrderedQuest);
        }
    }

}
