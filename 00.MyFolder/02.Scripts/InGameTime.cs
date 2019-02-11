using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class InGameTime
{
    public static InGameTime DeepCopy(InGameTime sub)
    {
        return new InGameTime(sub.Year, sub.Month, sub.Day, sub.hour, sub.minute);
    }
    public uint GetDays()   //일 수만 반환
    {
        return year * 360 + month * 30 + day;
    }

    public static InGameTime GetInGameTimeByStamp(uint timeStamp)
    {
        uint days = timeStamp / (24 * 60);
        uint hour = (timeStamp % (24 * 60)) / 60;
        uint minute = (timeStamp % (24 * 60)) % 60;

        uint year = days / 360;
        uint month = (days % 360) / 30;
        uint day = (days % 360) % 30;
        if (month == 0) month = 1;
        if (day == 0) day = 1;

        return new InGameTime(year, month, day, hour, minute);
    }
    [SerializeField]
    uint year;
    [SerializeField]
    uint month;
    [SerializeField]
    uint day;
    [SerializeField]
    uint hour;
    [SerializeField]
    uint minute;
    public uint Year
    {
        get { return year; }
    }
    public uint Month
    {
        get { return month; }
    }
    public uint Day
    {
        get { return day; }
    }
    public uint Hour
    {
        get { return hour; }
    }
    public uint Minute
    {
        get { return minute; }
    }
    //Vector3
    //1일은 24 시간 ,  60분,

    public InGameTime(uint year , uint month , uint day , uint hour , uint minute)
    {
        this.year = year;
        this.month = month;
        this.day = day;
        this.hour = hour;
        this.minute = minute;
    }
    public void SetInGameTime(uint year, uint month, uint day, uint hour, uint minute)
    {
        this.year = year;
        this.month = month;
        this.day = day;
        this.hour = hour;
        this.minute = minute;
    }
    public void SetInGameTimeHourMin(uint hour, uint min)
    {
        this.hour = hour;
        this.minute = min;
    }


    public void DayOnePlus()
    {
        if (day < 30)
        {
            day++; return;
        }

        day = 1;

        if (month < 12)
        {
            month++; return;
        }

        month = 1;

        year++;

    }

    public void PlusDays(int plusDays)
    {
        if (plusDays < 0) return;
        for (int i = 0; i < plusDays; i++)
        {
            DayOnePlus();
        }
    }

    public int GetDaysGap(InGameTime subject)
    {
        uint days = 0;
        days = GetDays(); ;

        uint sjdays = 0;
        sjdays =subject.GetDays();

        return ((int)days) - ((int)sjdays);
    }

    public static InGameTime GetOnlyOneDayMinus(InGameTime subjectIGT)
    {
        uint dateMinusOneDay = 0;
        uint days = ((subjectIGT.Year * 360) + (subjectIGT.Month * 30) + subjectIGT.Day);
        days--;
        dateMinusOneDay = (days * (24 * 60)) + (subjectIGT.hour * 60) + subjectIGT.Minute;

        return InGameTime.GetInGameTimeByStamp(dateMinusOneDay);
    }
    


    public uint InGameTimeMinuteStamp  //분단위로만 체크.
    {
        get
        {
            uint result=0;
            uint days = 0;
            days += ((year * 360) + (month * 30) + day);
            result += (days * (24 * 60));
            result += hour * 60;
            result += minute;

            return result;
        }
    }

    public static bool operator <(InGameTime a, InGameTime b)
    {
        if (a.InGameTimeMinuteStamp < b.InGameTimeMinuteStamp)
            return true;
        else return false;

        /*
        if (a.year < b.year) return true;
        if (a.year > b.year) return false;

        if (a.month < b.month) return true;
        if (a.month > b.month) return false;

        if (a.day < b.day) return true;
        if (a.day > b.day) return false;

        if (a.hour < b.hour) return true;
        if (a.hour > b.hour) return false;

        if (a.minute < b.minute) return true;
        if (a.minute > b.minute) return false;

        return false;
        */
    }
    public static bool operator >(InGameTime a, InGameTime b)
    {
        if (a.InGameTimeMinuteStamp > b.InGameTimeMinuteStamp)
            return true;
        else return false;
        /*
        if (a.year > b.year) return true;
        if (a.year < b.year) return false;

        if (a.month > b.month) return true;
        if (a.month < b.month) return false;

        if (a.day > b.day) return true;
        if (a.day < b.day) return false;

        if (a.hour > b.hour) return true;
        if (a.hour < b.hour) return false;

        if (a.minute > b.minute) return true;
        if (a.minute < b.minute) return false;

        return false;
        */
    }
    public override string ToString()
    {
        return "[ "+year + "/" + month + "/" + day + "/" + hour + "/" + minute+"]";
    }
}
