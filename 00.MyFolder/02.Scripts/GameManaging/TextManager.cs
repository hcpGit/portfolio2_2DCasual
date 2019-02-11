using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : Singleton<TextManager> {
    /*
    public static uint MakeNameKey(ushort firstName , ushort lastName)
    {
        uint key = 0;
        key = firstName;
        uint lastKey = lastName;
        key |= (lastKey << 16);
        return key;
    }
    public static void GetNameNumsByKey(uint key , out uint firNameNum, out uint lastNameNum)
    {
        lastNameNum = key >> 16;
        firNameNum = (key << 16) >> 16;
    }
    */
    public static string MakeName(string firstName, string lastName)
    {
        return firstName + " " + lastName;
    }
    Client_QuestSay_Normal clientSayNormal;
    Client_Text clientText;
    Hunter_Text hunterText;
    Charactor_Name cname;
    int nameMax;
    [SerializeField]
    Dictionary<string,bool> distributedNames = new Dictionary<string, bool>();  //원래 해쉬 셋으로 해야하는데 해쉬셋이 시리얼라이즈가 안되서 그냥 딕셔너리로 함.
    public Dictionary<string, bool> DistributedNames
    {
        get { return distributedNames; }
    }

    public void Init()
    {
        hunterText = Resources.Load("Sheet/Hunter_Text") as Hunter_Text;
        clientText = Resources.Load("Sheet/Client_Text") as Client_Text;
        clientSayNormal = Resources.Load("Sheet/Client_QuestSay_Normal") as Client_QuestSay_Normal;
        cname = Resources.Load("Sheet/Charactor_Name") as Charactor_Name;
        nameMax = cname.dataArray.Length;
    }

    public string GetNewbieName()
    {
        string makedName = "";
        while (true)
        {
            int firstNameNum = Random.Range(0, nameMax);
            int lastNameNum = Random.Range(0, nameMax);

            makedName = TextManager.MakeName(
                cname.dataArray[firstNameNum].Names,
                cname.dataArray[lastNameNum].Names
                );

            if (false == distributedNames.ContainsKey(makedName))
            {
                distributedNames.Add(makedName,true);
                break;
            }
        }
        return makedName;
    }
    public void HandInName(string name)
    {
        if (distributedNames.ContainsKey(name))
        {
            distributedNames.Remove(name);
        }
    }

    public string GetNormalClientQuestSay(E_ClientState sayNum, List<QuestPerMob> list)
    {
       // Debug.Log("겟노말클라이언트퀘스트세이" + CharactorFrame.GetInstance().clientIdea.CharaName);
        string result = "";

        for (int i = 0; i < list.Count; i++)
        {
            QuestPerMob qpm = list[i];

            int idx = (((int)E_Evidence.MAX) * ((int)qpm.mob)) + (int)qpm.evidence;
            string basic = "";

            switch (LanguageManager.GetInstance().Language)
            {
                case E_Language.KOREAN:
                    switch (sayNum)
                    {
                        case E_ClientState.COMMISSION_SAY_1:
                            basic = clientSayNormal.dataArray[idx].Say1kor;

                            break;
                        case E_ClientState.COMMISSION_SAY_2:
                            basic = clientSayNormal.dataArray[idx].Say2kor;

                            break;
                        case E_ClientState.COMMISSION_SAY_3:
                            basic = clientSayNormal.dataArray[idx].Say3kor;

                            break;
                    }
                    break;
                case E_Language.ENGLISH:
                    switch (sayNum)
                    {
                        case E_ClientState.COMMISSION_SAY_1:
                            basic = clientSayNormal.dataArray[idx].Say1eng;

                            break;
                        case E_ClientState.COMMISSION_SAY_2:
                            basic = clientSayNormal.dataArray[idx].Say2eng;

                            break;
                        case E_ClientState.COMMISSION_SAY_3:
                            basic = clientSayNormal.dataArray[idx].Say3eng;

                            break;
                    }
                    break;
            }

            result += basic.Replace("[0]", qpm.number.ToString())+" ";
        }
        return result;
    }

    public string GetClientText(E_ClientState state, ClientIdea client)
    {
        string result = "";
        Client_TextData textData = clientText.dataArray[(int)state];
        switch (LanguageManager.GetInstance().Language)
        {
            case E_Language.KOREAN:
                result = textData.Charatextkor;
                break;
            case E_Language.ENGLISH:
                result = textData.Charatexteng;
                break;
        }
        int deposit = PayFormula.CalculateClientRequestDeposit(client.OriginOrderedQuest, client.DaysExpire);
        

        switch (state)  //치환해야하는 문자열의 경우.
        {
            case E_ClientState.COMMISSION:
                int clientLastPayin100per = PayFormula.CalculateClientRequestLastPayment(
                    client.OriginOrderedQuest, client.DaysExpire, 100f, 0
                    );
                result = result.Replace("[0]", deposit.ToString());
                result = result.Replace("[1]", clientLastPayin100per.ToString());
                break;

            case E_ClientState.COMMISION_MAKE_DONE:
                
                result = result.Replace("[0]", client.DaysExpire.ToString());
                result = result.Replace("[1]", deposit.ToString());
                break;

            case E_ClientState.CHECK_NO_EXPIRE:
                result = result.Replace("[0]", client.DaysExpire.ToString());
                break;

            case E_ClientState.CHECK_IMPERFACT_00:  //뺐을 착수금으로 치환
                result = result.Replace("[0]", deposit.ToString());
                break;

            case E_ClientState.CHECK_IMPERFACT_50:  //성공보수금으로 치환
            case E_ClientState.CHECK_IMPERFACT_80:
            case E_ClientState.CHECK_IMPERFACT_95:
            case E_ClientState.CHECK_PERFACT:
                int outDue = 0;
                if (client.haveComeBeforeExpire)
                {
                    InGameTime clientExpire = client.GetExpire();
                    InGameTime nowTime = InGameTimeManager.GetInstance().GetNowTime();
                    outDue = nowTime.GetDaysGap(clientExpire);
                }
                int lastClientPayment = PayFormula.CalculateClientRequestLastPayment(client.OriginOrderedQuest,
                    client.DaysExpire,
                    EventParameterStorage.GetInstance().QuestCompareCompleteness, outDue);

                result = result.Replace("[0]", lastClientPayment.ToString());
                break;
        }

        return result;
    }
    public string[] GetClient_PlayerText(E_ClientState state, ClientIdea client)
    {
        string pt1 = null;
        string pt2 = null;
        Client_TextData textData = clientText.dataArray[(int)state];

        switch (LanguageManager.GetInstance().Language)
        {
            case E_Language.KOREAN:
                pt1 = textData.Playertext1kor;
                pt2 = textData.Playertext2kor;
                break;
            case E_Language.ENGLISH:
                pt1 = textData.Playertext1eng;
                pt2 = textData.Playertext2eng;
                break;
        }
        string[] result;
        if (pt2 == "-")
        {
            result = new string[1];
            result[0] = pt1;
        }
        else {
            result = new string[2];
            result[0] = pt1;
            result[1] = pt2;
        }
        return result;
    }

    public string GetHunterText(E_HunterState state, HunterIdea hunter)
    {
        string result = "";
        Hunter_TextData textData = hunterText.dataArray[(int)state];
        switch (LanguageManager.GetInstance().Language)
        {
            case E_Language.KOREAN:
                result = textData.Charatextkor;
                break;
            case E_Language.ENGLISH:
                result = textData.Charatexteng;
                break;
        }
        return result;
    }
    public string[] GetHunter_PlayerText(E_HunterState state, HunterIdea hunter)
    {
        string pt1 = null;
        string pt2 = null;
        Hunter_TextData textData = hunterText.dataArray[(int)state];

        switch (LanguageManager.GetInstance().Language)
        {
            case E_Language.KOREAN:
                pt1 = textData.Playertext1kor;
                pt2 = textData.Playertext2kor;
                break;
            case E_Language.ENGLISH:
                pt1 = textData.Playertext1eng;
                pt2 = textData.Playertext2eng;
                break;
        }
        string[] result;
        if (pt2 == "-")
        {
            result = new string[1];
            result[0] = pt1;
        }
        else
        {
            result = new string[2];
            result[0] = pt1;
            result[1] = pt2;
        }
        return result;
    }


    public void Load(SaveStructure st)
    {
        this.distributedNames = st.distributedNames;
    }
}
