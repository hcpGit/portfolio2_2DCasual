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
[CustomEditor(typeof(UIGeneralTexts))]
public class UIGeneralTextsEditor : BaseGoogleEditor<UIGeneralTexts>
{	    
    public override bool Load()
    {        
        UIGeneralTexts targetData = target as UIGeneralTexts;
        
        var client = new DatabaseClient("", "");
        string error = string.Empty;
        var db = client.GetDatabase(targetData.SheetName, ref error);	
        var table = db.GetTable<UIGeneralTextsData>(targetData.WorksheetName) ?? db.CreateTable<UIGeneralTextsData>(targetData.WorksheetName);
        
        List<UIGeneralTextsData> myDataList = new List<UIGeneralTextsData>();
        
        var all = table.FindAll();
        foreach(var elem in all)
        {
            UIGeneralTextsData data = new UIGeneralTextsData();
            
            data = Cloner.DeepCopy<UIGeneralTextsData>(elem.Element);
            myDataList.Add(data);
        }
                
        targetData.dataArray = myDataList.ToArray();
        
        EditorUtility.SetDirty(targetData);
        AssetDatabase.SaveAssets();
        
        return true;
    }
}
