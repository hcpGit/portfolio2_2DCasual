using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PayFormula  //무조건 돈만 딱 주는 공식. 다른 거 하지마 제발
{
    //만기 고려 해서 돈!!!!!!!!!!!!!!!!!!!!!!
    //만기!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    //만기가 작으면 돈 이빠이 주고!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    /*
   의뢰인의 오리진 오더 퀘스트
       */
    public static int CalculateClientRequestDeposit(Quest orderedQuest, int expireTerm)//의뢰인이 계약할떄 주는 계약금.
    {
        return (int)(ClientWholePayment(orderedQuest, expireTerm) * Constant.depositRatio);
    }
    /*
     의뢰인의 오리진 오더 퀘스트
     플레이어가 제출하기위해 선택한 리스트
     //의뢰인이 의뢰를 찾으러 올때 주는 성공 보수금.
         */
    public static int CalculateClientRequestLastPayment(Quest orderedQuest,int expireTerm , float completeness , int dayspastDue)
    {
        float wholeCPayment = ClientWholePayment(orderedQuest, expireTerm);
        float wholeLastPayment = wholeCPayment - (wholeCPayment * Constant.depositRatio);

        int takenDeposit = (int)(wholeCPayment * Constant.depositRatio);

        float outDueAdjust=0f;
        if (dayspastDue <= 0)
        {
            outDueAdjust = 1f;
        }
        else if (dayspastDue >= 1 && dayspastDue < 3)    //하루 이틀은 10프로만 깎음
        {
            outDueAdjust = 0.9f;
        }
        else if (dayspastDue >= 3 && dayspastDue < 6)    //3일 부터 5일은 30프로
        {
            outDueAdjust = 0.7f;
        }
        else if (dayspastDue >= 6 )    //6일 이상은 10퍼만 줌
        {
            outDueAdjust = 0.1f;
        }

        if (completeness == 0)
        {
            return -1 * takenDeposit;//의뢰금 환수.
        }
        if (completeness <= 50)
        {
            return (int)((wholeCPayment * 0.1f)*outDueAdjust);
        }
        if (completeness <= 80)
        {
            return (int)((wholeCPayment * 0.50f) * outDueAdjust) ;
        }
        if (completeness <= 95)
        {
            return (int)((wholeCPayment * 0.65f) * outDueAdjust);    //착수금이랑 쁠러스 하면 총 금액의 85프로 지급.
        }
        
        return (int)(wholeLastPayment * outDueAdjust);
    }
    /*
     * 플레이어가 작성했던 퀘스트
    헌터가 퀘스트 받아갈때 헌터에게 주는 계약금
       */
    public static int CalculateHuntDeposit(Quest writtenQuest, int expireTerm)
    {
        return (int)( 

            ( ClientWholePayment(writtenQuest, expireTerm)* Constant.hunterRatio) 
            
            * Constant.depositRatio
            
            );
        
    }
    /*
     * 플레이어가 작성했던 퀘스트
     * 헌터가 실제로 잡아온 것.
    헌터가 의뢰 완수 후 반납 하러 왔을 때 주는 돈(성공보수)
       */
    public static int CalculateHuntLastPayment(Quest writtenQuest, int expireTerm, List<QuestPerMob> huntedList, E_RewardType paymentWay)
    {
        int payment = 0;
        float hunterWholePay = ClientWholePayment(writtenQuest, expireTerm) * Constant.hunterRatio;
        float hunterLastPay = hunterWholePay - (hunterWholePay * Constant.depositRatio);
        float completeness = writtenQuest.GetCompleteness(huntedList);
        switch (paymentWay)
        {
            case E_RewardType.ALL_PAYMENT:
                payment = (int)hunterLastPay;
                break;

            case E_RewardType.PARTIAL_PAYMENT:
                payment = (int)(hunterLastPay * (completeness/100));

                break;
            case E_RewardType.PAYMENT_DENY:
                payment = 0;
                break;
        }
        return payment;
    }

    static float ClientWholePayment(Quest orderedQuest, int expireTerm)
    {
        float Q = orderedQuest.GetWeight();
        float CPayh = Constant.CPayh;
        float CPayExpireh = Constant.CPayExpireh;

        return Q * CPayh + (((Q * CPayExpireh )/ expireTerm));
    }
}
