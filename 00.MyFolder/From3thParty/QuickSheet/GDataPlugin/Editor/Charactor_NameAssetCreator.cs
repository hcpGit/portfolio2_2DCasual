using UnityEngine;
using UnityEditor;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
/// 
public partial class GoogleDataAssetUtility
{
    [MenuItem("Assets/Create/Google/Charactor_Name")]
    public static void CreateCharactor_NameAssetFile()
    {
        Charactor_Name asset = CustomAssetUtility.CreateAsset<Charactor_Name>();
        asset.SheetName = "MPQGame";
        asset.WorksheetName = "Charactor_Name";
        EditorUtility.SetDirty(asset);        
    }
    
}