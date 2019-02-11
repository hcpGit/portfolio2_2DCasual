using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayOffManager : MonoBehaviour {
    WaitForSeconds ws = new WaitForSeconds(0.3f);
    public MerchantUIManager merchantUI;
    public CallItADayUIManager callItADayUI;

    public GameObject gameEndCanvas;
    public GameObject adAskCanvas;

    [Header("UI General Texts")]
    [SerializeField]
    string uiGeneralUIName = "dom";
    public Text adAskText;
    public Text adYesText;


    void Awake()
    {
        ThreatUIManager.GetInstance().Hide();
        gameEndCanvas.SetActive(false);
        adAskCanvas.SetActive(false);

        string adasktxt = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "adask");

        adAskText.text = adasktxt.Replace("[0]", Constant.defenseExpense.ToString());
        adYesText.text = UIGeneralTextsManager.GetUIGeneralText(uiGeneralUIName, "adyes");

    }

    public void ExitMerchant()
    {
        ThreatUIManager.GetInstance().Show();
        
        List<WholeMonsterRiskManager.MonsterRisk> tonightHunted =
            WholeMonsterRiskManager.GetInstance().DecreaseThreat(WholeMonsterRiskManager.GetInstance().LetsHuntingAtEndOfDay());

        List<WholeMonsterRiskManager.MonsterRisk> increasedThreat = WholeMonsterRiskManager.GetInstance().IncreaseThreat();

       


        ThreatUIManager.GetInstance().exitBtn.onClick.AddListener(
            () => {
                AdAsk();    //위협도 정산 후 광고 묻기로 이동. 알아서 머챈트와 위협도는 꺼져있음.
            }
            );
        ThreatUIManager.GetInstance().exitBtn.interactable = false;

        StartCoroutine(ThreatUIAnim(tonightHunted , increasedThreat));

        
        GameEndJudgeManager.GetInstance().didThreatHitsMax(WholeMonsterRiskManager.GetInstance().IsThreatHitsMax());



        //이 로직은 따로 빼야함.

       
    }
    

    public void EndOfCallItADay()
    {
        //게임매니저에서 퀘스트룸 씬 로딩하게.
        GoldManager.GetInstance().DayOff();
        GameManager.GetInstance(). ContinueDay();
        //게임 매니저 퀘스트룸 호툴 부분
    }

    IEnumerator ThreatUIAnim(List<WholeMonsterRiskManager.MonsterRisk> tonightHunted, List<WholeMonsterRiskManager.MonsterRisk> increasedThreat)
    {
        while (ThreatUIManager.GetInstance().State != ThreatUIManager.E_ThreatUIState.IDLE)
            yield return ws;

        ThreatUIManager.GetInstance().IADThreat(false, tonightHunted);

        while (ThreatUIManager.GetInstance().State != ThreatUIManager.E_ThreatUIState.IDLE)
            yield return ws;

        ThreatUIManager.GetInstance().IADThreat(true, increasedThreat);

        while (ThreatUIManager.GetInstance().State != ThreatUIManager.E_ThreatUIState.IDLE)
            yield return ws;

        ThreatUIManager.GetInstance().exitBtn.interactable = true;
    }

    public void AdAsk() //위협도 정산 후 불려옴.
    {
        ThreatUIManager.GetInstance().exitBtn.onClick.RemoveListener(
           () => { AdAsk(); }
           );
        adAskCanvas.SetActive(true);
    }
    public void AdAskYes()
    {
       AdMobManager.GetInstance().ShowDeffenseSaveRewardAd();
    }

    public void AdAskCancel()
    {
        adAskCanvas.SetActive(false);
        GoldManager.GetInstance().AdjustGold(-1 * Constant.defenseExpense, GoldManager.E_PayType.NOTHING); //방위비용 지불.
        GameEndJudgeManager.GetInstance().didGoldRunOut(GoldManager.GetInstance().IsGoldRunOut());

        if (GameEndJudgeManager.GetInstance().IsGameEnd())
        {
                gameEndCanvas.SetActive(true);
            return;
        }
        callItADayUI.gameObject.SetActive(true);
        callItADayUI.Init(false);
    }
  
    public void AdShowDone()
    {
        Debug.Log("광고 끝 확인");
        bool rewarded = AdMobManager.GetInstance().Rewarded;

        adAskCanvas.SetActive(false);

        GameEndJudgeManager.GetInstance().didGoldRunOut(GoldManager.GetInstance().IsGoldRunOut());

        if (GameEndJudgeManager.GetInstance().IsGameEnd())
        {
            gameEndCanvas.SetActive(true);
            return;
        }

        callItADayUI.gameObject.SetActive(true);

       callItADayUI.Init(rewarded);
    }
    public void GameOverSoGoToStart()
    {
        Debug.Log("게임오버. 스타트씬으로");
        GameManager.GetInstance().GoToStartScene();
    }
}
