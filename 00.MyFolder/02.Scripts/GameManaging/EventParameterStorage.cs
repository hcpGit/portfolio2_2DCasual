using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventParameterStorage  : Singleton<EventParameterStorage> 
{

    public void ParametersReset()
    {
        selectedQuestKey = "";
        PlayerChoice = true;
        PlayerMultipleChoice = 0;
        QuestCompareCompleteness = 0f;
    }
    public string selectedQuestKey;

    //[SerializeField]
    public bool PlayerChoice;
    /*
    public bool PlayerChoice
    {
        set;get;
    }
    */
 //  [SerializeField]
    public int PlayerMultipleChoice;
    /*
    public  int PlayerMultipleChoice
    {
        set; get;
    }
    */
  //  [SerializeField]
    public float QuestCompareCompleteness;
    /*
    public float QuestCompareCompleteness
    {
        set;get;
    }
    */
}
