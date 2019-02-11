using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constant
{
    
    public static uint dayStartHour = 9;
    public static uint dayEndHour = 18;

    public static float CPayh = 0.9f;//페이 공식에 적용값.
    public static float CPayExpireh = 0.2f;//페이 공식에 만기 보건 적용값.

    public static float depositRatio = 0.2f;
    public static float hunterRatio = 0.5f; 

    public static int newStartGolds = 25000;

    public static int defenseExpense =  5000;
    static int threatHoldDays =         5;//위협도 연속으로 만땅일때 버틸 수 있는 날들.

    public static int ThreatHoldDays
    {
        get {
            return threatHoldDays;
        }
    }

    static int goldRunOutHoldDays = 5; //골드가 없어서 방위비용을 빚지게 될때 버틸 수 있는 날들/

    public static int GoldRunOutHoldDays
    {
        get
        {
            return goldRunOutHoldDays;
        }
    }

    public static readonly string  saveDataAllPath= Application.persistentDataPath + "/savedata.dat";

    public static readonly string saveFileNameInGPGSCloud = "MHBSave";

    public static readonly InGameTime GameStartTime = new InGameTime(1, 1, 2, 9, 0);

    public static readonly int hunterMaxEquipNum = 3;

    public static readonly int clientMaxOrderNum = 7;
}
