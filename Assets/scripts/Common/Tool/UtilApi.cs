using System.Xml;
using UnityEngine;

namespace SDK.Common
{
    /**
     * @brief 对 api 的进一步 wrap 
     */
    public class UtilApi
    {
        public const string TRUE = "true";
        public const string FALSE = "false";

        // 通过父对象和完整的目录查找 child 对象
        static public GameObject TransFindChildByPObjAndPath(GameObject pObject, string path)
        {
            return pObject.transform.Find(path).gameObject;
        }

        // 仅仅根据名字查找 GameObject ，注意如果 GameObject 设置 SetActive 为 false ，就会查找不到，如果有相同名字的 GameObject ，不保证查找到正确的。
        static public GameObject GoFindChildByPObjAndName(string name)
        {
            return GameObject.Find(name);
        }

        static public bool getXmlAttrBool(XmlAttribute attr)
        {
            if (attr != null)
            {
                if (TRUE == attr.Value)
                {
                    return true;
                }
                else if (FALSE == attr.Value)
                {
                    return false;
                }
            }

            return false;
        }

        static public string getXmlAttrStr(XmlAttribute attr)
        {
            if (attr != null)
            {
                return attr.Value;
            }

            return "";
        }
    }
}