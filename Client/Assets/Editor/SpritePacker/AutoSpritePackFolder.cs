using System.Collections;
using UnityEditor;
using System.IO;
using UnityEngine;

public class Post : AssetPostprocessor
{
    protected const string UI_Path = "Assets/Res/Image/UI";

    void OnPostprocessTexture(Texture2D texture)
    {
        if (Path.GetDirectoryName(assetPath).IndexOf(UI_Path) != -1)            // ����� UI Ŀ¼���ͽ�����Դ������
        {
            string AtlasName = new DirectoryInfo(Path.GetDirectoryName(assetPath)).Name;
            TextureImporter textureImporter = assetImporter as TextureImporter;
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.spritePackingTag = AtlasName;
            textureImporter.mipmapEnabled = false;
        }
    }
}