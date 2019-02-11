using UnityEngine;
using UnityEditor;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
/// 
public partial class GoogleDataAssetUtility
{
    [MenuItem("Assets/Create/Google/Evidence_Info")]
    public static void CreateEvidence_InfoAssetFile()
    {
        Evidence_Info asset = CustomAssetUtility.CreateAsset<Evidence_Info>();
        asset.SheetName = "MPQGame";
        asset.WorksheetName = "Evidence_Info";
        EditorUtility.SetDirty(asset);        
    }
    
}