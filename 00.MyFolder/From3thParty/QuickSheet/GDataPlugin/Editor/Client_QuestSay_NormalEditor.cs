using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using GDataDB;
using GDataDB.Linq;

using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
[CustomEditor(typeof(Client_QuestSay_Normal))]
public class Client_QuestSay_NormalEditor : BaseGoogleEditor<Client_QuestSay_Normal>
{	    
    public override bool Load()
    {        
        Client_QuestSay_Normal targetData = target as Client_QuestSay_Normal;
        
        var client = new DatabaseClient("", "");
        string error = string.Empty;
        var db = client.GetDatabase(targetData.SheetName, ref error);	
        var table = db.GetTable<Client_QuestSay_NormalData>(targetData.WorksheetName) ?? db.CreateTable<Client_QuestSay_NormalData>(targetData.WorksheetName);
        
        List<Client_QuestSay_NormalData> myDataList = new List<Client_QuestSay_NormalData>();
        
        var all = table.FindAll();
        foreach(var elem in all)
        {
            Client_QuestSay_NormalData data = new Client_QuestSay_NormalData();
            
            data = Cloner.DeepCopy<Client_QuestSay_NormalData>(elem.Element);
            myDataList.Add(data);
        }
                
        targetData.dataArray = myDataList.ToArray();
        
        EditorUtility.SetDirty(targetData);
        AssetDatabase.SaveAssets();
        
        return true;
    }
}
