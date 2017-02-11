using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

class MyAllPostprocessor : AssetPostprocessor
{
    void OnPostprocessAllAssets(
        string[] importedAssets,
        string[] deletedAssets,
        string[] movedAssets,
        string[] movedFromAssetPaths)
        {
            foreach (var str in importedAssets)
            {
                Debug.Log("Reimported Asset: " + str);
            }
            foreach (var str in deletedAssets)
            {
                Debug.Log("Deleted Asset: " + str);
            }
            for(int i = 0; i < movedAssets.Length; i++)
            {
                Debug.Log("Moved Asset:" + movedAssets[i] + " from: " + movedFromAssetPaths[i]);
            }
        }

    void OnPostprocessTexture(Texture2D texture)
    {
        string AtlasName = new DirectoryInfo(Path.GetDirectoryName(assetPath)).Name;
        TextureImporter textureImporter = assetImporter as TextureImporter;
        textureImporter.textureType = TextureImporterType.Sprite;
        textureImporter.spritePackingTag = AtlasName;
        textureImporter.mipmapEnabled = false;

        //List<SpriteMetaData> sprites = TexturePacker.ProcessToSprites(txt.text);
    }
}