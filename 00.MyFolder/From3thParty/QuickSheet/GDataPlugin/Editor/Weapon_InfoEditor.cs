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
[CustomEditor(typeof(Weapon_Info))]
public class Weapon_InfoEditor : BaseGoogleEditor<Weapon_Info>
{	    
    public override bool Load()
    {        
        Weapon_Info targetData = target as Weapon_Info;
        
        var client = new DatabaseClient("", "");
        string error = string.Empty;
        var db = client.GetDatabase(targetData.SheetName, ref error);	
        var table = db.GetTable<Weapon_InfoData>(targetData.WorksheetName) ?? db.CreateTable<Weapon_InfoData>(targetData.WorksheetName);
        
        List<Weapon_InfoData> myDataList = new List<Weapon_InfoData>();
        
        var all = table.FindAll();
        foreach(var elem in all)
        {
            Weapon_InfoData data = new Weapon_InfoData();
            
            data = Cloner.DeepCopy<Weapon_InfoData>(elem.Element);
            myDataList.Add(data);
        }
                
        targetData.dataArray = myDataList.ToArray();
        
        EditorUtility.SetDirty(targetData);
        AssetDatabase.SaveAssets();
        
        return true;
    }
}
