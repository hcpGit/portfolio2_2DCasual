using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class HunterQIManager : MonoBehaviour {

    static HunterQIManager instance;

    class QuestRadio
    {
        Dictionary<int, Quest> qDic = new Dictionary<int, Quest>();

        int? selected = null;
        public void SetQDic(int childNum, Quest quest)
        {
            if (quest == null) Debug.Log("셋큐딕 퀘스트가 널이야.");
            qDic.Add(childNum, quest);
        }
        public void SelectIQBtn(int originNumber , GameObject qlContent , System.Action spreadAction)
        {
            if (selected != null && originNumber == selected)   //같은번호가 선택됐다면.
            {
                DestroyImmediate
                   // Destroy
                (qlContent.transform.GetChild((int)selected + 1).gameObject);
                selected = null;
                HunterQIManager.GetInstance().AdjustQuestCap(0);
                return;
            }
            
            if (selected != null)  
                //이미 선택된게 있었으면 뿌시기. 
            {
                DestroyImmediate
                //Destroy
                    (qlContent.transform.GetChild((int)selected + 1).gameObject);
            }

            selected = originNumber;
            spreadAction();
            HunterQIManager.GetInstance().AdjustQuestCap(qDic[originNumber].GetWeight());
        }
        public string GetSelectedQuestKey()
        {
            if (selected == null) Debug.LogError("퀘 선택 안된 채로 제출 버튼 클릭");
            return qDic[(int)selected].Key;
        }
    }
    
    class rentalWeaponCombo
    {
        class rentalWeaponAndBtn
        {
            public GameObject btn;
            public Weapon weapon;
            public rentalWeaponAndBtn(GameObject btnGo, Weapon weaponItSelf) { btn = btnGo; weapon = weaponItSelf; }
        }

        int equipCap;    //헌터가 착용할 수 있는 무기 갯수 
        
        Color selectColor = new Color(190f / 255f, 240f / 255f, 190f / 255f, 255f / 255f);

        Dictionary<int, rentalWeaponAndBtn> weaponDic;
        List<int> selected;

        public rentalWeaponCombo(int cap)
        {
            equipCap = cap;
            selected = new List<int>();
            weaponDic = new Dictionary<int, rentalWeaponAndBtn>();
        }

        public void SetWeaponCombo(int originNumber , GameObject weaponBtn, Weapon weapon)
        {
            if (weaponDic.ContainsKey(originNumber))
            {
                Debug.LogError("웨폰 콤보 셋업 실수!");
                return;
            }
            weaponDic.Add(originNumber, new rentalWeaponAndBtn(weaponBtn, weapon));
        }

        public void SelectRentalWeapon(int newSelected)
        {
            if (alreadySelected(newSelected))//이미 있는 무기를 선택(무기 취소.)
            {
                HunterQIManager.GetInstance().AdjustHunterCap(  -1  * weaponDic[newSelected].weapon.PlusCapability);
                CancelSelected(newSelected);
                return;
            }
            if (selected.Count >= equipCap)
            {
                return; //장착 가능한 무기가 꽉찼어.
            }
            //새롭게 무기를 선택함.
            HunterQIManager.GetInstance().AdjustHunterCap(weaponDic[newSelected].weapon.PlusCapability);
            NewSelectBtn(newSelected);
        }
        
        public List<Weapon> GetSelectedWeapons()
        {
            if (selected.Count == 0) return null;
            List<Weapon> temp = new List<Weapon>();
            for (int i = 0; i < selected.Count; i++)
            {
                temp.Add(weaponDic[selected[i]].weapon);
            }
            return temp;
        }

        void NewSelectBtn(int newSelect)
        {
            selected.Add(newSelect);
            weaponDic[newSelect].btn.GetComponent<Image>().color = selectColor;
        }

        bool alreadySelected(int newSelected)
        {
            if (selected.Contains(newSelected)) return true;
            return false;
        }
        void CancelSelected(int cancelNum)
        {
            selected.Remove(cancelNum);
            weaponDic[cancelNum].btn.GetComponent<Image>().color = Color.white;
        }
    }

    public GameObject questsListView;
    public GameObject questsListContent;
    public GameObject iquestBtn;
    public GameObject iqDetailEviPanel; //컨텐츠에 패널을 자식으로 두고 (선택된 퀘스트 다음 순서 자식으로)
    //패널에 보드를 자식으로 두고 one을 보드의 자식으로 둠 (보드엔 2개가 올라가고, 다음 보드 생성.)
    public GameObject iqDetailEviBoard;
    public GameObject iqDetailEviOne;
    QuestRadio qradio = new QuestRadio();

    public Text hunterNameText;
    public Text hunterCapText;
    public Text questCapText;
    public Image hunterImage;
    public Text hunterPlusCapbyWeaponStatTxt;

    float hunterCap;
    float hunterCapPlusByWeaponRental;
    float questCap = 0;

    public GameObject rentalWeaponListViewContents;
    public GameObject weaponBtn;
    rentalWeaponCombo rCombo;

    public Button choiceButton;
    public Image choiceButtonImg;
    public Text choiceButtonTxt;
    Color interFalse = new Color(200f / 255f, 200f / 255f, 200f / 255f, 128f / 255f);

    public Text EquipText;

    [Header("UI General Texts")]
    [SerializeField]
    string uiGeneralUIName = "hiq";
    public Text rentableWeaponShowTxt;
    public Text equipSlotShowTxt;
    public Text hunterCapShowTxt;
    public Text questCapShowTxt;
    public Text questListShowTxt;
    public Text choiceBtnShowTxt;
    public Text cancelBtnShowTxt;

    List<Quest> hunterAbleQuest;

    void Awake()
    {
        instance = this;
        rentableWeaponShowTxt.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "rentableweapon");
        equipSlotShowTxt.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "equipslot");
        hunterCapShowTxt.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "huntercap");
        questCapShowTxt.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "questCap");
        questListShowTxt.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "questlist");
        choiceBtnShowTxt.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "choicebtn");
        cancelBtnShowTxt.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "cancelbtn");
    }
    public static HunterQIManager GetInstance()
    {
        return instance;
    }

    void InteractiveSubmit(bool tf)
    {
        if (tf == false)
        {
            choiceButton.interactable = false;
            choiceButtonImg.color = interFalse;
            choiceButtonTxt.color = interFalse;
        }
        else {
            choiceButton.interactable = true;
            choiceButtonImg.color = Color.white;
            choiceButtonTxt.color = Color.black;
        }
    }

    public void ShowHunterInquireQuestUI()
    {
        AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.INVENOPEN);
        this.gameObject.SetActive(true);
        qradio = new QuestRadio();
        hunterCap = 0;
        InitHunterStat();
        InitQuestList();
        InitRentalWeaponList();
        InteractiveSubmit(false);
        hunterPlusCapbyWeaponStatTxt.text = "";
        hunterCapText.color = Color.black;
    }
    public void HideHQIUI()
    {
        this.gameObject.SetActive(false);
    }
    public void ChoiceBtnOnClick()
    {
        AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.UNDERTAKE);
        InteractiveManager.GetInstance().HunterInquireQuestSubmit(qradio.GetSelectedQuestKey(), true,rCombo.GetSelectedWeapons());
        HideHQIUI();
    }
    public void CancelBtnOnClick()
    {
        AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.CLICK);
        InteractiveManager.GetInstance().HunterInquireQuestSubmit("", false, null);
        HideHQIUI();
    }

    /*
     헌터 능력치 텍스트 변화부
         */
    public void AdjustHunterCap(float capByWeapon)
    {
        //마이너스면 선택된게 빠진거고 뭐 그런거지.
        //풀로스면 새로 선택된거고
        hunterCapPlusByWeaponRental += capByWeapon;
        
        hunterCapText.text = (hunterCap+ hunterCapPlusByWeaponRental).ToString();

        if(hunterCapPlusByWeaponRental>0)
        hunterPlusCapbyWeaponStatTxt.text = "+" + hunterCapPlusByWeaponRental.ToString();
        else
            hunterPlusCapbyWeaponStatTxt.text = "";
        //나중에 이 추가된 능력치랑 등등 플러스해서 색갈 차이 나타내주기.

     //   Debug.Log("현재 퀘스트 캐퍼블리티 = "+questCap);

        if (questCap == 0)
        {
            hunterCapText.color = Color.black;
        }
        else if (hunterCap + hunterCapPlusByWeaponRental < questCap)
        {
            hunterCapText.color = Color.red;
        }
        else
        {
            hunterCapText.color = Color.blue;
        }

    }

    /*
     퀘스트 요구 능력치 텍스트 변화부
         */
    public void AdjustQuestCap(float requiredCap)
    {
        questCap = requiredCap;
        if (requiredCap == 0)   //초기화
        {
            hunterCapText.color = Color.black;
            questCapText.text = "";

            InteractiveSubmit(false);

            return;
        }
        InteractiveSubmit(true);
        

        if (hunterCap + hunterCapPlusByWeaponRental < requiredCap)
        {
            hunterCapText.color = Color.red;
        }
        else
        {
            hunterCapText.color = Color.blue;
        }
        questCapText.text = requiredCap.ToString();
    }

    /*
     헌터의 스텟 세팅
         */
    void InitHunterStat()
    {
        HunterIdea  hunter = CharactorFrame.GetInstance().hunterIdea;
        hunterImage.sprite = hunter.CharaSprite;
        hunterImage.preserveAspect = true;
        hunterNameText.text = hunter.CharaName;
        hunterCapText.text = hunter.HuntingCapabillity.ToString();
        questCapText.text = "";
        hunterCap = hunter.HuntingCapabillity;
        hunterCapPlusByWeaponRental = 0;
    }

    /*
     퀘스트 목록 셑팅
         */
    void InitQuestList()
    {
        questCap = 0;

        for (int i = 0; i < questsListContent.transform.childCount; i++)
        {
            Destroy(questsListContent.transform.GetChild(i).gameObject);
        }

        List<Quest> nowQuests = QuestManager.GetInstance().GetNowWrittenQuestList();

        InGameTime nowTime = InGameTimeManager.GetInstance().GetNowTime();

        var hq = from q in nowQuests
                 where q.ExpireDate.GetDays() > nowTime.GetDays()+1   //현재 일 보다 이틀 많은 일 수 (헌터의 만기는 의뢰인의 만기보다 하루 빨라야함.)
                 select  q;

        List<Quest> hunterAbleQuesttemp = hq.ToList<Quest>(); //이 헌터가 수임 가능한 퀘스트만 고른것 (만기 상태가)
        hunterAbleQuest = new List<Quest>();

        for (int i = 0; i < hunterAbleQuesttemp.Count; i++)
            hunterAbleQuest.Add(hunterAbleQuesttemp[i]);

        for (int i = 0; i < hunterAbleQuest.Count; i++)
        {
            //대체가 도대체 어째서 i를 쓰면 0 번째 자리에서도 i 맨 마지막이 들어가는 거야????????
            int num = i;
            GameObject iqBtnTemp = Instantiate(iquestBtn, questsListContent.transform);
            SetTxtsIQBtn(hunterAbleQuest[num], iqBtnTemp);  //퀘스트 버튼.

            Quest a = hunterAbleQuest[num];
            
            iqBtnTemp.GetComponent<Button>().onClick.AddListener(
                () => 
                {
                    qradio.SelectIQBtn(
                        num, questsListContent, 
                        () =>
                        {
                            AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.SLIDE);
                            //    Debug.Log(i + "번째 온클릭 란다식 메소드 진입."
                            //      + hunterAbleQuest[num].ToString()
                            //       );
                            OnClickIQBtn(hunterAbleQuest[num], num);    //왜 이 헌터에이블에 접근을 못하지?
                            //도대체 왜 a 대신 hunterAbleQuest[i] 로 들어가면 접근을 못핮;???????????????????
                        }
                    );
                }
                );

            qradio.SetQDic(num, hunterAbleQuest[num]);
        }
    }

    /*
     렌탈할 수 있는 무기들을 보여줌.
         */
    void InitRentalWeaponList()
    {
        for (int i = 0; i < rentalWeaponListViewContents.transform.childCount; i++)
        {
            Destroy(rentalWeaponListViewContents.transform.GetChild(i).gameObject);
        }

        HunterIdea hunter = CharactorFrame.GetInstance().hunterIdea;
        rCombo = new rentalWeaponCombo(hunter.EquipCap);

        EquipText.text = hunter.EquipCap.ToString();

        List<Weapon> nowInven = Inventory.GetInstance().GetWeaponInvenTypes();//현재 갖고있는 무기 중 하나씩만 가져옴.
        
        for (int i = 0; i < nowInven.Count; i++)
        { 
            int num = i;
            GameObject temp = Instantiate(weaponBtn, rentalWeaponListViewContents.transform);
            SetTxtsWeaponBtn(nowInven[i], temp);
            temp.GetComponent<Button>().onClick.AddListener(
                () => {
                    AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.CLICK);
                    rCombo.SelectRentalWeapon(num);
                }
                );
            rCombo.SetWeaponCombo(num, temp, nowInven[i]);
        }
    }

    /*
     퀘스트의 세부사항 보여주는 메소드
         */
    void  OnClickIQBtn(Quest quest,int iqBtnsChildNumber)
    {
     //   Debug.Log(quest.ToString() + "온클릭" + iqBtnsChildNumber);
        List<QuestPerMob> qpmList = quest.QuestList;
        GameObject detailEviPanel = Instantiate(iqDetailEviPanel, questsListContent.transform);
        detailEviPanel.transform.SetSiblingIndex(iqBtnsChildNumber + 1);//선택된 퀘스트 버튼 아래에 디테일 증거품 리스트 생성.

        int boardCount = 0;
        GameObject eviBoard = null;

        for (int i = 0; i < qpmList.Count; i++)
        {
            if(boardCount==0)
            eviBoard = Instantiate(iqDetailEviBoard, detailEviPanel.transform);

            GameObject eviOne = Instantiate(iqDetailEviOne, eviBoard.transform);
            SetTxtsOnEviOne(qpmList[i], eviOne);

            boardCount++;
            if (boardCount >= 2) boardCount = 0;    //보드에 2개만.
        }
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(questsListContent. transform as RectTransform); //레이아웃 리뉴 해줌.
        LayoutRebuilder.ForceRebuildLayoutImmediate(questsListView.transform as RectTransform);
    }

    void SetTxtsIQBtn(Quest quest , GameObject iqBtnGo) //나중에 퀘스트 버튼의 제너럴 텍스트를 맞춰줘야함.
    {
        iqBtnGo.transform.GetChild(0).GetComponent<Text>().text = quest.ClientName;
        iqBtnGo.transform.GetChild(1).GetComponent<Text>().text = quest.GetWeight().ToString();
        InGameTime expireDate = quest.ExpireDate;   //헌터의 만기일인 하루전으로 셋팅해야함.
        expireDate = InGameTime.GetOnlyOneDayMinus(expireDate);

        iqBtnGo.transform.GetChild(2).GetComponent<Text>().text = LanguageManager.GetInstance().GetDateString(expireDate);

        Debug.LogFormat("퀘스트 의뢰인 이름 = {0} , 퀘스트의 만기는 ={1} , 퀘스트의 요구 능력치 = {2} , 헌터에게 적용할 하루 뺀 만기는 = {3}",
            quest.ClientName, quest.ExpireDate, quest.GetWeight().ToString() , expireDate 
            );
        iqBtnGo.transform.GetChild(4).GetComponent<Text>().text = UIGeneralTextsManager.GetUIGeneralText("qbtn", "client"); //의뢰인
            iqBtnGo.transform.GetChild(5).GetComponent<Text>().text = UIGeneralTextsManager.GetUIGeneralText("qbtn", "cap"); //요구능력치
        iqBtnGo.transform.GetChild(6).GetComponent<Text>().text = UIGeneralTextsManager.GetUIGeneralText("qbtn", "expire"); //헌터만기
    }
    void SetTxtsOnEviOne(QuestPerMob qpm, GameObject eviOne)
    {
        eviOne.transform.GetChild(1).GetComponent<Image>().sprite = SpriteManager.GetInstance().GetColoredEvidenceSprite(qpm.mob,qpm.evidence);
        eviOne.transform.GetChild(2).GetComponent<Text>().text =  MobEviInfoManager.GetInstance().GetMobName(qpm.mob);
        eviOne.transform.GetChild(3).GetComponent<Text>().text = MobEviInfoManager.GetInstance().GetEvidenceName(qpm.evidence);
        eviOne.transform.GetChild(4).GetComponent<Text>().text = qpm.number.ToString();
    }
    void SetTxtsWeaponBtn(Weapon weapon, GameObject btn)
    {
        btn.transform.GetChild(0).GetComponent<Image>().sprite = SpriteManager.GetInstance().GetWeaponSprite( weapon.weaponType);
        btn.transform.GetChild(1).GetComponent<Text>().text = weapon.name;
        btn.transform.GetChild(2).GetComponent<Text>().text = UIGeneralTextsManager.GetUIGeneralText("merchantWeaponBtn", "cap");
        btn.transform.GetChild(3).GetComponent<Text>().text = weapon.PlusCapability.ToString();
    }
    
}
