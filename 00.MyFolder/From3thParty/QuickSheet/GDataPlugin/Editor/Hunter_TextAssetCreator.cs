using UnityEngine;
using UnityEditor;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
/// 
public partial class GoogleDataAssetUtility
{
    [MenuItem("Assets/Create/Google/Hunter_Text")]
    public static void CreateHunter_TextAssetFile()
    {
        Hunter_Text asset = CustomAssetUtility.CreateAsset<Hunter_Text>();
        asset.SheetName = "MPQGame";
        asset.WorksheetName = "Hunter_Text";
        EditorUtility.SetDirty(asset);        
    }
    
}