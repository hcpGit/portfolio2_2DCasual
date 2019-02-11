using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndJudgeManager : Singleton<GameEndJudgeManager>
{
    public void Load(SaveStructure st)
    {
        goldRunOutContinuityDays = st.goldRunOutContinuityDays;
        threatMaxContinuityDays = st.threatMaxContinuityDays;
    }
    int goldRunOutContinuityDays = 0;//골드가 연속으로 떨어졌던 날들.
    public int GoldRunOutContinuityDays
    {
        get {
            return goldRunOutContinuityDays;
        }
    }
    int threatMaxContinuityDays = 0;//위협도 맥스가 연속이었던 날들.
    public int ThreatMaxContinuityDays
    {
        get
        {
            return threatMaxContinuityDays;
        }
    }

    public void didGoldRunOut(bool runOut)
    {
        if (runOut) goldRunOutContinuityDays++;
        else goldRunOutContinuityDays = 0;
    }
    public void didThreatHitsMax(bool hit)
    {
        if (hit) threatMaxContinuityDays++;
        else threatMaxContinuityDays = 0;
    }
    public bool IsGameEnd()
    {
        return (goldRunOutContinuityDays > Constant.GoldRunOutHoldDays) || (threatMaxContinuityDays > Constant.ThreatHoldDays);
    }
    public void InitForNewStart()
    {
        goldRunOutContinuityDays = 0;
        threatMaxContinuityDays = 0;
    }
}
