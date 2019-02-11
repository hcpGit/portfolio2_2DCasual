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
[CustomEditor(typeof(Merchant_Weapon))]
public class Merchant_WeaponEditor : BaseGoogleEditor<Merchant_Weapon>
{	    
    public override bool Load()
    {        
        Merchant_Weapon targetData = target as Merchant_Weapon;
        
        var client = new DatabaseClient("", "");
        string error = string.Empty;
        var db = client.GetDatabase(targetData.SheetName, ref error);	
        var table = db.GetTable<Merchant_WeaponData>(targetData.WorksheetName) ?? db.CreateTable<Merchant_WeaponData>(targetData.WorksheetName);
        
        List<Merchant_WeaponData> myDataList = new List<Merchant_WeaponData>();
        
        var all = table.FindAll();
        foreach(var elem in all)
        {
            Merchant_WeaponData data = new Merchant_WeaponData();
            
            data = Cloner.DeepCopy<Merchant_WeaponData>(elem.Element);
            myDataList.Add(data);
        }
                
        targetData.dataArray = myDataList.ToArray();
        
        EditorUtility.SetDirty(targetData);
        AssetDatabase.SaveAssets();
        
        return true;
    }
}
