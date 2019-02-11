using UnityEngine;
using UnityEditor;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
/// 
public partial class GoogleDataAssetUtility
{
    [MenuItem("Assets/Create/Google/Weapon_Info")]
    public static void CreateWeapon_InfoAssetFile()
    {
        Weapon_Info asset = CustomAssetUtility.CreateAsset<Weapon_Info>();
        asset.SheetName = "MPQGame";
        asset.WorksheetName = "Weapon_Info";
        EditorUtility.SetDirty(asset);        
    }
    
}