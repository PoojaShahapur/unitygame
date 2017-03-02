using SDK.Lib;
using System.Collections;
using System.Security;

namespace EditorTool
{
    /**
     * @breif 编译信息
     */
    public class BuildInfo : XmlCfgBase
    {
        protected MList<BuildDeleteItem> mDeleteList;    // 删除的文件或者目录列表

        virtual public void parseXmlByPath(string path)
        {

        }

        override public void parseXml(string str)
        {
            base.parseXml(str);

            ArrayList list = new ArrayList();
            UtilXml.getXmlChildListByPath(this.mXmlConfig, "DeletePath.item", ref list);

            int idx = 0;
            int len = list.Count;

            SecurityElement elem = null;
            BuildDeleteItem delItem = null;

            while (idx < len)
            {
                elem = list[idx] as SecurityElement;
                delItem = new BuildDeleteItem();
                this.mDeleteList.Add(delItem);

                UtilXml.getXmlAttrStr(elem, "inpath", ref delItem.mPath);
                UtilXml.getXmlAttrBool(elem, "isDir", ref delItem.mIsDir);

                ++idx;
            }
        }
    }
}