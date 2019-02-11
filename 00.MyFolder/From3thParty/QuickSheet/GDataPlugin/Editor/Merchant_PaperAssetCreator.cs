using UnityEngine;
using UnityEditor;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
/// 
public partial class GoogleDataAssetUtility
{
    [MenuItem("Assets/Create/Google/Merchant_Paper")]
    public static void CreateMerchant_PaperAssetFile()
    {
        Merchant_Paper asset = CustomAssetUtility.CreateAsset<Merchant_Paper>();
        asset.SheetName = "MPQGame";
        asset.WorksheetName = "Merchant_Paper";
        EditorUtility.SetDirty(asset);        
    }
    
}