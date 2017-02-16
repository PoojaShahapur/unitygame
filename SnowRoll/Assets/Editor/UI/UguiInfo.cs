using SDK.Lib;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace EditorTool
{
    /**
     * @brief UGUI 预制 Item
     */
    public class UguiPrefaItembXml : XmlItemBase
    {
        public bool mIsDir; // 当前是否是目录，如果不是目录就是文件
        public string mInPath;      // 如果是目录，就是目录，如果不是目录，就是文件
        public bool mIsRecurse;
        public MList<string> mIncludeExt;

        public bool mIsModify;  // 是否修改了资源

        public UguiPrefaItembXml()
        {
            this.mIsDir = true;
            this.mInPath = "";
            this.mIsRecurse = true;
            this.mIncludeExt = new MList<string>();

            this.mIsModify = false;
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

            while(idx < len)
            {
                this.mIncludeExt.Add(array[idx]);

                ++idx;
            }
        }

        public void changeMat()
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

                if(null != _go)
                {
                    this.mIsModify = false;

                    UtilApi.traverseActor<Image>(_go, onGoHandle);

                    if (this.mIsModify)
                    {
                        EditorUtility.SetDirty(_go);
                        AssetDatabase.SaveAssets();
                    }
                }
            }
        }

        protected void onFileHandle(string srcFullPath, string fileName, string destFullPath)
        {
            string extName = UtilPath.getFileExt(srcFullPath);

            if (this.mIncludeExt.Contains(extName))
            {
                string assetPath = UtilEditor.convAbsPath2AssetPath(srcFullPath);
                GameObject _go = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

                if (null != _go)
                {
                    this.mIsModify = false;

                    UtilApi.traverseActor<Image>(_go, onGoHandle);

                    if (this.mIsModify)
                    {
                        EditorUtility.SetDirty(_go);
                        AssetDatabase.SaveAssets();
                    }
                }
            }
        }

        protected void onGoHandle(GameObject go_, Image image)
        {
            if (image.sprite)
            {
                Texture2D texture = image.sprite.texture;

                if (texture)
                {
                    string assetPath = AssetDatabase.GetAssetPath(texture);

                    if(UtilPath.isFileNameSuffixNoExt(assetPath, UtilApi.RGB_IMAGE_SUFFIX))
                    {
                        string matPath = string.Format("{0}/{1}{2}{3}", ChangeUIMatSys.MatPath, UtilPath.getFileNameRemoveSuffixNoExt(assetPath, UtilApi.RGB_IMAGE_SUFFIX), UtilPath.DOT, UtilApi.MAT);

                        matPath = UtilEditor.conRelPath2AssetPath(matPath);

                        Material mat = AssetDatabase.LoadAssetAtPath<Material>(matPath);
                        if (mat)
                        {
                            this.mIsModify = true;

                            image.material = mat;
                        }
                    }
                }
            }
        }
    }

    /**
     * @brief 一个 xml 配置文件
     */
    public class UguiInfo : XmlCfgBase
    {
        protected MList<XmlItemBase> mItemList;

        public UguiInfo()
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

            this.mItemList = this.parseXml<UguiPrefaItembXml>(null, "path");
        }

        public void changeMat()
        {
            if (null != this.mItemList)
            {
                int idx = 0;
                int len = this.mItemList.Count();
                UguiPrefaItembXml itemXml = null;

                while (idx < len)
                {
                    itemXml = this.mItemList[idx] as UguiPrefaItembXml;
                    itemXml.changeMat();

                    ++idx;
                }
            }
        }
    }
}