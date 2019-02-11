using UnityEngine;
using UnityEditor;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
/// 
public partial class GoogleDataAssetUtility
{
    [MenuItem("Assets/Create/Google/Merchant_Sell")]
    public static void CreateMerchant_SellAssetFile()
    {
        Merchant_Sell asset = CustomAssetUtility.CreateAsset<Merchant_Sell>();
        asset.SheetName = "MPQGame";
        asset.WorksheetName = "Merchant_Sell";
        EditorUtility.SetDirty(asset);        
    }
    
}