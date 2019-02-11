using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QMakeUIManager : MonoBehaviour {

    static QMakeUIManager instance;

    class RadioQM
    {
        Dictionary<string, Button> radioDic = new Dictionary<string, Button>();
        Button choosedBtn;

        public string GetChoosedBtnName()
        {
            return choosedBtn.name;
        }

        public bool IsReady()
        {
            if(choosedBtn==null) return false;
            return true;
        }

        public void AddRadioList(Button btn)
        {
            radioDic.Add(btn.name, btn);
        }

        public bool SetBtn(Button btn)
        {
            if (false == radioDic.ContainsKey(btn.name))
                return false;

            if (choosedBtn == null)
            {
                SetAsChoosed(btn);
                return true;
            }

            if (choosedBtn == btn) return true;

            CancelChoosed();
            SetAsChoosed(btn);

            return true;
        }

        public void CancelChoosed()
        {
            if (choosedBtn == null) return;
            choosedBtn.GetComponent<Image>().color = Color.white;
            //어떠고 처리해주기.
            choosedBtn = null;
        }
        public void SetAsChoosed(Button btn)
        {
            if (false == radioDic.ContainsKey(btn.name))
                Debug.LogError("에러!");

            choosedBtn = btn;
            choosedBtn.GetComponent<Image>().color = Color.yellow;
        }
    }

    public GameObject QMCanvas;
    public Text numText;
    RadioQM mobStampRadio;
    RadioQM evidenceStampRadio;
    public Button stampButton;
    public int number;
    public Button completeButton;

    [Header("UI General Texts")]
    [SerializeField]
    string uiGeneralUIName = "cqm";
    public Text dumpBtnText;
    public Text oneMoreBtnText;
    public Text completeBtnText;
    public Text stampBtnText;

    public static QMakeUIManager GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;

        dumpBtnText.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "dump");
        oneMoreBtnText.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "onemore");
        completeBtnText.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "complete");
        stampBtnText.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "stamp");
        
        mobStampRadio = new RadioQM();
        evidenceStampRadio = new RadioQM();
        
        Button[] btns = QMCanvas.GetComponentsInChildren<Button>();
        foreach (Button btn in btns)
        {
            switch (btn.name)
            {
                case "GoblinStampTool":
                    mobStampRadio.AddRadioList(btn);
                    break;

                case "GargoyleStampTool":
                    if (false == PhaseManager.GetInstance().IsOpen(E_Monster.GARGOYLE)) //인가증 유무에 따라 다르게.
                        btn.interactable = false;
                    else mobStampRadio.AddRadioList(btn);
                    break;
                case "HellhoundStampTool":
                    if (false == PhaseManager.GetInstance().IsOpen(E_Monster.HELLHOUND)) //인가증 유무에 따라 다르게.
                        btn.interactable = false;
                    else mobStampRadio.AddRadioList(btn);
                    break;
                case "WyvernStampTool":
                    if (false == PhaseManager.GetInstance().IsOpen(E_Monster.WYVERN)) //인가증 유무에 따라 다르게.
                        btn.interactable = false;
                    else mobStampRadio.AddRadioList(btn);
                    break;
                case "TrollStampTool":
                    if (false == PhaseManager.GetInstance().IsOpen(E_Monster.TROLL)) //인가증 유무에 따라 다르게.
                        btn.interactable = false;
                    else mobStampRadio.AddRadioList(btn);
                    break;
                case "SabretoothStampTool":
                    if (false == PhaseManager.GetInstance().IsOpen(E_Monster.SABRETOOTH)) //인가증 유무에 따라 다르게.
                        btn.interactable = false;
                    else mobStampRadio.AddRadioList(btn);
                    break;
                case "GriffonStampTool":
                    if (false == PhaseManager.GetInstance().IsOpen(E_Monster.GRIFFON)) //인가증 유무에 따라 다르게.
                        btn.interactable = false;
                    else mobStampRadio.AddRadioList(btn);
                    break;
                case "MinotaurusStampTool":
                    if (false == PhaseManager.GetInstance().IsOpen(E_Monster.MINOTAURUS)) //인가증 유무에 따라 다르게.
                        btn.interactable = false;
                    else mobStampRadio.AddRadioList(btn);
                    break;
                    

                case "FingerStampTool":
                    evidenceStampRadio.AddRadioList(btn);
                    break;
                case "CanineStampTool":
                    if (false == PhaseManager.GetInstance().IsOpen(E_Evidence.CANINE)) //인가증 유무에 따라 다르게.
                        btn.interactable = false;
                    else evidenceStampRadio.AddRadioList(btn);
                    break;
                case "LeatherStampTool":
                    if (false == PhaseManager.GetInstance().IsOpen(E_Evidence.LEATHER)) //인가증 유무에 따라 다르게.
                        btn.interactable = false;
                    else evidenceStampRadio.AddRadioList(btn);
                    break;
                case "CoreStampTool":
                    if (false == PhaseManager.GetInstance().IsOpen(E_Evidence.CORE)) //인가증 유무에 따라 다르게.
                        btn.interactable = false;
                    else evidenceStampRadio.AddRadioList(btn);
                    break;
            }
        }
        ResetQMUI();
    }

   
    public void QM_StampTool_OnClick(Button btn)
    {
        AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.CLICK);
        //각 라디오에 이 버튼이 걸려있는지 체크.
        if (mobStampRadio.SetBtn(btn))
        {
            StampBtnInteractJudge();
            return;
        }
        if (evidenceStampRadio.SetBtn(btn))
        {
            StampBtnInteractJudge();
            return;
        }
        Debug.LogError("스탬프온클릭 설정 오류!!");
    }

    public void NumAdjust(bool up)
    {
        AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.CLICK);
        if (false == PhaseManager.GetInstance().IsOpen(E_Upgrade.TENTH_STAMP))    //십의 자릿수 도장 찍기를 아직 획득하지 못한.
        {
            if (up)
            {
                if (number < 9)
                    number++;
            }
            else
            {
                if (number > 0)
                    number--;
            }
        }

        else {
            //십의 자릿수 개방이 되어있으면 99 까지 찍기 가능.
            if (up)
            {
                if (number < 99)
                    number++;
            }
            else
            {
                if (number > 0)
                    number--;
            }
        }

        numText.text = number.ToString();
        StampBtnInteractJudge();
    }


    public void DumpParchment()
    {
        AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.INVENOPEN);
        HandManager.GetInstance().DumpParchment();
    }
    public void CompleteParchment()
    {
        AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.UNDERTAKE);
        HandManager.GetInstance().CompleteParchment();
    }
    public void OneMoreParchment()
    {
        AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.INVENOPEN);
        HandManager.GetInstance().OneMoreParchment();
    }


    public void StampWhaam()//스탬프꽝
    {
        if (mobStampRadio.IsReady() == false
           ||
           number == 0
           ||
           evidenceStampRadio.IsReady() == false
            || HandManager.GetInstance().IsThereEmptySpaceInParchment() == false
           )
            return;
            //조건 충족 됐을때만 불러짐.

            E_Monster mob = E_Monster.GARGOYLE;
        E_Evidence evidence = E_Evidence.FINGER;
        
        switch (mobStampRadio.GetChoosedBtnName())
        {
            case "GoblinStampTool":
                mob = E_Monster.GOBLIN;
                break;
            case "GargoyleStampTool":
                mob = E_Monster.GARGOYLE;
                break;
            case "HellhoundStampTool":
                mob = E_Monster.HELLHOUND;
                break;
            case "WyvernStampTool":
                mob = E_Monster.WYVERN;
                break;
            case "TrollStampTool":
                mob = E_Monster.TROLL;
                break;
            case "SabretoothStampTool":
                mob = E_Monster.SABRETOOTH;
                break;
            case "GriffonStampTool":
                mob = E_Monster.GRIFFON;
                break;
            case "MinotaurusStampTool":
                mob = E_Monster.MINOTAURUS;
                break;
        }

        switch (evidenceStampRadio.GetChoosedBtnName())
        {
            case "FingerStampTool":
                evidence = E_Evidence.FINGER;
                break;
            case "CanineStampTool":
                evidence = E_Evidence.CANINE;
                break;
            case "LeatherStampTool":
                evidence = E_Evidence.LEATHER;
                break;
            case "CoreStampTool":
                evidence = E_Evidence.CORE;
                break;
        }
        AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.CLICK);
        HandManager.GetInstance().StampParchment(mob, evidence, number);

        ResetQMUI();
        completeButton.interactable = true;
    }

    public void ResetQMUI()
    {
        number = 0;
        numText.text = number.ToString();
        mobStampRadio.CancelChoosed();
        evidenceStampRadio.CancelChoosed();
        stampButton.interactable = false; //버튼 비활성화
    }

    void StampBtnInteractJudge()
    {
        if (mobStampRadio.IsReady() == false
            ||
            number == 0
            ||
            evidenceStampRadio.IsReady() == false
            || HandManager.GetInstance().IsThereEmptySpaceInParchment() ==false
            )
        {
            stampButton.interactable = false;
            return;
        }
        else stampButton.interactable = true;

    }

    public void HideQMUI()
    {
        QMCanvas.SetActive(false);
        completeButton.interactable = false;
    }
    public void ShowQMUI()
    {
        QMCanvas.SetActive(true);
        completeButton.interactable = false;
        ResetQMUI();
    }

}
