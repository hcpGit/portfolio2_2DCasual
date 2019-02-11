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
[CustomEditor(typeof(Monster_Info))]
public class Monster_InfoEditor : BaseGoogleEditor<Monster_Info>
{	    
    public override bool Load()
    {        
        Monster_Info targetData = target as Monster_Info;
        
        var client = new DatabaseClient("", "");
        string error = string.Empty;
        var db = client.GetDatabase(targetData.SheetName, ref error);	
        var table = db.GetTable<Monster_InfoData>(targetData.WorksheetName) ?? db.CreateTable<Monster_InfoData>(targetData.WorksheetName);
        
        List<Monster_InfoData> myDataList = new List<Monster_InfoData>();
        
        var all = table.FindAll();
        foreach(var elem in all)
        {
            Monster_InfoData data = new Monster_InfoData();
            
            data = Cloner.DeepCopy<Monster_InfoData>(elem.Element);
            myDataList.Add(data);
        }
                
        targetData.dataArray = myDataList.ToArray();
        
        EditorUtility.SetDirty(targetData);
        AssetDatabase.SaveAssets();
        
        return true;
    }
}
