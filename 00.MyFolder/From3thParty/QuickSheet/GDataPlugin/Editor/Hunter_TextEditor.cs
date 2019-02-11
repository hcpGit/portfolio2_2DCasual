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
[CustomEditor(typeof(Hunter_Text))]
public class Hunter_TextEditor : BaseGoogleEditor<Hunter_Text>
{	    
    public override bool Load()
    {        
        Hunter_Text targetData = target as Hunter_Text;
        
        var client = new DatabaseClient("", "");
        string error = string.Empty;
        var db = client.GetDatabase(targetData.SheetName, ref error);	
        var table = db.GetTable<Hunter_TextData>(targetData.WorksheetName) ?? db.CreateTable<Hunter_TextData>(targetData.WorksheetName);
        
        List<Hunter_TextData> myDataList = new List<Hunter_TextData>();
        
        var all = table.FindAll();
        foreach(var elem in all)
        {
            Hunter_TextData data = new Hunter_TextData();
            
            data = Cloner.DeepCopy<Hunter_TextData>(elem.Element);
            myDataList.Add(data);
        }
                
        targetData.dataArray = myDataList.ToArray();
        
        EditorUtility.SetDirty(targetData);
        AssetDatabase.SaveAssets();
        
        return true;
    }
}
