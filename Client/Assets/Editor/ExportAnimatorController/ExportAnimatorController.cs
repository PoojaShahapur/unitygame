using UnityEditor;

namespace EditorTool
{
    public class AnimatorControllerExport
    {
        [MenuItem("Assets/ExportSkinMeshSkel/ExportAnimatorController")]
        static public void ExportAnimatorController()
        {
            export();
        }

        public static void export()
        {
            ExportAnimatorControllerSys.instance();
            ExportAnimatorControllerSys.m_instance.clear();
            ExportAnimatorControllerSys.m_instance.parseXml();
            ExportAnimatorControllerSys.m_instance.exportControllerAsset();
        }
    }
}