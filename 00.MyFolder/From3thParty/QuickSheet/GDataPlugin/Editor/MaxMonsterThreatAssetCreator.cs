using UnityEngine;
using UnityEditor;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
/// 
public partial class GoogleDataAssetUtility
{
    [MenuItem("Assets/Create/Google/MaxMonsterThreat")]
    public static void CreateMaxMonsterThreatAssetFile()
    {
        MaxMonsterThreat asset = CustomAssetUtility.CreateAsset<MaxMonsterThreat>();
        asset.SheetName = "MPQGame";
        asset.WorksheetName = "MaxMonsterThreat";
        EditorUtility.SetDirty(asset);        
    }
    
}