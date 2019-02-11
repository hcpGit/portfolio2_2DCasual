using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class merchantTabBuyPaper : merchantTabBase
{
    public GameObject buyPaperContent;
    public GameObject buyPaperBtn;
    public Text nowGoldTxt;
    public Text goldAdjustTxt;
    public Text merchantCommentTxt;
    public Button buyBtn;
    Color selectedColor = new Color(140f / 255f, 190f / 255f, 140f / 255f, 255f / 255f);

    public Merchant_Paper merchantPaperSheet;

    class mpBtn
    {
        public GameObject btn;
        public E_Paper upType;
        public string name;
        public int price;
        public string merchantCmt;
        public mpBtn(E_Paper up, GameObject b, string n, int p, string c) { upType = up; btn = b; name = n; price = p; merchantCmt = c; }
    }

    Dictionary<int, mpBtn> mpDic = new Dictionary<int, mpBtn>();
    int? selected = null;
    int nowGold;
    E_Language nowLang;


    void Init()
    {
        selected = null;
        mpDic.Clear();

        buyBtn.interactable = false;
        nowGold = GoldManager.GetInstance().Gold;
        nowGoldTxt.text = nowGold.ToString();

        int miCount = (int)E_Paper.MAX;

        nowLang = LanguageManager.GetInstance().Language;

        List<E_Monster> opendMob = PhaseManager.GetInstance().GetOpendMobDataList();
        List<E_Evidence> opendEvi = PhaseManager.GetInstance().GetOpendEviDataList();

        for (int i = 1; i < miCount; i++)
        {
            int dataArrIdx = i - 1;
            string name = "";
            string comment = "";

            int price = merchantPaperSheet.dataArray[dataArrIdx].Price;
            switch (nowLang)
            {
                case E_Language.KOREAN:
                    name = merchantPaperSheet.dataArray[dataArrIdx].Namekor;
                    comment = merchantPaperSheet.dataArray[dataArrIdx].Mpcmtkor;
                    break;

                case E_Language.ENGLISH:
                    name = merchantPaperSheet.dataArray[dataArrIdx].Nameeng;
                    comment = merchantPaperSheet.dataArray[dataArrIdx].Mpcmteng;
                    break;
            }

            int num = i;

            GameObject btn = Instantiate(buyPaperBtn, buyPaperContent.transform);

            btn.GetComponent<Button>().onClick.AddListener(
                () =>
                {
                    AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.CLICK);
                    SelectPaper(num);
                }

                );
            mpDic.Add(num, new mpBtn((E_Paper)i, btn, name, price, comment));

            SetTxtsPaperBtn(btn, price, name, (E_Paper)i);


            if (i <= ((int)E_Monster.MAX) - 1)  //몬스터 허가증인 경우
            {
                if (opendMob.Contains((E_Monster)i))    //이미 허가증을 얻은 몬스터면
                {
                    btn.GetComponent<Button>().interactable = false;
                }
            }
            else {
                int eviNum = i -  (((int)E_Monster.MAX) - 1);

                //증거품 허가증인경우
                if (opendEvi.Contains((E_Evidence)eviNum))    //이미 허가증을 얻은 몬스터면
                {
                    btn.GetComponent<Button>().interactable = false;
                }
            }

        }

        ShowDefaultComment();
        GoldAdjustShow(0);
    }

    void SelectPaper(int originNumber)
    {
        if (selected == null)
        {
            selected = originNumber;
            mpDic[originNumber].btn.GetComponent<Image>().color = selectedColor;
            GoldAdjustShow(mpDic[originNumber].price);
            ShowPaperComment(originNumber);

            if (GoldManager.GetInstance().CanPaymentThis(mpDic[originNumber].price))
                buyBtn.interactable = true;
            else buyBtn.interactable = false;

            return;
        }
        if (selected == originNumber)   //취소할   때.
        {
            selected = null;
            mpDic[originNumber].btn.GetComponent<Image>().color = Color.white;
            buyBtn.interactable = false;
            GoldAdjustShow(0);
            ShowDefaultComment();
            return;
        }
        //새로운 놈을 고를때.
        mpDic[(int)selected].btn.GetComponent<Image>().color = Color.white;
        selected = originNumber;
        mpDic[originNumber].btn.GetComponent<Image>().color = selectedColor;

        if (GoldManager.GetInstance().CanPaymentThis(mpDic[originNumber].price))
            buyBtn.interactable = true;
        else buyBtn.interactable = false;

        GoldAdjustShow(mpDic[originNumber].price);
        ShowPaperComment(originNumber);

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
        int dch = merchantPaperSheet.dataArray.Length - 1;
        int dcl = dch;

        while (merchantPaperSheet.dataArray[dcl].Papernum != 9000)
        {
            dcl--;
        }
        int randCmt = Random.Range(dcl, dch + 1);
        string commment = "";

        switch (nowLang)
        {
            case E_Language.KOREAN:
                commment = merchantPaperSheet.dataArray[randCmt].Mpcmtkor;
                break;

            case E_Language.ENGLISH:
                commment = merchantPaperSheet.dataArray[randCmt].Mpcmteng;
                break;
        }

        merchantCommentTxt.text = commment;
    }

    void ShowPaperComment(int num)
    {
        merchantCommentTxt.text = mpDic[num].merchantCmt;
    }

    void SetTxtsPaperBtn(GameObject btn, int price, string name, E_Paper up)
    {
        btn.transform.GetChild(0).GetComponent<Image>().sprite = SpriteManager.GetInstance().GetPaperSprite(up);
        btn.transform.GetChild(1).GetComponent<Text>().text = name;
        btn.transform.GetChild(2).GetComponent<Text>().text = price.ToString();
    }

    public void OnClickBuy()
    {
        if (selected == null) Debug.LogError("페이퍼 구입 선택 오류0");

        if (selected <= ((int)E_Monster.MAX) - 1)   //몬스터 허가증인 경우
        {
            E_Monster mob = (E_Monster)selected;
            Debug.Log(mob);

            if (PhaseManager.GetInstance().IsOpen(mob))
            {
                Debug.Log("이미 열린것?");
                HideTab();
                ShowTab();
                return;
            }
            else
            {
                Debug.Log("몬스터 구입");
                PhaseManager.GetInstance().OpenMonsterCertificate(mob);
                GoldManager.GetInstance().AdjustGold(-1 * mpDic[(int)selected].price, GoldManager.E_PayType.BUY_BY_MERCHANT);
                HideTab();
                ShowTab();
                return;
            }
        }
        else {
            int eviNum = ((int)selected) - (((int)E_Monster.MAX) - 1);

            E_Evidence evi = (E_Evidence)eviNum;
            Debug.Log(evi);
            if (PhaseManager.GetInstance().IsOpen(evi))
            {
                Debug.Log("이미 열린것?");
                HideTab();
                ShowTab();
                return;
            }
            else
            {
                Debug.Log("증거품 구입");
                PhaseManager.GetInstance().OpenEvidenceCertificate(evi);
                GoldManager.GetInstance().AdjustGold(-1 * mpDic[(int)selected].price, GoldManager.E_PayType.BUY_BY_MERCHANT);
                HideTab();
                ShowTab();
                return;
            }

        }
        
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

        for (int i = 0; i < buyPaperContent.transform.childCount; i++)
        {
            Destroy(buyPaperContent.transform.GetChild(i).gameObject);
        }
    }
}
