using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : Singleton<LanguageManager> {
    E_Language lang = E_Language.KOREAN;
    Dictionary<int, string> englishMonthDic = new Dictionary<int, string>();

    System.Action languageChanged;

    public void RemoveListnerLanguageChange(System.Action changeAction)
    {
        if (languageChanged != null)
            languageChanged -= changeAction;

    }

    public void AddListnerLanguageChange(System.Action changeAction)//언어 설정 변경시 호출할 애들
    {
        languageChanged += changeAction;
    }

    public E_Language Language
    {
        get {
            return lang;
        }
    }

    public void SetLanguage(E_Language lang)
    {
        if (this.lang == lang) return;
        this.lang = lang;
        if(languageChanged!=null)
        languageChanged();  //언어 변경 이벤트 발생.
    }

    public string GetEngMonth(uint month)
    {
        return englishMonthDic[(int)month];
    }

    public string GetDateString(InGameTime date)
    {
        string str="";
        switch (lang)
        {
            case E_Language.KOREAN:
                str = date.Year.ToString() + "년 " + date.Month.ToString() + "월 " + date.Day.ToString() + "일";
                break;

            case E_Language.ENGLISH:
                str = englishMonthDic[(int)date.Month] + " " + date.Day + " ," + date.Year;
                break;
        }
        return str;
    }

    public void Init()
    {
        englishMonthDic.Add(1, "Jan.");
        englishMonthDic.Add(2, "Feb.");
        englishMonthDic.Add(3, "Mar.");
        englishMonthDic.Add(4, "Apr.");
        englishMonthDic.Add(5, "May.");
        englishMonthDic.Add(6, "Jun.");
        englishMonthDic.Add(7, "Jul.");
        englishMonthDic.Add(8, "Aug.");
        englishMonthDic.Add(9, "Sep.");
        englishMonthDic.Add(10, "Oct.");
        englishMonthDic.Add(11, "Nov.");
        englishMonthDic.Add(12, "Dec.");
    }
}
