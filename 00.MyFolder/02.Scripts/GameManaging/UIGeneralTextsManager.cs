using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGeneralTextsManager : Singleton<UIGeneralTextsManager> {
   
    UIGeneralTexts uigt;

    static Dictionary<string, string> uigtDic = new Dictionary<string, string>();
   
    public void Init()
    {
        uigt = Resources.Load("Sheet/UIGeneralTexts") as UIGeneralTexts;
        LanguageSetting();
        LanguageManager.GetInstance().AddListnerLanguageChange(LanguageSetting);
    }


    public static string GetUIGeneralText(string uiName, string textName)
    {
        return uigtDic[ MakeUIGTKey(uiName, textName)];
    }

    public void LanguageSetting()
    {
        uigtDic.Clear();
        E_Language lang = LanguageManager.GetInstance().Language;

        for (int i = 0;i < uigt.dataArray.Length; i++)
        {
            string btnText = "";
            switch (lang)
            {
                case E_Language.KOREAN:
                    btnText = uigt.dataArray[i].Kor;
                    break;

                case E_Language.ENGLISH:
                    btnText = uigt.dataArray[i].Eng;
                    break;
            }
            string key = MakeUIGTKey(uigt.dataArray[i].Uiname, uigt.dataArray[i].Uigeneraltextkeyname);
            uigtDic.Add(key, btnText);
        }
    }

    static string MakeUIGTKey(string uiName, string textName)
    {
        return uiName + "/" + textName;
    }
}
