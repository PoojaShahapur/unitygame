using SDK.Lib;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace EditorTool
{
    /**
     * @brief һ���ض��� XML
     */
    public class SpriteRedirectPathItemXml : XmlItemBase
    {
        public bool mIsDir; // ��ǰ�Ƿ���Ŀ¼���������Ŀ¼�����ļ�
        public string mInPath;      // �����Ŀ¼������Ŀ¼���������Ŀ¼�������ļ�
        public bool mIsRecurse;
        public MList<string> mIncludeExt;

        public bool mIsModify;  // �Ƿ��޸�����Դ

        public SpriteRedirectPathItemXml()
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

        public void redirectSprite()
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

                    UtilApi.traverseActor<Image>(_go, onGoHandle);

                    if (this.mIsModify)
                    {
                        // ֪ͨ�༭������Դ���޸���
                        EditorUtility.SetDirty(_go);
                        AssetDatabase.SaveAssets();
                        // ж����Դ��ע�⣬ж�ط�������Resources����
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
                string assetPath = UtilEditor.convAbsPath2AssetPath(srcFullPath);
                GameObject _go = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

                if (null != _go)
                {
                    this.mIsModify = false;

                    UtilApi.traverseActor<Image>(_go, onGoHandle);

                    if (this.mIsModify)
                    {
                        // ֪ͨ�༭������Դ���޸���
                        EditorUtility.SetDirty(_go);
                        AssetDatabase.SaveAssets();
                        // ж����Դ��ע�⣬ж�ط�������Resources����
                        //Resources.UnloadAsset(_go);
                        //UnityEngine.Object.Destroy(_go);
                        //UnityEngine.Object.DestroyImmediate(_go, true);
                    }
                }
            }
        }

        protected void onGoHandle(GameObject go_, Image image)
        {
            // ������鶪ʧ�� Sprite ���ֵ�ǲ��ܻ�ȡ�ģ����һ��Ҫ�ھ��鲻��ʧ������»�ȡ
            if (image.sprite)
            {
                Texture2D texture = image.sprite.texture;

                if (texture)
                {
                    string assetPath = AssetDatabase.GetAssetPath(texture);
                    SpriteRedirectAliasItemXml aliasItem = SpriteRedirectSys.getSingletonPtr().getSpriteAliasItem(assetPath);

                    if (null != aliasItem)
                    {
                        Sprite sprite = UtilEditor.getSpriteByAssetPath(aliasItem.mNewName, aliasItem.mSpriteName);

                        if (null != sprite)
                        {
                            this.mIsModify = true;

                            image.sprite = sprite;
                        }
                    }
                }
            }
        }
    }

    /**
     * @brief һ�� Alias ��
     */
    public class SpriteRedirectAliasItemXml : XmlItemBase
    {
        public string mOldName;
        public string mNewName;
        public string mSpriteName;

        public SpriteRedirectAliasItemXml()
        {

        }

        public override void parseXml(System.Security.SecurityElement xmlelem)
        {
            UtilXml.getXmlAttrStr(xmlelem, "old", ref this.mOldName);
            UtilXml.getXmlAttrStr(xmlelem, "new", ref this.mNewName);
            UtilXml.getXmlAttrStr(xmlelem, "spriteName", ref this.mSpriteName);
        }
    }

    /**
     * @brief һ�� xml �����ļ�
     */
    public class SpriteRedirectInfo : XmlCfgBase
    {
        protected MList<XmlItemBase> mPathItemList;
        protected MList<XmlItemBase> mAliasItemList;

        public SpriteRedirectInfo()
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
            this.mPathItemList = this.parseXml<SpriteRedirectPathItemXml>(elemXml, "item");

            UtilXml.getXmlChild(this.mXmlConfig, "redirect", ref elemXml);
            this.mAliasItemList = this.parseXml<SpriteRedirectAliasItemXml>(elemXml, "item");
        }

        public void redirectSprite()
        {
            if (null != this.mPathItemList)
            {
                int idx = 0;
                int len = this.mPathItemList.Count();
                SpriteRedirectPathItemXml itemXml = null;

                while (idx < len)
                {
                    itemXml = this.mPathItemList[idx] as SpriteRedirectPathItemXml;
                    itemXml.redirectSprite();

                    ++idx;
                }
            }
        }

        public SpriteRedirectAliasItemXml getSpriteAliasItem(string oldName)
        {
            int idx = 0;
            int len = this.mAliasItemList.Count();
            SpriteRedirectAliasItemXml itemXml = null;
            SpriteRedirectAliasItemXml ret = null;

            while (idx < len)
            {
                itemXml = this.mAliasItemList[idx] as SpriteRedirectAliasItemXml;

                if(itemXml.mOldName == oldName)
                {
                    ret = itemXml;
                    break;
                }

                ++idx;
            }

            return ret;
        }
    }
}