using UnityEngine;
using UnityEditor;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
/// 
public partial class GoogleDataAssetUtility
{
    [MenuItem("Assets/Create/Google/Client_QuestSay_Normal")]
    public static void CreateClient_QuestSay_NormalAssetFile()
    {
        Client_QuestSay_Normal asset = CustomAssetUtility.CreateAsset<Client_QuestSay_Normal>();
        asset.SheetName = "MPQGame";
        asset.WorksheetName = "Client_QuestSay_Normal";
        EditorUtility.SetDirty(asset);        
    }
    
}