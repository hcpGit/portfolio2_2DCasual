using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class merchantTabBuyWeapon : merchantTabBase {
    public GameObject buyWeaponContent;
    public GameObject buyWeaponBtn;
    public Text nowGoldTxt;
    public Text goldAdjustTxt;
    public Text merchantCommentTxt;
    public Button buyBtn;
    Color selectedColor = new Color(140f / 255f, 190f / 255f, 140f / 255f, 255f / 255f);

    public Merchant_Weapon merchantWeaponSheet;
    public Weapon_Info weaponInfo;

    class merchantWeapon
    {
        public GameObject btn;
        public Weapon weapon;
        public int price;
        public string merchantCmt;

        public merchantWeapon(GameObject b, Weapon w, int p, string c) { btn = b; weapon = w;price = p; merchantCmt = c; }
    }

    Dictionary<int, merchantWeapon> merchantWeaponDic = new Dictionary<int, merchantWeapon>();
    int? selected = null;
    int nowGold;
    E_Language nowLang;


    void Init()
    {
       
        selected = null;
        merchantWeaponDic.Clear();

        buyBtn.interactable = false;
        nowGold = GoldManager.GetInstance().Gold;
        nowGoldTxt.text = nowGold.ToString();

        int merchantWeaponCount=(int)E_Weapon.MAX;

        nowLang = LanguageManager.GetInstance().Language;
      
        for (int i = 0;i < merchantWeaponCount; i++)
        {
            string name="";
            string comment="";
            int price = merchantWeaponSheet.dataArray[i].Price;
              switch (nowLang)
            {
                case E_Language.KOREAN:
                    name = weaponInfo.dataArray[i].Namekor;
                    comment = merchantWeaponSheet.dataArray[i].Mwcmtkor;
                    break;

                case E_Language.ENGLISH:
                    name = weaponInfo.dataArray[i].Nameeng;
                    comment = merchantWeaponSheet.dataArray[i].Mwcmteng;
                    break;
            }
           
            int num = i;

            GameObject btn = Instantiate(buyWeaponBtn,buyWeaponContent.transform);
            Weapon weapon = WeaponInfoManager.GetInstance().CreateWeapon((E_Weapon)i);
                
               
            btn.GetComponent<Button>().onClick.AddListener(
                ()=>
                {
                    SelectWeapon(num);
                }
                
                );
            merchantWeaponDic.Add(num, new merchantWeapon(btn,weapon, price , comment));

            SetTxtsWeaponBtn(weapon, btn, price);
        }
        ShowDefaultComment();
        GoldAdjustShow(0);
    }

    void SelectWeapon(int originNumber)
    {
        if (selected == null)
        {

            AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.CLICK);
            selected = originNumber;
            merchantWeaponDic[originNumber].btn.GetComponent<Image>().color = selectedColor;
            GoldAdjustShow(merchantWeaponDic[originNumber].price);
            ShowWeaponComment(originNumber);

            if(GoldManager.GetInstance().CanPaymentThis(merchantWeaponDic[originNumber].price))
            buyBtn.interactable = true;
            else buyBtn.interactable = false;

            return;
        }
        if (selected == originNumber)   //취소할   때.
        {
            AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.CLICK);
            selected = null;
            merchantWeaponDic[originNumber].btn.GetComponent<Image>().color = Color.white;
            buyBtn.interactable = false;
            GoldAdjustShow(0);
            ShowDefaultComment();
            return;
        }
        AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.CLICK);
        //새로운 놈을 고를때.
        merchantWeaponDic[(int)selected].btn.GetComponent<Image>().color = Color.white;
        selected = originNumber;
        merchantWeaponDic[originNumber].btn.GetComponent<Image>().color = selectedColor;

        if (GoldManager.GetInstance().CanPaymentThis(merchantWeaponDic[originNumber].price))
            buyBtn.interactable = true;
        else buyBtn.interactable = false;

        GoldAdjustShow(merchantWeaponDic[originNumber].price);
        ShowWeaponComment(originNumber);

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
        int dch = merchantWeaponSheet.dataArray.Length-1;
        int dcl=dch;
        while (merchantWeaponSheet.dataArray[dcl].Weaponnum != 9000)
        {
            dcl--;
        }
        int randCmt =  Random.Range(dcl, dch + 1);
        string commment="";

        switch (nowLang)
        {
            case E_Language.KOREAN:
                commment = merchantWeaponSheet.dataArray[randCmt].Mwcmtkor;
                break;

            case E_Language.ENGLISH:
                commment = merchantWeaponSheet.dataArray[randCmt].Mwcmteng;
                break;
        }

        merchantCommentTxt.text = commment;
    }

    void ShowWeaponComment(int num)
    {
        merchantCommentTxt.text = merchantWeaponDic[num].merchantCmt;
    }

    void SetTxtsWeaponBtn(Weapon weapon, GameObject btn, int price)
    {
        btn.transform.GetChild(0).GetComponent<Image>().sprite = SpriteManager.GetInstance().GetWeaponSprite(weapon.weaponType);
        btn.transform.GetChild(1).GetComponent<Text>().text = weapon.name;
        btn.transform.GetChild(2).GetComponent<Text>().text = UIGeneralTextsManager.GetUIGeneralText("merchantWeaponBtn", "cap");
        btn.transform.GetChild(3).GetComponent<Text>().text = weapon.PlusCapability.ToString();
        btn.transform.GetChild(4).GetComponent<Text>().text = price.ToString();
    }
    
    public void OnClickBuy()
    {
        if (selected == null) Debug.LogError("웨폰 구입 선택 오류0");
        merchantWeapon selectedWeapon = merchantWeaponDic[(int)selected];
        Weapon weapon = WeaponInfoManager.GetInstance().CreateWeapon(selectedWeapon.weapon.weaponType);
        List<Weapon> temp = new List<Weapon>();
        temp.Add(weapon);

        Inventory.GetInstance().AddWeaponsToInven(temp);
        GoldManager.GetInstance().AdjustGold(-1*selectedWeapon.price, GoldManager.E_PayType.BUY_BY_MERCHANT);
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

        for (int i = 0; i < buyWeaponContent.transform.childCount; i++)
        {
            Destroy(buyWeaponContent.transform.GetChild(i).gameObject);
        }
    }
}
