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
[CustomEditor(typeof(Client_Text))]
public class Client_TextEditor : BaseGoogleEditor<Client_Text>
{	    
    public override bool Load()
    {        
        Client_Text targetData = target as Client_Text;
        
        var client = new DatabaseClient("", "");
        string error = string.Empty;
        var db = client.GetDatabase(targetData.SheetName, ref error);	
        var table = db.GetTable<Client_TextData>(targetData.WorksheetName) ?? db.CreateTable<Client_TextData>(targetData.WorksheetName);
        
        List<Client_TextData> myDataList = new List<Client_TextData>();
        
        var all = table.FindAll();
        foreach(var elem in all)
        {
            Client_TextData data = new Client_TextData();
            
            data = Cloner.DeepCopy<Client_TextData>(elem.Element);
            myDataList.Add(data);
        }
                
        targetData.dataArray = myDataList.ToArray();
        
        EditorUtility.SetDirty(targetData);
        AssetDatabase.SaveAssets();
        
        return true;
    }
}
