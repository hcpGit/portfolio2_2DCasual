using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryShow : MonoBehaviour {
    public GameObject questDetailPan;
    [Header("BtnPrefabs")]
    public GameObject itemBtn;
    public GameObject inventoryWeaponBtn;
    public GameObject questBtn;
    public GameObject questDetailhunterBtn;
    [Header("Contents")]
    public GameObject questContents;
    public GameObject mobEviInvenContents;
    public GameObject weaponInvenContents;
    public GameObject questDetailMobEviContents;
    public GameObject questDetailHunterContents;

    [Header("UI General Texts")]
    [SerializeField]
    string uiGeneralUIName = "inven";
    public Text eviShowTxt;
    public Text weaponShowTxt;
    public Text questShowTxt;
    public Text questCtxShowTxt;
    public Text questHunterShowTxt;

    GameObject selected = null;
    Color selectedColor = new Color(190f / 255f, 240f / 255f, 190f / 255f, 255f / 255f);
  //  Vector2 questDetailOutPos = new Vector2(2137, 786);
   // Vector2 questDetailInPos = new Vector2(944, 786);

    private void Awake()
    {
        ClearAllBtns();
        this.gameObject.SetActive(false);

        eviShowTxt.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "evi");
        weaponShowTxt.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "weapon");
        questShowTxt.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "qlist");
        questCtxShowTxt.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "qctx");
        questHunterShowTxt.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "qhunters");
    }

    void InitInventoryShow()
    {
        questDetailPan.SetActive(false);
        selected = null;
        List<QuestPerMob> mobEviInven =  Inventory.GetInstance().GetNowMobEvidencesInven();
        List<Inventory.weaponInven> nowWeaponInven =  Inventory.GetInstance().WeaponInventory;
        List<Quest> quests = QuestManager.GetInstance().GetNowWrittenQuestList();

        for (int i = 0; i < quests.Count; i++)
        {
            GameObject temp = Instantiate(questBtn, questContents.transform);
            string key = quests[i].Key;
            temp.GetComponent<Button>().onClick.AddListener(() => {
                OnClickQuestBtn(temp,key);
            });
            SetQuestBtnTexts(temp , quests[i]);
        }
        for (int i = 0; i < mobEviInven.Count; i++) {
            GameObject temp = Instantiate(itemBtn, mobEviInvenContents.transform);
            SetMobEviBtnTexts(temp, mobEviInven[i]);
            Destroy(temp.GetComponent<Button>());
        }
        for (int i = 0; i < nowWeaponInven.Count; i++)
        {
            GameObject temp = Instantiate(inventoryWeaponBtn, weaponInvenContents.transform);
            SetWeaponBtnTexts(temp, nowWeaponInven[i].weapon , nowWeaponInven[i].number);
            Destroy(temp.GetComponent<Button>());
        }
    }

    void OnClickQuestBtn(GameObject btn, string key)
    {
        AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.SLIDE);
        if (selected == null)
        {
            selected = btn;
            btn.GetComponent<Image>().color = selectedColor;
            questDetailPan.SetActive(true);
        }
        else if (selected == btn)
        {
            //디테일 패널 접는 로직. 2137 에서 944 임 와이는 786
            ClearQuestDetailBtns();
            questDetailPan.SetActive(false);
            btn.GetComponent<Image>().color = Color.white;
            selected = null;
            return;
        }
        else {
            questDetailPan.SetActive(true);
            selected.GetComponent<Image>().color = Color.white;
            btn.GetComponent<Image>().color = selectedColor;
            selected = btn;
        }



        ClearQuestDetailBtns();
        Quest quest = QuestManager.GetInstance().GetQuest(key);

        List<QuestPerMob> questContext = quest.QuestList;
        List<HunterIdea> hunters = CharactorManager.GetInstance().GetHuntersByQuestKey(key);

        for (int i = 0; i < questContext.Count; i++)
        {
            GameObject temp = Instantiate(itemBtn, questDetailMobEviContents.transform);
            SetMobEviBtnTexts(temp, questContext[i]);
            Destroy(temp.GetComponent<Button>());
        }
        for (int i = 0; i < hunters.Count; i++) {
            if (hunters[i] == null) continue;
            GameObject temp = Instantiate(questDetailhunterBtn, questDetailHunterContents.transform);
            SetQDHunterBtnTexts(temp, hunters[i]);
            Destroy(temp.GetComponent<Button>());
        }
    }


    void SetQuestBtnTexts(GameObject btn , Quest quest) {
        btn.transform.GetChild(0).GetComponent<Text>().text = quest.ClientName;
        btn.transform.GetChild(1).GetComponent<Text>().text = quest.GetWeight().ToString();
        InGameTime expireDate = quest.ExpireDate;   //헌터의 만기일인 하루전으로 셋팅해야함.
        btn.transform.GetChild(2).GetComponent<Text>().text = LanguageManager.GetInstance().GetDateString(expireDate);
        /*
        Debug.LogFormat("퀘스트 의뢰인 이름 = {0} , 퀘스트의 만기는 ={1} , 퀘스트의 요구 능력치 = {2} , 헌터에게 적용할 하루 뺀 만기는 = {3}",
            quest.ClientName, quest.ExpireDate, quest.GetWeight().ToString(), expireDate
            );*/
        btn.transform.GetChild(4).GetComponent<Text>().text = UIGeneralTextsManager.GetUIGeneralText("qbtn", "client"); //의뢰인
        btn.transform.GetChild(5).GetComponent<Text>().text = UIGeneralTextsManager.GetUIGeneralText("qbtn", "cap"); //요구능력치
        btn.transform.GetChild(6).GetComponent<Text>().text = UIGeneralTextsManager.GetUIGeneralText("inven", "qbtnexpire"); //헌터만기
    }


    void SetMobEviBtnTexts(GameObject btn, QuestPerMob qpm) {
        btn.transform.GetChild(0).GetComponent<Image>().sprite = SpriteManager.GetInstance().GetColoredEvidenceSprite(qpm.mob, qpm.evidence); //qpm 에 맞춰서 넣어주기.
        btn.transform.GetChild(1).GetComponent<Text>().text = MobEviInfoManager.GetInstance().GetMobName(qpm.mob);
        btn.transform.GetChild(2).GetComponent<Text>().text = MobEviInfoManager.GetInstance().GetEvidenceName(qpm.evidence);
        btn.transform.GetChild(3).GetComponent<Text>().text = qpm.number.ToString();
    }
    void SetWeaponBtnTexts(GameObject btn, Weapon weapon, int number) {
        btn.transform.GetChild(0).GetComponent<Image>().sprite = SpriteManager.GetInstance().GetWeaponSprite(weapon.weaponType);
        btn.transform.GetChild(1).GetComponent<Text>().text = weapon.name;
        btn.transform.GetChild(2).GetComponent<Text>().text = UIGeneralTextsManager.GetUIGeneralText("merchantWeaponBtn", "cap");
        btn.transform.GetChild(3).GetComponent<Text>().text = weapon.PlusCapability.ToString();
        btn.transform.GetChild(4).GetComponent<Text>().text = number.ToString();
    }
    void SetQDHunterBtnTexts(GameObject btn , HunterIdea hunter) {
        btn.transform.GetChild(0).GetComponent<Text>().text = hunter.CharaName;
        btn.transform.GetChild(1).GetComponent<Text>().text = UIGeneralTextsManager.GetUIGeneralText("inven", "huntercap");
        btn.transform.GetChild(2).GetComponent<Text>().text = hunter.GetHuntCapabillityForRealhunt().ToString();
        btn.transform.GetChild(3).GetComponent<Text>().text = UIGeneralTextsManager.GetUIGeneralText("inven", "hunterday");
        btn.transform.GetChild(4).GetComponent<Text>().text = LanguageManager.GetInstance().GetDateString(hunter.QuestTakeOnDay);
        btn.transform.GetChild(5).GetComponent<Text>().text = UIGeneralTextsManager.GetUIGeneralText("inven", "rentals");

        Sprite weaponSprite = null;
        Color wcolor = new Color(1, 1, 1, 0);
        if (hunter.DidRentalWeapon())
        {
            for (int i = 0; i < 3; i++)
            {
                int num = 6 + i;
                if (i < hunter.RentalWeapon.Count)
                {
                    weaponSprite = SpriteManager.GetInstance().GetWeaponSprite(hunter.RentalWeapon[i].weaponType);
                    wcolor = Color.white;
                }
                else
                {
                    weaponSprite = null;
                    wcolor = new Color(1, 1, 1, 0);
                }
                btn.transform.GetChild(num).GetComponent<Image>().sprite = weaponSprite;
                btn.transform.GetChild(num).GetComponent<Image>().color = wcolor;
            }
        }
        else {
            btn.transform.GetChild(6).GetComponent<Image>().sprite = weaponSprite;
            btn.transform.GetChild(6).GetComponent<Image>().color = wcolor;
            btn.transform.GetChild(7).GetComponent<Image>().sprite = weaponSprite;
            btn.transform.GetChild(7).GetComponent<Image>().color = wcolor;
            btn.transform.GetChild(8).GetComponent<Image>().sprite = weaponSprite;
            btn.transform.GetChild(8).GetComponent<Image>().color = wcolor;
        }
    }

    public void Show() {

        AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.INVENOPEN);
        ClearAllBtns();
        InitInventoryShow();
        this.gameObject.SetActive(true);
    }
    public void Hide() {

        AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.INVENOPEN);
        ClearAllBtns();
        this.gameObject.SetActive(false);
    }

    void ClearAllBtns()
    {
        GameObject temp = questContents;
        for (int i = 0; i < temp.transform.childCount; i++) {
            Destroy(temp.transform.GetChild(i).gameObject);
        }
        temp = mobEviInvenContents;
        for (int i = 0; i < temp.transform.childCount; i++)
        {
            Destroy(temp.transform.GetChild(i).gameObject);
        }
        temp = weaponInvenContents;
        for (int i = 0; i < temp.transform.childCount; i++)
        {
            Destroy(temp.transform.GetChild(i).gameObject);
        }
        temp = questDetailMobEviContents;
        for (int i = 0; i < temp.transform.childCount; i++)
        {
            Destroy(temp.transform.GetChild(i).gameObject);
        }
        temp = questDetailHunterContents;
        for (int i = 0; i < temp.transform.childCount; i++)
        {
            Destroy(temp.transform.GetChild(i).gameObject);
        }
    }

    void ClearQuestDetailBtns()
    {
        GameObject temp = null;
        temp = questDetailMobEviContents;
        for (int i = 0; i < temp.transform.childCount; i++)
        {
            Destroy(temp.transform.GetChild(i).gameObject);
        }
        temp = questDetailHunterContents;
        for (int i = 0; i < temp.transform.childCount; i++)
        {
            Destroy(temp.transform.GetChild(i).gameObject);
        }
    }
}
