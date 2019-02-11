using UnityEngine;
using UnityEditor;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
/// 
public partial class GoogleDataAssetUtility
{
    [MenuItem("Assets/Create/Google/Merchant_Upgrade")]
    public static void CreateMerchant_UpgradeAssetFile()
    {
        Merchant_Upgrade asset = CustomAssetUtility.CreateAsset<Merchant_Upgrade>();
        asset.SheetName = "MPQGame";
        asset.WorksheetName = "Merchant_Upgrade";
        EditorUtility.SetDirty(asset);        
    }
    
}