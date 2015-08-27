using EditorTool;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using SDK.Lib;

namespace CleanResourceSys
{
    public class CleanResource
    {
        static public AtlasMgr m_atlasMgr = new AtlasMgr();
        [MenuItem("Assets/CleanResouce")]
        static public void CleanResouce()
        {
            GameObject go = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Resources/UI/UIShop/UIShop.prefab", ExportUtil.convResStr2Type("prefab")) as GameObject;
  
            if(go != null)
            {
                UtilApi.CleanTex(go);
            }
        }
    }
}
