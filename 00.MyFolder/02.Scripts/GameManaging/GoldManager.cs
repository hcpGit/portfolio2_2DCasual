using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager : Singleton<GoldManager> {
    public enum E_PayType
    {
        NOTHING,
        FROM_CLIENT,
        TO_HUNTER,
        BUY_BY_MERCHANT,
        SELL_TO_MERCHANT,

        MAX
    }


    int golds=0;
    public int Gold
    {
        get { return golds; }
    }
    int clientIncomeToday = 0;  //오늘 클라리언트에게 받은 총 금액.
    public int ClientIncomeToday
    {
        get {
            return clientIncomeToday;
        }
    }
    int hunterPaymentToday = 0; //오늘 헌터에게 지불한 총 금액.
    public int HunterPaymentToday
    {
        get {
            return hunterPaymentToday;
        }
    }
    int todayStartWallet=0; //오늘 시작할때 들고 있던 돈.
    public int TodayStartWallet
    {
        get {
            return todayStartWallet;
        }
    }
    int merchantBuyCost = 0;
    public int MerchantBuyCost
    {
        get
        {
            return merchantBuyCost;
        }
    }

    int merchantSellCostIncome = 0;
    public int MerchantSellCostIncome
    {
        get
        {
            return merchantSellCostIncome;
        }
    }

    System.Action goldChanged;
   
    public void SetGold(int gold)
    {
        golds = gold;
        DayOff();
    }
    public void AdjustGold(int increaseOrDecrease ,E_PayType payType)//골드의 증감을 해줄때 쓰임.
    {
        AudioThing.GetInstance().PlaySFX(AudioThing.E_SFX.GOLD);
        switch (payType)
        {
            case E_PayType.FROM_CLIENT:
                clientIncomeToday += increaseOrDecrease;
                break;
            case E_PayType.TO_HUNTER:
                hunterPaymentToday += increaseOrDecrease;
                break;

            case E_PayType.BUY_BY_MERCHANT:
                merchantBuyCost += increaseOrDecrease;
                break;

            case E_PayType.SELL_TO_MERCHANT:
                merchantSellCostIncome += increaseOrDecrease;
                break;
        }

        golds += increaseOrDecrease;
        
        if(goldChanged!=null )
        goldChanged();

        Debug.Log("골드 조정" + increaseOrDecrease);
#if UNITY_ANDROID
        if (Social.localUser.authenticated)
        {
            if (golds > 50000)
            {
                Social.ReportProgress(GPGSIds.achievement_collect_50000_golds, 100.0, (sucess) =>
                {
                });
            }
        }
#endif
    }

    public bool CanPaymentThis(int price)   //이 가격을 살 수 있냐 없냐
    {
        if ((golds - price) >= 0) return true;
        return false;
    }
    public void AddGoldChangeListner(System.Action ac)
    {
        goldChanged += ac;
    }
    public void RemoveGoldChangeListner(System.Action ac)
    {
        goldChanged -= ac;
    }
    public void DayOff()
    {
        clientIncomeToday = 0;
        hunterPaymentToday = 0;
        todayStartWallet = golds;
        merchantBuyCost = 0;
        merchantSellCostIncome = 0;
    }
    public bool IsGoldRunOut()
    {
        return golds < 0;
    }
}
