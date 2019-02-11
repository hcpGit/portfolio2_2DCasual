using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class merchantTabSell : merchantTabBase
{
    public GameObject sellListContent;
    public GameObject sellBtnPrefab;
    public Text nowGoldTxt;
    public Text goldAdjustTxt;
    public Text merchantCommentTxt;

    public Button sellBtn;
    public Button plusBtn;
    public Button minusBtn;
    public Text sellNumText;

    Color selectedColor = new Color(140f / 255f, 190f / 255f, 140f / 255f, 255f / 255f);

    public Merchant_Sell merchantSellSheet;
    Dictionary<uint, Merchant_SellData> msCmtDic = new Dictionary<uint, Merchant_SellData>();

    class msBtn
    {
        public GameObject btn;
        public E_Monster mob;
        public E_Evidence evidence;
        public int number;
        public int price;
        public string merchantCmt;

        public msBtn(E_Monster mob, E_Evidence evidence, GameObject b, int n, int p, string c)
        {
            this.mob = mob;
            this.evidence = evidence;
            btn = b;
            number = n;
            price = p;
            merchantCmt = c; }
    }

    Dictionary<int, msBtn> msBtnDic = new Dictionary<int, msBtn>();
    int? selected = null;
    int nowGold;
    E_Language nowLang;
    int sellNumber;

    private void Awake()
    {
        msCmtDic.Clear();
        for (int i = 0; i < merchantSellSheet.dataArray.Length; i++)
        {
            if (merchantSellSheet.dataArray[i].Msnum == 9000) break;
            uint key = MakeDicKey(merchantSellSheet.dataArray[i].Sellmob, merchantSellSheet.dataArray[i].Sellevidence);
                if (msCmtDic.ContainsKey(key))
            {
                Debug.LogError("키 생성 오류." + i);
            }
            msCmtDic.Add(key, merchantSellSheet.dataArray[i]);
        }
        

    }
    uint MakeDicKey(int mob, int evi)
    {
        uint key = 0;
        uint mobkey = 0;
        uint evikey = 0;

        mobkey = (uint)mob;
        mobkey <<= 16;

        evikey = (uint)evi;

        key = mobkey | evikey;
        return key;
    }


    void Init()
    {
        selected = null;
        msBtnDic.Clear();
        sellNumber = 1;
        ShowNumText();

        sellBtn.interactable = false;
        nowGold = GoldManager.GetInstance().Gold;
        nowGoldTxt.text = nowGold.ToString();
        
        nowLang = LanguageManager.GetInstance().Language;
        // nowLang = E_Language.ENGLISH;

        List<QuestPerMob> nowEviInvenList = Inventory.GetInstance().GetNowMobEvidencesInven();

        for (int i = 0; i < nowEviInvenList.Count; i++)
        {
            string comment="";
            uint dickey = MakeDicKey((int)nowEviInvenList[i].mob,(int) nowEviInvenList[i].evidence);
            int price = msCmtDic[dickey].Sellprice;

            switch (nowLang)
            {
                case E_Language.KOREAN:
                    comment = msCmtDic[dickey].Cmtkor;
                    break;

                case E_Language.ENGLISH:
                    comment = msCmtDic[dickey].Cmteng;
                    break;
            }

            int num = i;

            GameObject btn = Instantiate(sellBtnPrefab, sellListContent.transform);

            btn.GetComponent<Button>().onClick.AddListener(
                () =>
                {
                    AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.CLICK);
                    SelectSell(num);
                }
                );
            msBtnDic.Add(num, new msBtn(nowEviInvenList[i].mob , nowEviInvenList[i].evidence, btn , nowEviInvenList[i].number,price , comment));

            SetTxtsSellBtn(btn, nowEviInvenList[i].mob, nowEviInvenList[i].evidence,  nowEviInvenList[i].number, price);
        }
        ShowDefaultComment();
        GoldAdjustShow(0);
    }

    void SelectSell(int originNumber)
    {
        if (selected == null)
        {
            selected = originNumber;
            msBtnDic[originNumber].btn.GetComponent<Image>().color = selectedColor;
            GoldAdjustShow(msBtnDic[originNumber].price * sellNumber);
            ShowSellComment(originNumber);

            sellBtn.interactable = true;

            return;
        }
        if (selected == originNumber)   //취소할   때.
        {
            selected = null;
            msBtnDic[originNumber].btn.GetComponent<Image>().color = Color.white;
            sellBtn.interactable = false;
            GoldAdjustShow(0);
            ShowDefaultComment();

            sellNumber = 1;
            ShowNumText();
            return;
        }
        //새로운 놈을 고를때.
        msBtnDic[(int)selected].btn.GetComponent<Image>().color = Color.white;
        selected = originNumber;
        msBtnDic[originNumber].btn.GetComponent<Image>().color = selectedColor;
        
        sellBtn.interactable = true;
        
        ShowSellComment(originNumber);
        sellNumber = 1;
        ShowNumText();
        GoldAdjustShow(msBtnDic[originNumber].price * sellNumber);

        return;
    }

    void GoldAdjustShow(int price)
    {
        if (price == 0) //초기화
        {
            nowGoldTxt.text = (nowGold).ToString();
            goldAdjustTxt.text = "";
            return;
        }
        nowGoldTxt.text = (nowGold + price).ToString();
        goldAdjustTxt.text = "+" + price.ToString();
    }

    void ShowDefaultComment()   //9000 은 시트 상의 넘버라고 보면 됨.
    {
        int dch = merchantSellSheet.dataArray.Length - 1;
        int dcl = dch;

        while (merchantSellSheet.dataArray[dcl].Msnum != 9000)
        {
            dcl--;
        }
        int randCmt = Random.Range(dcl, dch + 1);
        string commment = "";

        switch (nowLang)
        {
            case E_Language.KOREAN:
                commment = merchantSellSheet.dataArray[randCmt].Cmtkor;
                break;

            case E_Language.ENGLISH:
                commment = merchantSellSheet.dataArray[randCmt].Cmteng;
                break;
        }

        merchantCommentTxt.text = commment;
    }

    void ShowSellComment(int num)
    {
        merchantCommentTxt.text = msBtnDic[num].merchantCmt;
    }

    void SetTxtsSellBtn(GameObject btn, E_Monster mob , E_Evidence evi ,int number, int price)
    {
        btn.transform.GetChild(0).GetComponent<Image>().sprite = SpriteManager.GetInstance().GetColoredEvidenceSprite (mob,evi);
        btn.transform.GetChild(1).GetComponent<Text>().text = MobEviInfoManager.GetInstance().GetMobName(mob);
        btn.transform.GetChild(2).GetComponent<Text>().text = MobEviInfoManager.GetInstance().GetEvidenceName(evi);
        btn.transform.GetChild(3).GetComponent<Text>().text = number.ToString();
        btn.transform.GetChild(4).GetComponent<Text>().text = price.ToString();
    }

    public void OnClickNumber(bool plusOrMinus)
    {
        if (selected == null) return;

        AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.CLICK);
        if (plusOrMinus)
        {
            //선택된 애의 최대치 맞춰서
            if (msBtnDic[(int)selected].number <= sellNumber)
            {
                sellNumber = msBtnDic[(int)selected].number;
                ShowNumText();
                return;
            }
            sellNumber++;
            ShowNumText();
            GoldAdjustShow(msBtnDic[(int)selected].price * sellNumber);

            return;
        }

        //마이너스 경우
        if (sellNumber <= 1) return;

        sellNumber--;
        ShowNumText();
        GoldAdjustShow(msBtnDic[(int)selected].price * sellNumber);
    }

    public void ShowNumText()
    {
        sellNumText.text = sellNumber.ToString();
    }

    public void OnClickSell()
    {
        if (selected == null) Debug.LogError("판매 오류!");

        List<QuestPerMob> temp = new List<QuestPerMob>();

        QuestPerMob qpm = new QuestPerMob(
             msBtnDic[(int)selected].mob,
             msBtnDic[(int)selected].evidence,
             sellNumber
            );
        temp.Add(qpm);

        Inventory.GetInstance().MinusMobEviItem(temp);

        GoldManager.GetInstance().AdjustGold((msBtnDic[(int)selected].price * sellNumber),GoldManager.E_PayType.SELL_TO_MERCHANT);

        HideTab();
        ShowTab();
        //인벤토리에서 빼주고고.
        //골드 까주고.
    }

    public override void ShowTab()
    {
        this.gameObject.SetActive(true);
        Init();
    }
    public override void HideTab()
    {
        this.gameObject.SetActive(false);

        for (int i = 0; i < sellListContent.transform.childCount; i++)
        {
            Destroy(sellListContent.transform.GetChild(i).gameObject);
        }
    }
}
