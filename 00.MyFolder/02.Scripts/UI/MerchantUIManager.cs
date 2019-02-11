using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MerchantUIManager : MonoBehaviour {
    //탭 관리가 얘의 임무

    enum E_MerchantTab  //탭과 패널 꼭 이 순서대로 가있어야함.
    {
        BUY_WEAPON=0,
        BUY_UPGRADE,
        BUY_INTERIOR,
        BUY_PAPER,
        SELL,
        MAX
    }
    public GameObject buySellParent;
    public GameObject[] buyTabs;
    int tabCount;

    public GameObject tabPanelParent;
    public merchantTabBase[] tabPanels;

    int selectedTab;

    public DayOffManager dayOffUI;
    public GameObject mobShowUpScreen;
    public Image showUpMobStampedImage;
    public Text mobShowUpText;
    public Button mobShowUpExitBtn;

    [Header("UI General Texts")]
    [SerializeField]
    string uiGeneralUIName = "merchant";
    public Text weaponTabShowText;
    public Text upgradeTabShowText;
    public Text interiorTabShowText;
    public Text paperTabShowText;
    public Text sellTabShowText;
    
    public Text sellBtnShowText;

    public Text weaponTabTitleShowText;
    public Text upgradeTabTitleShowText;
    public Text interiorTabTitleShowText;
    public Text paperTabTitleShowText;
    public Text sellTabTitleShowText;

    public string buyBtnShowText;
    public Text[] buyBtnArr;

    public string currentGoldsShowText;
    public Text[] currentGoldsTextArr;

    public string merchantCommentTitleShowText;
    public Text[] mcTitleTextArr;




    private void Awake()
    {
        CheckMonsterShowUp();
        tabCount = buySellParent.transform.childCount - 1;
        buyTabs = new GameObject[buySellParent.transform.childCount];
        for (int i = 0; i < buySellParent.transform.childCount; i++)
        {
            buyTabs[i] = buySellParent.transform.GetChild(i).gameObject;
        }

        int tabPanelCount = tabPanelParent.transform.childCount;
        tabPanels = new merchantTabBase[tabPanelCount];
        for (int i = 0; i < tabPanelCount; i++)
        {
            tabPanels[i] = tabPanelParent.transform.GetChild(i).GetComponent<merchantTabBase>();
        }
    }
    private void Start()
    {
        AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.SLIDE);
        gameObject.SetActive(true);

        SetGeneralTexts();

        selectedTab = -1;
        OnClickBuyTab(0);
       selectedTab = 0;
    }
    
    public void OnClickBuyTab(int tabNum)
    {
        if (selectedTab == tabNum) return;

        AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.SLIDE);
        selectedTab = tabNum;

        buyTabs[tabNum].transform.SetSiblingIndex(tabCount);
        int nextSiblingCount = tabCount - 1;
        for (int i = tabNum - 1; i >= 0; i--)
        {
            buyTabs[i].transform.SetSiblingIndex(nextSiblingCount--);
        }
        for (int i = tabNum + 1; i <= tabCount; i++)
        {
            buyTabs[i].transform.SetSiblingIndex(nextSiblingCount--);
        }

        for (int i = 0; i < tabPanelParent.transform.childCount; i++)
        {
            if (i == tabNum)
            {
                tabPanels[i].ShowTab();
            }
            else
            {
                tabPanels[i].HideTab();
            }
        }
    }
    public void ExitMerchant()
    {

        AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.SLIDE);
        //상인 ui 나가기
        this.gameObject.SetActive(false);
        //위협도 
        dayOffUI.ExitMerchant();
    }

    void SetGeneralTexts()
    {
        weaponTabShowText.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "wt");
        upgradeTabShowText.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "ut");
        interiorTabShowText.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "it");
        paperTabShowText.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "pt");
        sellTabShowText.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "st");

        merchantCommentTitleShowText = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "merchantcomment");
        foreach (Text t in mcTitleTextArr)
        {
            t.text = merchantCommentTitleShowText;
        }

        buyBtnShowText = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "buybtn");
        foreach (Text t in buyBtnArr)
        {
            t.text = buyBtnShowText;
        }

        sellBtnShowText.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "sellbtn");

        currentGoldsShowText = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "currentgold");
        foreach (Text t in currentGoldsTextArr)
        {
            t.text = currentGoldsShowText;
        }

        weaponTabTitleShowText.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "wttitle");
        upgradeTabTitleShowText.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "uttitle");
        interiorTabTitleShowText.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "ittitle");
        paperTabTitleShowText.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "pttitle");
        sellTabTitleShowText.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "sttitle");
    }

    void CheckMonsterShowUp()
    {
        mobShowUpScreen.SetActive(false);
        int daysAfter = InGameTimeManager.GetInstance().GetDaysAfterGameStart();
        string noticeBase = UIGeneralTextsManager.GetUIGeneralText("mobshowup", "notice");
        mobShowUpExitBtn.onClick.AddListener(() => { mobShowUpScreen.SetActive(false); });

        for (int i = (int)E_Monster.GARGOYLE; i < (int)E_Monster.MAX; i++)    //가고일 부터 체크 시작.
        {
            E_Monster mob = (E_Monster)i;
            int showUpDay = MobEviInfoManager.GetInstance().GetMonsterShowUpDay(mob);
            string mobname = MobEviInfoManager.GetInstance().GetMobName(mob);
            string notice = noticeBase.Replace("[0]", mobname);

            if (daysAfter == showUpDay)
            {

                AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.GROWL);
                PhaseManager.GetInstance().MonsterShowUp(mob);  //몬스터 출현하도록 처리

                showUpMobStampedImage.sprite = SpriteManager.GetInstance().GetMobStampedSprite(mob);
                mobShowUpText.text = notice;
                mobShowUpScreen.SetActive(true);
                PhaseManager.GetInstance().LogDebug();
                return;
            }
        }
    }
}
