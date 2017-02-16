using SDK.Lib;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EditorTool
{
    /**
     * @brief 一项精灵单
     */
    public class SpriteSheetItemXml : XmlItemBase
    {
        public string mName;
        public int mX;
        public int mY;
        public int mW;
        public int mH;
        public float mPX;
        public float mPY;

        public override void parseXml(System.Security.SecurityElement xmlelem)
        {
            UtilXml.getXmlAttrStr(xmlelem, "n", ref this.mName);
            UtilXml.getXmlAttrInt(xmlelem, "x", ref this.mX);
            UtilXml.getXmlAttrInt(xmlelem, "y", ref this.mY);
            UtilXml.getXmlAttrInt(xmlelem, "w", ref this.mW);
            UtilXml.getXmlAttrInt(xmlelem, "h", ref this.mH);
            UtilXml.getXmlAttrFloat(xmlelem, "pX", ref this.mPX);
            UtilXml.getXmlAttrFloat(xmlelem, "pY", ref this.mPY);

            this.mName = UtilPath.getFileNameNoExt(this.mName);
        }

        public UnityEditor.SpriteMetaData toMetaData(SpriteSheetInfo info)
        {
            UnityEditor.SpriteMetaData data = new UnityEditor.SpriteMetaData();

            data.alignment = 0;
            data.border.x = 0;
            data.border.y = 0;
            data.border.z = 0;
            data.border.w = 0;

            data.name = this.mName;
            data.pivot.x = 0.5f;
            data.pivot.y = 0.5f;

            // 翻转 X ，TexturePacker 打包出来的图集， X 轴与 Unity 相反
            data.rect.x = this.mX;
            //data.rect.y = mY;
            data.rect.y = info.mHeight - this.mY - this.mH;
            data.rect.width = this.mW;
            data.rect.height = this.mH;

            return data;
        }
    }

    /**
     * @brief 一个 xml 配置文件
     */
    public class SpriteSheetInfo : XmlCfgBase
    {
        public string mImagePath;
        public int mWidth;
        public int mHeight;

        protected MList<XmlItemBase> mItemList;

        public SpriteSheetInfo()
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

            UtilXml.getXmlAttrStr(this.mXmlConfig, "imagePath", ref mImagePath);
            UtilXml.getXmlAttrInt(this.mXmlConfig, "imagePath", ref mWidth);
            UtilXml.getXmlAttrInt(this.mXmlConfig, "height", ref mHeight);

            this.mItemList = this.parseXml<SpriteSheetItemXml>(null, "sprite");
        }

        public List<UnityEditor.SpriteMetaData> getSpriteMetaList()
        {
            List<UnityEditor.SpriteMetaData> list = new List<UnityEditor.SpriteMetaData>();

            UnityEditor.SpriteMetaData data;

            int idx = 0;
            int len = this.mItemList.Count();

            while(idx < len)
            {
                data = (this.mItemList[idx] as SpriteSheetItemXml).toMetaData(this);
                list.Add(data);

                ++idx;
            }

            return list;
        }
    }

    //------------------------ Sprite Sheet 配置文件信息 --------------------
    /**
     * @brief SpriteSheetCfgItemXml 目录配置中一项 Item
     */
    public class SpriteSheetCfgItemXml : XmlItemBase
    {
        public bool mIsDir; // 当前是否是目录，如果不是目录就是文件
        public string mInPath;      // 如果是目录，就是目录，如果不是目录，就是文件
        public bool mIsRecurse;
        public MList<string> mIncludeExt;

        public SpriteSheetCfgItemXml()
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

        protected void onFileHandle(string srcFullPath, string fileName, string destFullPath)
        {
            string extName = UtilPath.getFileExt(srcFullPath);
            string xmlPath = string.Format("{0}.xml", UtilPath.getFilePathNoExt(srcFullPath));

            if (this.mIncludeExt.Contains(extName) && UtilPath.existFile(xmlPath))
            {
                string AtlasName = UtilPath.getFileParentDirName(srcFullPath);
                string assetPath = UtilEditor.convAbsPath2AssetPath(srcFullPath);

                SpriteSheetInfo spriteSheetInfo = new SpriteSheetInfo();
                spriteSheetInfo.parseXmlByPath(xmlPath);

                Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
                TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;

                textureImporter.textureType = TextureImporterType.Sprite;
                textureImporter.spriteImportMode = SpriteImportMode.Multiple;   // 如果一个纹理上有多个精灵，必须设置这个为 SpriteImportMode.Multiple
                textureImporter.spritePackingTag = AtlasName;
                textureImporter.mipmapEnabled = false;

                List<SpriteMetaData> sprites = spriteSheetInfo.getSpriteMetaList();

                textureImporter.spritesheet = sprites.ToArray();

                TextureImporterSettings textureSettings = new TextureImporterSettings();
                textureImporter.ReadTextureSettings(textureSettings);
                textureImporter.SetTextureSettings(textureSettings);
                AssetDatabase.ImportAsset(assetPath);
            }
        }

        public void importSpriteSheet()
        {
            string path = "";

            if (this.mIsDir)
            {
                path = UtilEditor.convAssetPath2FullPath(this.mInPath);
                UtilPath.traverseDirectory(path, "", null, this.onFileHandle, this.mIsRecurse);
            }
            else
            {
                path = UtilEditor.convAssetPath2FullPath(this.mInPath);
                this.onFileHandle(path, "", "");
            }
        }
    }

    /**
     * @brief 一个 xml 配置文件
     */
    public class SpriteSheetConfigInfo : XmlCfgBase
    {
        protected MList<XmlItemBase> mItemList;

        public SpriteSheetConfigInfo()
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

            this.mItemList = this.parseXml<SpriteSheetCfgItemXml>(null, "path");
        }

        public void importSpriteSheet()
        {
            if (null != this.mItemList)
            {
                int idx = 0;
                int len = this.mItemList.Count();
                SpriteSheetCfgItemXml itemXml = null;

                while (idx < len)
                {
                    itemXml = this.mItemList[idx] as SpriteSheetCfgItemXml;
                    itemXml.importSpriteSheet();

                    ++idx;
                }
            }
        }
    }
}