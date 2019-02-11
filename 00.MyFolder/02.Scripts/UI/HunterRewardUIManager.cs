using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class HunterRewardUIManager : MonoBehaviour {
    static HunterRewardUIManager instance;
    public GameObject writtenQuestContent;
    public GameObject hunterBringContent;
    public GameObject itemBtn;

    public Button partialBtn;

    public Text hunterName;
    public Text completenessText;
    float completeness;

    public Text allPaymentText;
    public Text partialPaymentText;
    public Text denyPaymentText;

    int allPayGolds = 0;
    int partialPayGolds = 0;

    [Header("UI General Texts")]
    [SerializeField]
    string uiGeneralUIName = "hr";
    public Text orderedListTxt;
    public Text hunterBringListTxt;
    public Text completenessTxt;
    public Text allPayTxt;
    public Text partPayTxt;
    public Text denyPayTxt;
    public string paymentTxt;

    void Awake()
    {
        instance = this;
        orderedListTxt.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "orderedlist");
        hunterBringListTxt.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "hunterbring");
        completenessTxt.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "completeness");
        allPayTxt.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "allpay");
        partPayTxt.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "partpay");
        denyPayTxt.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "denypay");
        paymentTxt = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "payment");
    }
    public static HunterRewardUIManager GetInstance()
    {
        return instance;
    }

    public void ShowHRUI()
    {
        for (int i = 0; i < writtenQuestContent.transform.childCount; i++)
        {
            Destroy(writtenQuestContent.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < hunterBringContent.transform.childCount; i++)
        {
            Destroy(hunterBringContent.transform.GetChild(i).gameObject);
        }
        this.gameObject.SetActive(true);

        Init();
    }
    public void HideHRUI()
    {
        for (int i = 0; i < writtenQuestContent.transform.childCount; i++)
        {
            Destroy(writtenQuestContent.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < hunterBringContent.transform.childCount; i++)
        {
            Destroy(hunterBringContent.transform.GetChild(i).gameObject);
        }
        partialBtn.interactable = true;
        gameObject.SetActive(false);
    }

    void Init()
    {
        HunterIdea hunter = CharactorFrame.GetInstance().hunterIdea;
        Debug.Log("헌터 리워드, 원래 헌터."+hunter.HuntedList.DebugString());

        List<QuestPerMob> huntingOutcome;
        var hol = from ho in hunter.HuntedList
                  orderby (int)ho.mob
                  select ho;
        huntingOutcome = hol.ToList<QuestPerMob>();

        for (int i = 0; i < huntingOutcome.Count; i++)
        {
            QuestPerMob qpm = huntingOutcome[i];
            GameObject temp = Instantiate(itemBtn,hunterBringContent.transform);
            SetTxtsItem(qpm, temp);
        }
        List<QuestPerMob> writtenQuestList = QuestManager.GetInstance().GetQuest(
            hunter.associatedQuestKey
            ).QuestList;

        var wql = from wq in writtenQuestList
                  orderby (int)wq.mob
                  select wq;
        writtenQuestList = wql.ToList<QuestPerMob>();

        for (int i = 0; i < writtenQuestList.Count; i++)
        {
            QuestPerMob qpm = writtenQuestList[i];
            GameObject temp = Instantiate(itemBtn, writtenQuestContent.transform);
            SetTxtsItem(qpm, temp);
        }
        completeness = QuestManager.GetInstance().GetQuest(hunter.associatedQuestKey).GetCompleteness
            (hunter.HuntedList);

        hunterName.text = hunter.CharaName;
        completenessText.text = completeness.ToString();

        if (completeness <= 50) completenessText.color = Color.red;
        else if (completeness < 80) completenessText.color = Color.yellow;
        else completenessText.color = Color.green;


        allPayGolds = PayFormula.CalculateHuntLastPayment(QuestManager.GetInstance().GetQuest(
            hunter.associatedQuestKey
            ), 
            hunter.GetHunterExpireGap(),
            hunter.HuntedList, E_RewardType.ALL_PAYMENT);

        partialPayGolds = PayFormula.CalculateHuntLastPayment(QuestManager.GetInstance().GetQuest(
                hunter.associatedQuestKey),
                hunter.GetHunterExpireGap()
                , hunter.HuntedList, E_RewardType.PARTIAL_PAYMENT);
       
        //지불 금액 텍스트 표시.
        allPaymentText.text = allPayGolds.ToString() +" "+ paymentTxt;

        if (completeness < 100 && completeness>0)
        {
            partialPaymentText.text = partialPayGolds.ToString() + " " + paymentTxt;
        }
        else {
            partialBtn.interactable = false;
            partialPaymentText.text = "";
        }

        denyPaymentText.text = "0 "+paymentTxt;
    }

    void SetTxtsItem(QuestPerMob qpm, GameObject btn)   //텍스트세팅.
    {
        btn.transform.GetChild(0).GetComponent<Image>().sprite = SpriteManager.GetInstance().GetColoredEvidenceSprite(qpm.mob,qpm.evidence);
        btn.transform.GetChild(1).GetComponent<Text>().text = MobEviInfoManager.GetInstance().GetMobName(qpm.mob);
        btn.transform.GetChild(2).GetComponent<Text>().text = MobEviInfoManager.GetInstance().GetEvidenceName(qpm.evidence);
        btn.transform.GetChild(3).GetComponent<Text>().text = qpm.number.ToString();
    }

    public void ChoiceBtnOnClick(int paymentWay)
    {
        InteractiveManager.GetInstance().HunterRewardSubmit(paymentWay);
        HideHRUI();
    }
}
