using UnityEngine;
using UnityEditor;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
/// 
public partial class GoogleDataAssetUtility
{
    [MenuItem("Assets/Create/Google/Merchant_Weapon")]
    public static void CreateMerchant_WeaponAssetFile()
    {
        Merchant_Weapon asset = CustomAssetUtility.CreateAsset<Merchant_Weapon>();
        asset.SheetName = "MPQGame";
        asset.WorksheetName = "Merchant_Weapon";
        EditorUtility.SetDirty(asset);        
    }
    
}