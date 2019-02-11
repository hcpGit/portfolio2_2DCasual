using UnityEngine;
using UnityEditor;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
/// 
public partial class GoogleDataAssetUtility
{
    [MenuItem("Assets/Create/Google/UIGeneralTexts")]
    public static void CreateUIGeneralTextsAssetFile()
    {
        UIGeneralTexts asset = CustomAssetUtility.CreateAsset<UIGeneralTexts>();
        asset.SheetName = "MPQGame";
        asset.WorksheetName = "UIGeneralTexts";
        EditorUtility.SetDirty(asset);        
    }
    
}