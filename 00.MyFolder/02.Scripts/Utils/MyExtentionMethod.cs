using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class MyExtentionMethod
{
    public static void AddMobEviOnce(this List<QuestPerMob> qpmlist, E_Monster mob, E_Evidence evi)
    {
        if (qpmlist == null) return;
        for (int i = 0; i < qpmlist.Count; i++)
        {
            if (qpmlist[i].IsIt(mob, evi))
            {
                qpmlist[i].number++;
                return;
            }
        }
        qpmlist.Add(new QuestPerMob(mob, evi, 1));
    }
    public static void AddQPM(this List<QuestPerMob> qpmlist, QuestPerMob qpm)
    {
        if (qpmlist == null|| qpm == null) return;

        for (int j = 0; j < qpmlist.Count; j++)
        {
            if (qpmlist[j].IsIt(qpm.mob, qpm.evidence))
            {
                qpmlist[j].number += qpm.number;
                return;
            }
        }
        qpmlist.Add(new QuestPerMob(qpm.mob,qpm.evidence,qpm.number));
    }
    public static void AddQPMList(this List<QuestPerMob> qpmlist, List<QuestPerMob> addList)
    {
        if (qpmlist == null || addList==null) return;

        bool matched = false;
        for (int i = 0; i < addList.Count; i++)
        {
            matched = false;
            for (int j = 0; j < qpmlist.Count; j++)
            {
                if (qpmlist[j].IsIt(addList[i].mob, addList[i].evidence))
                {
                    qpmlist[j].number += addList[i].number;
                    matched = true;
                    break;
                }
            }
            if (!matched)   //맞는 게 없으면
            {
                qpmlist.Add(new QuestPerMob(addList[i].mob, addList[i].evidence,addList[i].number));
            }
        }
    }
    public static string DebugString(this List<QuestPerMob> qpmlist)
    {
        if (qpmlist == null) return "QPM리스트가 널" ;
        string result = "";

        for (int i = 0; i < qpmlist.Count; i++)
        {
            result += "[" + i + "]" + qpmlist[i].ToString();
        }
        
        return result;
    }
    public static bool hasThisMobEvi(this List<QuestPerMob> qpmlist, E_Monster mob, E_Evidence evi)
    {
        if (qpmlist == null) return false;
        for (int i = 0; i < qpmlist.Count; i++)
        {
            if (qpmlist[i].IsIt(mob, evi))
                return true;
        }
        return false;
    }
    public static void OrderByMobEvi(this List<QuestPerMob> qpmlist)
    {
        var ol = from qpm in qpmlist
                 orderby  ((int)E_Evidence.MAX * (int)qpm.mob) + (int)qpm.evidence 
                 select  qpm ;
        
        List<QuestPerMob> temp = ol.ToList<QuestPerMob>();
        qpmlist.Clear();
        qpmlist.AddQPMList(temp);
    }
}
