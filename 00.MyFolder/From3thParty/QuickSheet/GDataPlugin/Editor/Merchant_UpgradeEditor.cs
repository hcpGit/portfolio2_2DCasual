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
[CustomEditor(typeof(Merchant_Upgrade))]
public class Merchant_UpgradeEditor : BaseGoogleEditor<Merchant_Upgrade>
{	    
    public override bool Load()
    {        
        Merchant_Upgrade targetData = target as Merchant_Upgrade;
        
        var client = new DatabaseClient("", "");
        string error = string.Empty;
        var db = client.GetDatabase(targetData.SheetName, ref error);	
        var table = db.GetTable<Merchant_UpgradeData>(targetData.WorksheetName) ?? db.CreateTable<Merchant_UpgradeData>(targetData.WorksheetName);
        
        List<Merchant_UpgradeData> myDataList = new List<Merchant_UpgradeData>();
        
        var all = table.FindAll();
        foreach(var elem in all)
        {
            Merchant_UpgradeData data = new Merchant_UpgradeData();
            
            data = Cloner.DeepCopy<Merchant_UpgradeData>(elem.Element);
            myDataList.Add(data);
        }
                
        targetData.dataArray = myDataList.ToArray();
        
        EditorUtility.SetDirty(targetData);
        AssetDatabase.SaveAssets();
        
        return true;
    }
}
