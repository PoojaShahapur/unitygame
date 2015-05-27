using System.Collections;
using UnityEditor;
using System.IO;
using UnityEngine;

public class Post : AssetPostprocessor
{
    protected const string UI_Path = "Assets/Res/Image/UI";
    protected const string SpriteAni_Path = "Assets/Res/Image/Effect/SpriteEffect";
    //protected const string UI_Dyn_Suffix = "Dyn";

    void OnPostprocessTexture(Texture2D texture)
    {
        if (Path.GetDirectoryName(assetPath).IndexOf(UI_Path) != -1 ||
            Path.GetDirectoryName(assetPath).IndexOf(SpriteAni_Path) != -1)            // 如果是 UI 目录，就进行资源的设置
        {
            // if (assetPath.Substring(assetPath.Length - UI_Dyn_Suffix.Length) != UI_Dyn_Suffix)
            //{
                string AtlasName = new DirectoryInfo(Path.GetDirectoryName(assetPath)).Name;
                TextureImporter textureImporter = assetImporter as TextureImporter;
                textureImporter.textureType = TextureImporterType.Sprite;
                textureImporter.spritePackingTag = AtlasName;
                textureImporter.mipmapEnabled = false;
            //}
        }
    }
}