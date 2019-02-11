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
[CustomEditor(typeof(Evidence_Info))]
public class Evidence_InfoEditor : BaseGoogleEditor<Evidence_Info>
{	    
    public override bool Load()
    {        
        Evidence_Info targetData = target as Evidence_Info;
        
        var client = new DatabaseClient("", "");
        string error = string.Empty;
        var db = client.GetDatabase(targetData.SheetName, ref error);	
        var table = db.GetTable<Evidence_InfoData>(targetData.WorksheetName) ?? db.CreateTable<Evidence_InfoData>(targetData.WorksheetName);
        
        List<Evidence_InfoData> myDataList = new List<Evidence_InfoData>();
        
        var all = table.FindAll();
        foreach(var elem in all)
        {
            Evidence_InfoData data = new Evidence_InfoData();
            
            data = Cloner.DeepCopy<Evidence_InfoData>(elem.Element);
            myDataList.Add(data);
        }
                
        targetData.dataArray = myDataList.ToArray();
        
        EditorUtility.SetDirty(targetData);
        AssetDatabase.SaveAssets();
        
        return true;
    }
}
