using SDK.Lib;
using UnityEditor;
using UnityEngine;

namespace EditorTool
{
    /**
     * @brief 一项重定向 XML
     */
    public class SpriteSettingPathItemXml : XmlItemBase
    {
        public bool mIsDir; // 当前是否是目录，如果不是目录就是文件
        public string mInPath;      // 如果是目录，就是目录，如果不是目录，就是文件
        public bool mIsRecurse;
        public MList<string> mIncludeExt;

        public bool mIsModify;  // 是否修改了资源

        public SpriteSettingPathItemXml()
        {
            this.mIsDir = true;
            this.mInPath = "";
            this.mIsRecurse = true;
            this.mIncludeExt = new MList<string>();
        }

        public override void parseXml(System.Security.SecurityElement xmlelem)
        {
            UtilXml.getXmlAttrStr(xmlelem, "inpath", ref this.mInPath);
            UtilXml.getXmlAttrBool(xmlelem, "isDir", ref this.mIsDir);
            UtilXml.getXmlAttrBool(xmlelem, "recurse", ref this.mIsRecurse);

            string str = "";
            UtilXml.getXmlAttrStr(xmlelem, "includeext", ref str);

            string[] array = UtilStr.split(ref str, ',');

            int idx = 0;
            int len = array.Length;

            while (idx < len)
            {
                this.mIncludeExt.Add(array[idx]);

                ++idx;
            }
        }

        public void spriteSetting()
        {
            string assetPath = "";
            GameObject _go = null;

            if (this.mIsDir)
            {
                assetPath = UtilEditor.convAssetPath2FullPath(this.mInPath);
                UtilPath.traverseDirectory(assetPath, "", null, this.onFileHandle, this.mIsRecurse);
            }
            else
            {
                assetPath = UtilEditor.conRelPath2AssetPath(this.mInPath);
                _go = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

                if (null != _go)
                {
                    this.mIsModify = false;

                    if (this.mIsModify)
                    {
                        // 通知编辑器有资源被修改了
                        EditorUtility.SetDirty(_go);
                        AssetDatabase.SaveAssets();
                        // 卸载资源：注意，卸载方法是在Resources类中
                        //Resources.UnloadAsset(_go);
                        //UnityEngine.Object.Destroy(_go);
                        //UnityEngine.Object.DestroyImmediate(_go);
                    }
                }
            }
        }

        protected void onFileHandle(string srcFullPath, string fileName, string destFullPath)
        {
            string extName = UtilPath.getFileExt(srcFullPath);

            if (this.mIncludeExt.Contains(extName))
            {
                string AtlasName = UtilPath.getFileParentDirName(srcFullPath);
                string assetPath = UtilEditor.convAbsPath2AssetPath(srcFullPath);

                Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
                TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;

                textureImporter.textureType = TextureImporterType.Sprite;
                textureImporter.spriteImportMode = SpriteImportMode.Single;   // 如果一个纹理上有多个精灵，必须设置这个为 SpriteImportMode.Multiple
                textureImporter.spritePackingTag = AtlasName;
                textureImporter.mipmapEnabled = false;
                textureImporter.isReadable = false;
                textureImporter.mipmapEnabled = false;

                TextureImporterSettings textureSettings = new TextureImporterSettings();
                textureImporter.ReadTextureSettings(textureSettings);

                textureSettings.alphaSource = TextureImporterAlphaSource.FromInput;

                textureImporter.SetTextureSettings(textureSettings);

#if UNITY_ANDROID
                TextureImporterPlatformSettings textureImporterPlatformSettings = null;
                textureImporterPlatformSettings = textureImporter.GetPlatformTextureSettings("Android");

                textureImporterPlatformSettings.overridden = true;
                textureImporterPlatformSettings.allowsAlphaSplitting = true;
                textureImporterPlatformSettings.maxTextureSize = 1024;
                textureImporterPlatformSettings.format = TextureImporterFormat.ETC_RGB4;

                textureImporter.SetPlatformTextureSettings(textureImporterPlatformSettings);
#else
                // iPhone 
#endif
                AssetDatabase.ImportAsset(assetPath);
            }
        }
    }

    /**
     * @brief 一个 xml 配置文件
     */
    public class SpriteSettingInfo : XmlCfgBase
    {
        protected MList<XmlItemBase> mPathItemList;

        public SpriteSettingInfo()
        {

        }

        public void parseXmlByPath(string path)
        {
            MFileStream fileStream = new MFileStream(path);
            this.parseXml(fileStream.readText());
            fileStream.dispose();
            fileStream = null;
        }

        override public void parseXml(string str)
        {
            base.parseXml(str);

            System.Security.SecurityElement elemXml = null;
            UtilXml.getXmlChild(this.mXmlConfig, "path", ref elemXml);
            this.mPathItemList = this.parseXml<SpriteSettingPathItemXml>(elemXml, "item");

            UtilXml.getXmlChild(this.mXmlConfig, "redirect", ref elemXml);
        }

        public void spriteSetting()
        {
            if (null != this.mPathItemList)
            {
                int idx = 0;
                int len = this.mPathItemList.Count();
                SpriteSettingPathItemXml itemXml = null;

                while (idx < len)
                {
                    itemXml = this.mPathItemList[idx] as SpriteSettingPathItemXml;
                    itemXml.spriteSetting();

                    ++idx;
                }
            }
        }
    }
}