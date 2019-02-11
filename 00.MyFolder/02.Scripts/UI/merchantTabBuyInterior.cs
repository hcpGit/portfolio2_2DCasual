using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class merchantTabBuyInterior : merchantTabBase
{
    public GameObject buyInteriorContent;
    public GameObject buyInteriorBtn;
    public Text nowGoldTxt;
    public Text goldAdjustTxt;
    public Text merchantCommentTxt;
    public Button buyBtn;
    Color selectedColor = new Color(140f / 255f, 190f / 255f, 140f / 255f, 255f / 255f);

    public Merchant_Interior merchantInteriorSheet;

    class miBtn
    {
        public GameObject btn;
        public E_Interior upType;
        public string name;
        public int price;
        public string merchantCmt;
        public miBtn(E_Interior up, GameObject b, string n, int p, string c) { upType = up; btn = b; name = n; price = p; merchantCmt = c; }
    }

    Dictionary<int, miBtn> miDic = new Dictionary<int, miBtn>();
    int? selected = null;
    int nowGold;
    E_Language nowLang;


    void Init()
    {
        selected = null;
        miDic.Clear();

        buyBtn.interactable = false;
        nowGold = GoldManager.GetInstance().Gold;
        nowGoldTxt.text = nowGold.ToString();

        int miCount = (int)E_Interior.MAX;

        nowLang = LanguageManager.GetInstance().Language;
        //nowLang = E_Language.ENGLISH;

        for (int i = 0; i < miCount; i++)
        {
            string name = "";
            string comment = "";
            int price = merchantInteriorSheet.dataArray[i].Price;
            switch (nowLang)
            {
                case E_Language.KOREAN:
                    name = merchantInteriorSheet.dataArray[i].Namekor;
                    comment = merchantInteriorSheet.dataArray[i].Micmtkor;
                    break;

                case E_Language.ENGLISH:
                    name = merchantInteriorSheet.dataArray[i].Nameeng;
                    comment = merchantInteriorSheet.dataArray[i].Micmteng;
                    break;
            }

            int num = i;

            GameObject btn = Instantiate(buyInteriorBtn, buyInteriorContent.transform);

            btn.GetComponent<Button>().onClick.AddListener(
                () =>
                {
                    AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.CLICK);
                    SelectInterior(num);
                }

                );
            miDic.Add(num, new miBtn((E_Interior)i, btn, name, price, comment));

            SetTxtsInteriorBtn(btn, price, name, (E_Interior)i);

            if (PhaseManager.GetInstance().IsOpen((E_Interior)i))
            {
                btn.GetComponent<Button>().interactable = false;
            }

        }
        ShowDefaultComment();
        GoldAdjustShow(0);
    }

    void SelectInterior(int originNumber)
    {
        if (selected == null)
        {
            selected = originNumber;
            miDic[originNumber].btn.GetComponent<Image>().color = selectedColor;
            GoldAdjustShow(miDic[originNumber].price);
            ShowInteriorComment(originNumber);

            if (GoldManager.GetInstance().CanPaymentThis(miDic[originNumber].price))
                buyBtn.interactable = true;
            else buyBtn.interactable = false;

            return;
        }
        if (selected == originNumber)   //취소할   때.
        {
            selected = null;
            miDic[originNumber].btn.GetComponent<Image>().color = Color.white;
            buyBtn.interactable = false;
            GoldAdjustShow(0);
            ShowDefaultComment();
            return;
        }
        //새로운 놈을 고를때.
        miDic[(int)selected].btn.GetComponent<Image>().color = Color.white;
        selected = originNumber;
        miDic[originNumber].btn.GetComponent<Image>().color = selectedColor;

        if (GoldManager.GetInstance().CanPaymentThis(miDic[originNumber].price))
            buyBtn.interactable = true;
        else buyBtn.interactable = false;

        GoldAdjustShow(miDic[originNumber].price);
        ShowInteriorComment(originNumber);

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
        int dch = merchantInteriorSheet.dataArray.Length - 1;
        int dcl = dch;
       // Debug.Log("초기 dcl=" + dcl + "dch=" + dch);

        while (merchantInteriorSheet.dataArray[dcl].Interiornum != 9000)
        {
           // Debug.Log("dcl=" + dcl + "dch=" + dch + "dcl의 웨폰넘버 = " + merchantInteriorSheet.dataArray[dcl].Interiornum + "dch의 웨폰넘버 = " + merchantInteriorSheet.dataArray[dch].Interiornum);
            dcl--;
        }
        int randCmt = Random.Range(dcl, dch + 1);
        string commment = "";

        switch (nowLang)
        {
            case E_Language.KOREAN:
                commment = merchantInteriorSheet.dataArray[randCmt].Micmtkor;
                break;

            case E_Language.ENGLISH:
                commment = merchantInteriorSheet.dataArray[randCmt].Micmteng;
                break;
        }

        merchantCommentTxt.text = commment;
    }

    void ShowInteriorComment(int num)
    {
        merchantCommentTxt.text = miDic[num].merchantCmt;
    }

    void SetTxtsInteriorBtn(GameObject btn, int price, string name, E_Interior up)
    {
        btn.transform.GetChild(0).GetComponent<Image>().sprite = SpriteManager.GetInstance().GetInteriorSprite(up);
        btn.transform.GetChild(1).GetComponent<Text>().text = name;
        btn.transform.GetChild(2).GetComponent<Text>().text = price.ToString();
    }

    public void OnClickBuy()
    {
        if (selected == null) Debug.LogError("인테리어 구입 선택 오류0");

        E_Interior inter = (E_Interior)((int)selected);

        if (false == PhaseManager.GetInstance().IsOpen(inter))
        {
            PhaseManager.GetInstance().OpenInterior(inter);
            GoldManager.GetInstance().AdjustGold(-1 * miDic[(int)selected].price, GoldManager.E_PayType.BUY_BY_MERCHANT);
            
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

        for (int i = 0; i < buyInteriorContent.transform.childCount; i++)
        {
            Destroy(buyInteriorContent.transform.GetChild(i).gameObject);
        }
    }
}
