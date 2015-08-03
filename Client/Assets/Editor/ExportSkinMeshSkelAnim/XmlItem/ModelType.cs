using System.Collections.Generic;
using System.Xml;

namespace EditorTool
{
    /**
     * @brief 模型类型常量。分为男 0 - 鲜肉、1 - 猛男、2 - 女、3 - 萝莉女、4 - npc、5 - 怪物、6 - 坐骑、7 - 武器
     */
    public enum eModelType
    {
        eMT_XianRou,        // 鲜肉
        eMT_MengNan,        // 猛男
        eMT_Woman,          // 女主角
        eMT_LuoLi,          // 萝莉女
        eMT_NPC,            // NPC
        eMT_Master,         // 怪物
        eMT_Mount,          // 坐骑
        eMT_Equip,          // 武器
    }

    /**
     * @brief 所有的类型
     */
    public class ModelTypes
    {
        protected Dictionary<eModelType, ModelType> m_modelTypeDic;

        public ModelTypes()
        {
            m_modelTypeDic = new Dictionary<eModelType,ModelType>();
        }

        public Dictionary<eModelType, ModelType> modelTypeDic
        {
            get
            {
                return m_modelTypeDic;
            }
        }

        public void parseXml(XmlElement elem)
        {
            XmlNodeList itemNodeList = elem.ChildNodes;
            XmlElement itemElem;
            ModelType item;

            foreach (XmlNode itemNode in itemNodeList)
            {
                itemElem = (XmlElement)itemNode;
                item = new ModelType();

                item.parseXml(itemElem);
                m_modelTypeDic[item.id] = item;
            }
        }
    }

    /**
     * @brief 分为男 0 - 鲜肉、1 - 猛男、2 - 女、3 - 萝莉女、4 - npc、5 - 怪物、6 - 坐骑、7 - 武器
     */
    public class ModelType
    {
        protected eModelType m_id;
        protected string m_subPath;

        public eModelType id
        {
            get
            {
                return m_id;
            }
        }

        public string subPath
        {
            get
            {
                return m_subPath;
            }
        }

        public void parseXml(XmlElement elem)
        {
            m_id = (eModelType)ExportUtil.getXmlAttrInt(elem.Attributes["id"]);
            m_subPath = ExportUtil.getXmlAttrStr(elem.Attributes["subpath"]);
        }
    }
}