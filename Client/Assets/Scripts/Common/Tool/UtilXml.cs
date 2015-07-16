using System.Collections;
using System.Security;

namespace SDK.Common
{
    public class UtilXml
    {
        static public bool getXmlAttrBool(SecurityElement attr, string name)
        {
            if (attr != null)
            {
                if (UtilApi.TRUE == attr.Attribute(name))
                {
                    return true;
                }
                else if (UtilApi.FALSE == attr.Attribute(name))
                {
                    return false;
                }
            }

            return false;
        }

        static public string getXmlAttrStr(SecurityElement attr, string name)
        {
            if (attr != null)
            {
                return attr.Attribute(name);
            }

            return "";
        }

        static public uint getXmlAttrUInt(SecurityElement attr, string name)
        {
            uint ret = 0;
            if (attr != null)
            {
                uint.TryParse(attr.Attribute(name), out ret);
            }

            return ret;
        }

        static public int getXmlAttrInt(SecurityElement attr, string name)
        {
            int ret = 0;
            if (attr != null)
            {
                int.TryParse(attr.Attribute(name), out ret);
            }

            return ret;
        }

        // 获取一个孩子节点列表
        static public void getXmlChildList(SecurityElement elem, string name, ref ArrayList list)
        {
            foreach (SecurityElement child in elem.Children)
            {
                //比对下是否使自己所需要得节点
                if (child.Tag == name)
                {
                    list.Add(child);
                }
            }
        }

        // 获取一个孩子节点
        static public void getXmlChild(SecurityElement elem, string name, ref SecurityElement childNode)
        {
            foreach (SecurityElement child in elem.Children)
            {
                //比对下是否使自己所需要得节点
                if (child.Tag == name)
                {
                    childNode = child;
                    break;
                }
            }
        }
    }
}