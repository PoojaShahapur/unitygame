using UnityEditor;
namespace NSFBXImport
{
    /**
     * @brief 导入 FBX 文件
     */
    public class FBXImportMenu
    {
        [MenuItem("Assets/FBXImport/FBXImport")]
        static public void FBXImport()
        {
            import();
        }

        public static void import()
        {
            FBXImportUtil.createPrefab();
        }
    }
}