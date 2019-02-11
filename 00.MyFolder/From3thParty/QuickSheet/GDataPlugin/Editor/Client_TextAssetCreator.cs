using UnityEngine;
using UnityEditor;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
/// 
public partial class GoogleDataAssetUtility
{
    [MenuItem("Assets/Create/Google/Client_Text")]
    public static void CreateClient_TextAssetFile()
    {
        Client_Text asset = CustomAssetUtility.CreateAsset<Client_Text>();
        asset.SheetName = "MPQGame";
        asset.WorksheetName = "Client_Text";
        EditorUtility.SetDirty(asset);        
    }
    
}