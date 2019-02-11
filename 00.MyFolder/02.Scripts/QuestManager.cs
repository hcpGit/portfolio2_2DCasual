using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    Dictionary<string ,  Quest> questMg = new Dictionary<string, Quest>();
    public Dictionary<string, Quest> QuestDic
    {
        get {
            return questMg;
        }
    }

    public List<Quest> GetNowWrittenQuestList()
    {
        Dictionary<string, Quest>.Enumerator enu = questMg.GetEnumerator();
        List<Quest> temp = new List<Quest>();
        while (enu.MoveNext())
        {
            temp.Add(enu.Current.Value);
        }
        return temp;
    }

    public void AddQuest(string key , Quest quest)
    {
        questMg.Add(key, quest);
    }
    public Quest GetQuest(string key)
    {
        return questMg[key];
    }
    public void Load(SaveStructure st)
    {
        Dictionary<string, Quest> savedMg = st.questDic;
        questMg = savedMg;
    }
    public void DayOff()    //연관된 캐릭터 없는 퀘스트를 삭제
    {
        List<CharactorIdea> charaList = CharactorManager.GetInstance().CharaList;
        Dictionary<string, Quest>.Enumerator enu = questMg.GetEnumerator();
        bool associated = false;

        List<string> deleteTarget = new List<string>();

        while (enu.MoveNext())
        {
            associated = false;
            string Qkey = enu.Current.Key;

            for (int i = 0; i < charaList.Count; i++)
            {
                if (Qkey == charaList[i].associatedQuestKey)    //연관된 애가 있으므로 삭제 대상 패쓰.
                {
                    associated = true;
                    break;
                }
            }

            if (false == associated)
            {
                deleteTarget.Add(Qkey);
            }
        }
        for (int i = 0; i < deleteTarget.Count; i++)
        {
            questMg.Remove(deleteTarget[i]);
        }
    }

    public float GetAverageQuestCap()
    {
        float result = 0f;
        Dictionary<string, Quest>.Enumerator enu = questMg.GetEnumerator();
        while (enu.MoveNext())
        {
            result += enu.Current.Value.GetWeight();
        }
        return result / questMg.Count;
    }
}
