using UnityEngine;
using UnityEditor;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
/// 
public partial class GoogleDataAssetUtility
{
    [MenuItem("Assets/Create/Google/Merchant_Interior")]
    public static void CreateMerchant_InteriorAssetFile()
    {
        Merchant_Interior asset = CustomAssetUtility.CreateAsset<Merchant_Interior>();
        asset.SheetName = "MPQGame";
        asset.WorksheetName = "Merchant_Interior";
        EditorUtility.SetDirty(asset);        
    }
    
}