using UnityEditor;

namespace EditorTool
{
    public class AnimatorControllerExport
    {
        [MenuItem("Assets/AnimatorControllerCreate/ExportAnimatorController")]
        static public void ExportAnimatorController()
        {
            export();
        }

        public static void export()
        {
            AnimatorControllerCreateSys.instance();
            AnimatorControllerCreateSys.m_instance.clear();
            AnimatorControllerCreateSys.m_instance.parseXml();
            AnimatorControllerCreateSys.m_instance.exportControllerAsset();
        }
    }
}