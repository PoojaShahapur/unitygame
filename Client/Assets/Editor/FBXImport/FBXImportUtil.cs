using UnityEditor;
using UnityEngine;

namespace NSFBXImport
{
    /**
     * @brief 导入 FBX 文件
     */
    public class FBXImportUtil
    {
        /**
         * @brief 直接将文件拷贝到当前文件夹下就行了
         */
        static public void importFBX()
        {
            string inPath = "E:/aaaaa/bujian_zhanchang.FBX";
            string outPath = "D:/file/opensource/unity-game-git/unitygame/unitygame/Client/Assets/Res";
            AssetDatabase.MoveAsset(inPath, outPath);
        }

        static public void createPrefab()
        {
            string inPath = "Assets/Res/Model/Scene/bujian_zhanchang.FBX";
            string outPath = "Assets/Resources/Model/aaa.prefab";

            GameObject _go = AssetDatabase.LoadAssetAtPath(inPath, typeof(GameObject)) as GameObject;
            PrefabUtility.CreatePrefab(outPath, _go, ReplacePrefabOptions.ConnectToPrefab);
            AssetDatabase.Refresh();
        }
    }
}