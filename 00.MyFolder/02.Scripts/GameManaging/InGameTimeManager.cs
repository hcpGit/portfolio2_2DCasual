using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameTimeManager : Singleton<InGameTimeManager> {
    //30일 이 그냥 최대임.
    //12월에  30일 이니까
    //1년은 360일.
    //하루 시작은 9:0
    //하루 종료 시간은 18:0?
    //종료시각은 변경 가능.

    InGameTime mainTime = new InGameTime(0,1,0,0,0);    //나중에 로드 할때는 달라지겠지.

    public uint Year
    {
        get {
            return mainTime.Year;
        }
    }
    public uint Month
    {
        get
        {
            return mainTime.Month;
        }
    }
    public uint Day
    {
        get
        {
            return mainTime.Day;
        }
    }
    public uint Hour
    {
        get
        {
            return mainTime.Hour;
        }
    }
    public uint Minute
    {
        get
        {
            return mainTime.Minute;
        }
    }

    public void SetMainTime(uint year, uint month, uint day, uint hour, uint minute)
    {
        mainTime = new InGameTime(year, month, day, hour, minute);
    }
    
    public uint GetNowInGameTimeStamp()
    {
        return mainTime.InGameTimeMinuteStamp;
    }
    public InGameTime GetNowTime()
    {
        InGameTime temp = new InGameTime(mainTime.Year, mainTime.Month, mainTime.Day, mainTime.Hour, mainTime.Minute);
        return temp;
    }

    public void NewMorningStart()
    {
        mainTime.DayOnePlus();
        mainTime.SetInGameTimeHourMin(Constant.dayStartHour, 0);
    }

    public void Update15Minute(out bool isDayDone)   
    {
        uint hour = mainTime.Hour;
        uint minute = mainTime.Minute;

        if (minute < 45)
        {
            minute += 15;
            isDayDone = false;
            mainTime.SetInGameTimeHourMin(hour, minute);
            return;
        }
        if (hour == 17)
        {
            hour++;
            minute = 0;
            mainTime.SetInGameTimeHourMin(hour, minute);
            isDayDone = true;
            return;
        }
        hour++;
        minute = 0;
        mainTime.SetInGameTimeHourMin(hour, minute);
        isDayDone = false;
    }
    public void Load(SaveStructure st)
    {
        mainTime = st.savedDate;
    }

    public int GetDaysAfterGameStart()
    {
        return mainTime.GetDaysGap(Constant.GameStartTime);
    }
}
