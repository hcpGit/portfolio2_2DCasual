using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class merchantTabBuyUpgrade : merchantTabBase
{
    public GameObject buyUpgradeContent;
    public GameObject buyUpgradeBtn;
    public Text nowGoldTxt;
    public Text goldAdjustTxt;
    public Text merchantCommentTxt;
    public Button buyBtn;
    Color selectedColor = new Color(140f / 255f, 190f / 255f, 140f / 255f, 255f / 255f);

    public Merchant_Upgrade merchantUpgradeSheet;

    class muBtn
    {
        public GameObject btn;
        public E_Upgrade upType;
        public string name;
        public int price;
        public string merchantCmt;
        public muBtn(E_Upgrade up, GameObject b, string n, int p, string c) { upType = up; btn = b; name = n; price = p; merchantCmt = c; }
    }

    Dictionary<int, muBtn> muDic = new Dictionary<int, muBtn>();
    int? selected = null;
    int nowGold;
    E_Language nowLang;


    void Init()
    {
        selected = null;
        muDic.Clear();

        buyBtn.interactable = false;
        nowGold = GoldManager.GetInstance().Gold;
        nowGoldTxt.text = nowGold.ToString();

        int muCount = (int)E_Upgrade.MAX;

        nowLang = LanguageManager.GetInstance().Language;

        for (int i = 0; i < muCount; i++)
        {
            string name = "";
            string comment = "";
            int price = merchantUpgradeSheet.dataArray[i].Price;
            switch (nowLang)
            {
                case E_Language.KOREAN:
                    name = merchantUpgradeSheet.dataArray[i].Namekor;
                    comment = merchantUpgradeSheet.dataArray[i].Mucmtkor;
                    break;

                case E_Language.ENGLISH:
                    name = merchantUpgradeSheet.dataArray[i].Nameeng;
                    comment = merchantUpgradeSheet.dataArray[i].Mucmteng;
                    break;
            }

            int num = i;

            GameObject btn = Instantiate(buyUpgradeBtn, buyUpgradeContent.transform);

            btn.GetComponent<Button>().onClick.AddListener(
                () =>
                {
                    SelectUpgrade(num);
                    AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.CLICK);
                }

                );
            muDic.Add(num, new muBtn((E_Upgrade)i,  btn, name, price, comment));

            SetTxtsUpgradeBtn(btn, price, name, (E_Upgrade)i);

            if (PhaseManager.GetInstance().IsOpen((E_Upgrade)i)) {
                btn.GetComponent<Button>().interactable = false;
            }
            
        }
        ShowDefaultComment();
        GoldAdjustShow(0);
    }

    void SelectUpgrade(int originNumber)
    {
        if (selected == null)
        {
            selected = originNumber;
            muDic[originNumber].btn.GetComponent<Image>().color = selectedColor;
            GoldAdjustShow(muDic[originNumber].price);
            ShowUpgradeComment(originNumber);

            if (GoldManager.GetInstance().CanPaymentThis(muDic[originNumber].price))
                buyBtn.interactable = true;
            else buyBtn.interactable = false;

            return;
        }
        if (selected == originNumber)   //취소할   때.
        {
            selected = null;
            muDic[originNumber].btn.GetComponent<Image>().color = Color.white;
            buyBtn.interactable = false;
            GoldAdjustShow(0);
            ShowDefaultComment();
            return;
        }
        //새로운 놈을 고를때.
        muDic[(int)selected].btn.GetComponent<Image>().color = Color.white;
        selected = originNumber;
        muDic[originNumber].btn.GetComponent<Image>().color = selectedColor;

        if (GoldManager.GetInstance().CanPaymentThis(muDic[originNumber].price))
            buyBtn.interactable = true;
        else buyBtn.interactable = false;

        GoldAdjustShow(muDic[originNumber].price);
        ShowUpgradeComment(originNumber);

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
        nowGoldTxt.text = (nowGold - price).ToString();
        goldAdjustTxt.text = "-" + price.ToString();
    }

    void ShowDefaultComment()   //9000 은 시트 상의 넘버라고 보면 됨.
    {
        int dch = merchantUpgradeSheet.dataArray.Length - 1;
        int dcl = dch;

        while (merchantUpgradeSheet.dataArray[dcl].Upgradenum != 9000)
        {
            dcl--;
        }
        int randCmt = Random.Range(dcl, dch + 1);
        string commment = "";

        switch (nowLang)
        {
            case E_Language.KOREAN:
                commment = merchantUpgradeSheet.dataArray[randCmt].Mucmtkor;
                break;

            case E_Language.ENGLISH:
                commment = merchantUpgradeSheet.dataArray[randCmt].Mucmteng;
                break;
        }

        merchantCommentTxt.text = commment;
    }

    void ShowUpgradeComment(int num)
    {
        merchantCommentTxt.text = muDic[num].merchantCmt;
    }

    void SetTxtsUpgradeBtn(GameObject btn, int price , string name , E_Upgrade up)
    {
        btn.transform.GetChild(0).GetComponent<Image>().sprite = SpriteManager.GetInstance().GetUpgradeSprite(up);
        btn.transform.GetChild(1).GetComponent<Text>().text = name;
        btn.transform.GetChild(2).GetComponent<Text>().text = price.ToString();
    }

    public void OnClickBuy()
    {
        if (selected == null) Debug.LogError("업글 구입 선택 오류0");

        E_Upgrade up = (E_Upgrade)((int)selected);

        if (false == PhaseManager.GetInstance().IsOpen(up))
        {
            PhaseManager.GetInstance().OpenUpgrade(up);
            GoldManager.GetInstance().AdjustGold(-1* muDic[(int)selected].price, GoldManager.E_PayType.BUY_BY_MERCHANT);
        }
        HideTab();
        ShowTab();

        //인벤토리에 넣어주고.
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

        for (int i = 0; i < buyUpgradeContent.transform.childCount; i++)
        {
            Destroy(buyUpgradeContent.transform.GetChild(i).gameObject);
        }
    }
}
