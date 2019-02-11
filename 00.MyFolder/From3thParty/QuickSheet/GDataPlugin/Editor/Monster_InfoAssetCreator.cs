using UnityEngine;
using UnityEditor;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
/// 
public partial class GoogleDataAssetUtility
{
    [MenuItem("Assets/Create/Google/Monster_Info")]
    public static void CreateMonster_InfoAssetFile()
    {
        Monster_Info asset = CustomAssetUtility.CreateAsset<Monster_Info>();
        asset.SheetName = "MPQGame";
        asset.WorksheetName = "Monster_Info";
        EditorUtility.SetDirty(asset);        
    }
    
}