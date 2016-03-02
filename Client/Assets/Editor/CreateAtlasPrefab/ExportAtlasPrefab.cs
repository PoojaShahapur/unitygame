using UnityEditor;

namespace AtlasPrefabSys
{
    public class AtlasPrefabExport
    {
        [MenuItem("My/Assets/AtlasPrefab/ExportAtlasPrefab")]
        static public void ExportAtlasPrefab()
        {
            //spriteGoPack();
            spriteScriptPack();
            //singleSpriteScriptPack();
        }

        public static void spriteGoPack()
        {
            CreateAtlasPrefabSys.instance();
            CreateAtlasPrefabSys.m_instance.clear();
            //AtlasPrefabUtil.createPrefab("aaaa");
            CreateAtlasPrefabSys.m_instance.parseXml();
            CreateAtlasPrefabSys.m_instance.exportPrefab();
        }

        public static void spriteScriptPack()
        {
            CreateAtlasPrefabSys.instance();
            CreateAtlasPrefabSys.m_instance.clear();
            CreateAtlasPrefabSys.m_instance.parseXml();
            CreateAtlasPrefabSys.m_instance.exportAsset();
        }

        public static void singleSpriteScriptPack()
        {
            EditorSOSprite packSprite = new EditorSOSprite();
            // 不管扩展名字是 asset 还是 prefab ，结果 Unity 编辑器中显示的图标都是一样的
            packSprite.packSprite("Assets/Res/Image/UI/Common/denglu_srk.png", "Assets/Resources/Atlas/aaa.asset");
            //packSprite.packSprite("Assets/Res/Image/UI/Common/denglu_srk.png", "Assets/Resources/Atlas/aaa.prefab");
        }
    }
}