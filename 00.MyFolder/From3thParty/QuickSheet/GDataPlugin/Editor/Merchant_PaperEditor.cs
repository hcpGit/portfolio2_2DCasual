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
[CustomEditor(typeof(Merchant_Paper))]
public class Merchant_PaperEditor : BaseGoogleEditor<Merchant_Paper>
{	    
    public override bool Load()
    {        
        Merchant_Paper targetData = target as Merchant_Paper;
        
        var client = new DatabaseClient("", "");
        string error = string.Empty;
        var db = client.GetDatabase(targetData.SheetName, ref error);	
        var table = db.GetTable<Merchant_PaperData>(targetData.WorksheetName) ?? db.CreateTable<Merchant_PaperData>(targetData.WorksheetName);
        
        List<Merchant_PaperData> myDataList = new List<Merchant_PaperData>();
        
        var all = table.FindAll();
        foreach(var elem in all)
        {
            Merchant_PaperData data = new Merchant_PaperData();
            
            data = Cloner.DeepCopy<Merchant_PaperData>(elem.Element);
            myDataList.Add(data);
        }
                
        targetData.dataArray = myDataList.ToArray();
        
        EditorUtility.SetDirty(targetData);
        AssetDatabase.SaveAssets();
        
        return true;
    }
}
