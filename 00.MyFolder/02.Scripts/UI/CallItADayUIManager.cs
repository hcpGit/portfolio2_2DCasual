using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEditor;
public class CallItADayUIManager : MonoBehaviour {
    public DayOffManager dayOffManager;
    public Text todayStartGoldTxt;
    public Text todayClientIncomeGoldTxt;
    public Text todayHunterPayGoldTxt;
    public Text todayDefenseGoldTxt;
    public Text todayRemainGoldTxt;
    public Text todayMerchantBuyCostTxt;
    public Text todayMerchantSellCostIncomeTxt;

    Color negativeColor = new Color(255/255f, 42 / 255f, 42 / 255f, 255 / 255f);
    Color positiveColor = new Color(0, 13 / 255f, 136 / 255f, 1);


    [Header("UI General Texts")]
    [SerializeField]
    string uiGeneralUIName = "call";
    public Text todayHoldGoldShowTxt;
    public Text todayDefShowTxt;
    public Text todayQIShowTxt;
    public Text todayQPayShowTxt;
    public Text todayMBuyShowTxt;
    public Text todayMSellShowTxt;
    public Text todayRemainShowTxt;

    public Text noteThreatShowTxt;
    public Text noteGoldShowTxt;
    public Text noteEndInfoShowTxt_Threat;
    public Text noteEndInfoShowTxt_Gold;


    private void Awake()
    {
        todayHoldGoldShowTxt.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "todayHolded");
        todayDefShowTxt.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "todayDef");
        todayQIShowTxt.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "todayQIncome");
        todayQPayShowTxt.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "todayQPay");
        todayMBuyShowTxt.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "todayMBuy");
        todayMSellShowTxt.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "todayMSell");
        todayRemainShowTxt.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "todayRemainGold");
        noteThreatShowTxt.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "noteThreatWarn");
        noteGoldShowTxt.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "noteGoldRunoutWarn");
        noteEndInfoShowTxt_Threat.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "noteEndInfo");
        noteEndInfoShowTxt_Gold.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "noteEndInfo");
    }

    // Use this for initialization
    void Start () {
        gameObject.SetActive(false);
	}
    public void OnClick_Exit()
    {

        AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.SLIDE);
        gameObject.SetActive(false);
        dayOffManager.EndOfCallItADay();
    }

    public void Init(bool didWatchAd)
    {
        todayStartGoldTxt.text = GoldManager.GetInstance().TodayStartWallet.ToString();

        if (didWatchAd)
            todayDefenseGoldTxt.text = "0";
        else
        todayDefenseGoldTxt.text = "- " + Constant.defenseExpense.ToString();
        

        TextShowByColor(todayClientIncomeGoldTxt, GoldManager.GetInstance().ClientIncomeToday);
        
        TextShowByColor(todayHunterPayGoldTxt, GoldManager.GetInstance().HunterPaymentToday);
        
        TextShowByColor(todayRemainGoldTxt, GoldManager.GetInstance().Gold);
        
        TextShowByColor(todayMerchantBuyCostTxt, GoldManager.GetInstance().MerchantBuyCost);
        
        TextShowByColor(todayMerchantSellCostIncomeTxt, GoldManager.GetInstance().MerchantSellCostIncome);

        
        string net = noteEndInfoShowTxt_Threat.text;
        net=net.Replace("[0]", Constant.ThreatHoldDays.ToString());
        noteEndInfoShowTxt_Threat.text = net;

        string neg = noteEndInfoShowTxt_Gold.text;
        neg=neg.Replace("[0]", Constant.GoldRunOutHoldDays.ToString());
        noteEndInfoShowTxt_Gold.text = neg;

        string netnow = noteThreatShowTxt.text;
        netnow=netnow.Replace("[0]", GameEndJudgeManager.GetInstance().ThreatMaxContinuityDays.ToString());
        noteThreatShowTxt.text = netnow;

        string negnow = noteGoldShowTxt.text;
        negnow=negnow.Replace("[0]", GameEndJudgeManager.GetInstance().GoldRunOutContinuityDays.ToString());
        noteGoldShowTxt.text = negnow;
        
    }

    void TextShowByColor(Text text, int gold)
    {
        if (gold < 0)
        {
            text.text = gold.ToString();
            text.color = negativeColor;
        }
        else {
            text.text = gold.ToString();
            text.color = positiveColor;
        }
    }
}
