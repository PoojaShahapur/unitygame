using System.IO;
using UnityEditor;
using UnityEngine;

namespace EditorTool
{
    public class ExportTer
    {
        [MenuItem("My/Assets/Export Texture")]
        static public void Apply()
        {
            Texture2D texture = Selection.activeObject as Texture2D;
            if (texture == null)
            {
                EditorUtility.DisplayDialog("Select Texture", "You Must Select a Texture first!", "Ok");
                return;
            }

            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + "/exported_texture.png", bytes);
        }
    }
}