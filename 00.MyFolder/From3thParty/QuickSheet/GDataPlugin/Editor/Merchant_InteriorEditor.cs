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
[CustomEditor(typeof(Merchant_Interior))]
public class Merchant_InteriorEditor : BaseGoogleEditor<Merchant_Interior>
{	    
    public override bool Load()
    {        
        Merchant_Interior targetData = target as Merchant_Interior;
        
        var client = new DatabaseClient("", "");
        string error = string.Empty;
        var db = client.GetDatabase(targetData.SheetName, ref error);	
        var table = db.GetTable<Merchant_InteriorData>(targetData.WorksheetName) ?? db.CreateTable<Merchant_InteriorData>(targetData.WorksheetName);
        
        List<Merchant_InteriorData> myDataList = new List<Merchant_InteriorData>();
        
        var all = table.FindAll();
        foreach(var elem in all)
        {
            Merchant_InteriorData data = new Merchant_InteriorData();
            
            data = Cloner.DeepCopy<Merchant_InteriorData>(elem.Element);
            myDataList.Add(data);
        }
                
        targetData.dataArray = myDataList.ToArray();
        
        EditorUtility.SetDirty(targetData);
        AssetDatabase.SaveAssets();
        
        return true;
    }
}
