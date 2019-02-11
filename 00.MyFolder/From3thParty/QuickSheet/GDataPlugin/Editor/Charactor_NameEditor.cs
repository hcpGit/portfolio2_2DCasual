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
[CustomEditor(typeof(Charactor_Name))]
public class Charactor_NameEditor : BaseGoogleEditor<Charactor_Name>
{	    
    public override bool Load()
    {        
        Charactor_Name targetData = target as Charactor_Name;
        
        var client = new DatabaseClient("", "");
        string error = string.Empty;
        var db = client.GetDatabase(targetData.SheetName, ref error);	
        var table = db.GetTable<Charactor_NameData>(targetData.WorksheetName) ?? db.CreateTable<Charactor_NameData>(targetData.WorksheetName);
        
        List<Charactor_NameData> myDataList = new List<Charactor_NameData>();
        
        var all = table.FindAll();
        foreach(var elem in all)
        {
            Charactor_NameData data = new Charactor_NameData();
            
            data = Cloner.DeepCopy<Charactor_NameData>(elem.Element);
            myDataList.Add(data);
        }
                
        targetData.dataArray = myDataList.ToArray();
        
        EditorUtility.SetDirty(targetData);
        AssetDatabase.SaveAssets();
        
        return true;
    }
}
