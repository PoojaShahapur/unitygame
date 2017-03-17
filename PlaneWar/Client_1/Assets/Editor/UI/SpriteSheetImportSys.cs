using SDK.Lib;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EditorTool
{
    /**
     * @brief SpriteSheetImportSys 精灵单导入系统
     */
    public class SpriteSheetImportSys : Singleton<SpriteSheetImportSys>, IMyDispose
    {
        protected SpriteSheetInfo mSpriteSheetInfo;
        protected bool MIsFlipX;

        public SpriteSheetImportSys()
        {
            this.MIsFlipX = true;
        }

        public void init()
        {

        }

        public void dispose()
        {

        }

        public bool getIsFlipX()
        {
            return this.MIsFlipX;
        }

        public void parseSpriteSheet(string path)
        {
            this.mSpriteSheetInfo = new SpriteSheetInfo();
            this.mSpriteSheetInfo.parseXmlByPath(path);
        }

        public bool isSpriteSheetPath(string path)
        {
            return true;
        }

        public List<UnityEditor.SpriteMetaData> getSpriteMetaList()
        {
            return this.mSpriteSheetInfo.getSpriteMetaList();
        }

        public void importSpriteSheet()
        {
            string path = "Editor/Config/ImportSpriteSheet.xml";
            path = UtilEditor.convAssetPath2FullPath(path);

            SpriteSheetConfigInfo spriteSheetConfigInfo = new SpriteSheetConfigInfo();
            spriteSheetConfigInfo.parseXmlByPath(path);
            spriteSheetConfigInfo.importSpriteSheet();

            //AssetDatabase.SaveAssets();
            //AssetDatabase.Refresh();
        }

        public void importAllSpriteSheet()
        {
            string rootPath = "F:/File/opensource/unity-game-git/unitygame/unitygame/SnowRoll/Assets/Resources/UiImage/TestAtlas";

            UtilPath.traverseDirectory(rootPath, "", null, this.onFileHandle);
        }

        protected void onFileHandle(string srcFullPath, string fileName, string destFullPath)
        {
            string extName = UtilPath.getFileExt(srcFullPath);
            string xmlPath = string.Format("{0}.xml", UtilPath.getFilePathNoExt(srcFullPath));

            if ("png" == extName && UtilPath.existFile(xmlPath))
            {
                string AtlasName = UtilPath.getFileParentDirName(srcFullPath);
                string assetPath = UtilEditor.convAbsPath2AssetPath(srcFullPath);

                this.mSpriteSheetInfo = new SpriteSheetInfo();
                this.mSpriteSheetInfo.parseXmlByPath(xmlPath);

                Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
                TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;

                textureImporter.textureType = TextureImporterType.Sprite;
                textureImporter.spriteImportMode = SpriteImportMode.Multiple;   // 如果一个纹理上有多个精灵，必须设置这个为 SpriteImportMode.Multiple
                textureImporter.spritePackingTag = AtlasName;
                textureImporter.mipmapEnabled = false;

                this.parseSpriteSheet(xmlPath);
                List<SpriteMetaData> sprites = this.getSpriteMetaList();

                textureImporter.spritesheet = sprites.ToArray();

                TextureImporterSettings textureSettings = new TextureImporterSettings();
                textureImporter.ReadTextureSettings(textureSettings);
                textureImporter.SetTextureSettings(textureSettings);
                AssetDatabase.ImportAsset(assetPath);
            }
        }
    }
}