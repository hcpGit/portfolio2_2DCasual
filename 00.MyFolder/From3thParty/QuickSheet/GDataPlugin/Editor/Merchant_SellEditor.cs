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
[CustomEditor(typeof(Merchant_Sell))]
public class Merchant_SellEditor : BaseGoogleEditor<Merchant_Sell>
{	    
    public override bool Load()
    {        
        Merchant_Sell targetData = target as Merchant_Sell;
        
        var client = new DatabaseClient("", "");
        string error = string.Empty;
        var db = client.GetDatabase(targetData.SheetName, ref error);	
        var table = db.GetTable<Merchant_SellData>(targetData.WorksheetName) ?? db.CreateTable<Merchant_SellData>(targetData.WorksheetName);
        
        List<Merchant_SellData> myDataList = new List<Merchant_SellData>();
        
        var all = table.FindAll();
        foreach(var elem in all)
        {
            Merchant_SellData data = new Merchant_SellData();
            
            data = Cloner.DeepCopy<Merchant_SellData>(elem.Element);
            myDataList.Add(data);
        }
                
        targetData.dataArray = myDataList.ToArray();
        
        EditorUtility.SetDirty(targetData);
        AssetDatabase.SaveAssets();
        
        return true;
    }
}
