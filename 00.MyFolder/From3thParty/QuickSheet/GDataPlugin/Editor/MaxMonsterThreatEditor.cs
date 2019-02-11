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
[CustomEditor(typeof(MaxMonsterThreat))]
public class MaxMonsterThreatEditor : BaseGoogleEditor<MaxMonsterThreat>
{	    
    public override bool Load()
    {        
        MaxMonsterThreat targetData = target as MaxMonsterThreat;
        
        var client = new DatabaseClient("", "");
        string error = string.Empty;
        var db = client.GetDatabase(targetData.SheetName, ref error);	
        var table = db.GetTable<MaxMonsterThreatData>(targetData.WorksheetName) ?? db.CreateTable<MaxMonsterThreatData>(targetData.WorksheetName);
        
        List<MaxMonsterThreatData> myDataList = new List<MaxMonsterThreatData>();
        
        var all = table.FindAll();
        foreach(var elem in all)
        {
            MaxMonsterThreatData data = new MaxMonsterThreatData();
            
            data = Cloner.DeepCopy<MaxMonsterThreatData>(elem.Element);
            myDataList.Add(data);
        }
                
        targetData.dataArray = myDataList.ToArray();
        
        EditorUtility.SetDirty(targetData);
        AssetDatabase.SaveAssets();
        
        return true;
    }
}
