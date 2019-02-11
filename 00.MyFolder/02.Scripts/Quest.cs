using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Quest 
{
    public static string MakeQuestKey(string charaName)
    {
       return charaName+"-"+InGameTimeManager.GetInstance().GetNowInGameTimeStamp().ToString();
    }
    [SerializeField]
    string clientName;
    public string ClientName
    {
        get { return clientName; }
        set { clientName = value; }
    }
    [SerializeField]
    string key;
    public string Key
    {
        get { return key; }
    }
    /*
    public void SetQuestKey(string key)
    {
        this.key = key;
    }
    public string GetKey()
    {
        return key;
    }

    */
    [SerializeField]
    InGameTime orderDate;
    public InGameTime OrderDate { 
        get { return orderDate; }
    }
    [SerializeField]
    InGameTime expireDate;
    public InGameTime ExpireDate
    {
        get { return expireDate; }
    }

    public void SettingQuestMetaDataByToday(string key, string name , int daysToExpire , List<QuestPerMob> questList)
    {
        this.key = key;
        this.clientName = name;
        this.questList = questList;
        SetOrderDateAtNowTime();
        SetExpireAfterDays(daysToExpire);
    }

    public void SetOrderDateAtNowTime()
    {
        InGameTime orTemp = InGameTimeManager.GetInstance().GetNowTime();
        orderDate = orTemp;
    }

    public void SetExpireAfterDays(int days)
    {
        InGameTime exTemp = InGameTimeManager.GetInstance().GetNowTime();
        exTemp.PlusDays(days);
        exTemp.SetInGameTimeHourMin( (uint)(Random.Range(Constant.dayStartHour, (Constant.dayEndHour - 1))), 0);
        expireDate = exTemp;
    }



    public void SetOrderDate(uint year , uint month , uint day , uint hour ,uint minute)
    {
        if (orderDate == null)
        {
            orderDate = new InGameTime(year, month, day, hour, minute);
            return; 
        }
        orderDate.SetInGameTime(year, month, day, hour, minute);
    }
    public void SetExpireDate(uint year, uint month, uint day, uint hour, uint minute)
    {
        if (expireDate == null)
        {
            expireDate = new InGameTime(year, month, day, hour, minute);
            return;
        }
        expireDate.SetInGameTime(year, month, day, hour, minute);
    }
    public void SetKey(string key)
    {
        this.key = key;
    }



   

    public Quest() { }
    [SerializeField]
    List<QuestPerMob> questList = new List<QuestPerMob>();

    public List<QuestPerMob> QuestList
    {
        get { return questList; }
    }


    public void AddQuestMob(QuestPerMob QPM) //있나 체크하고 넣기.
    {
        questList.AddQPM(QPM);
    }

    public void AddQuestMob(E_Monster mob, E_Evidence evidence, int number) //있나 체크하고 넣기.
    {
        QuestPerMob qpm = new QuestPerMob(mob, evidence, number);
        AddQuestMob(qpm);
    }

    public void AddQuestMob(List<QuestPerMob> list)
    {
        questList.AddQPMList(list);
    }

    public void DebugQuest()
    {
        foreach (QuestPerMob n in questList)
        {
            Debug.LogFormat("몹 {0} , 증거품 {1} , 몇 개 {2}", n.mob, n.evidence, n.number);
        }
    }

    public float GetCompleteness(List<QuestPerMob> compare)
    {
        if (compare.Count == 0) return 0f;
        float compareWeight = 0;

        float myWeight = GetWeight();

        if (myWeight <= 0)
        {
            Debug.LogError("퀘스트에 내용 없는데 완성도 호출");
        }

        for (int i = 0; i < compare.Count; i++)
        {
            for (int j = 0; j < questList.Count; j++)
            {
                if (questList[j].IsIt(compare[i].mob, compare[i].evidence))
                {
                    compareWeight += compare[i].GetWeight();
                    break;
                }
            }
        }
        
        return (compareWeight / myWeight) * 100;    //백분율로
    }

    public float GetWeight()
    {
        float result=0;
        if (questList == null) return 0f;
        for (int i = 0; i < questList.Count; i++)
        {
            result += questList[i].GetWeight();
        }
        return result;
    }
    public override string ToString()
    {
        string od = "";
        string ed = "";
        string list = "";
        if (orderDate == null)
        {
            od = "[널]";
        }
        else {
            od = orderDate.ToString();
        }
        if (expireDate == null)
        {
            ed = "[널]";
        }
        else
        {
            ed = expireDate.ToString();
        }
        foreach (QuestPerMob qpm in questList)
        {
            list += qpm.ToString();
        }
        return "[ 퀘스트 의뢰인이름 = " + clientName + " 키 = " + key + " 의뢰시간 = " + od + "만기시간 = " + ed + " 내용 = " + list + " ]";
    }
}
