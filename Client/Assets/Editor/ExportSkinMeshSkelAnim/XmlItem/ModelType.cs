using SDK.Lib;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace EditorTool
{
    /**
     * @brief 导出文件的类型
     */
    public enum eExportFileType
    {
        eOneMeshOneFile,        // 一个模型一个文件
        eOneSubMeshOneFile,     // 一个子模型一个文件
        eOneTypeOneFile,        // 一种类型的模型一个文件
    }

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

        public void clear()
        {
            m_modelTypeDic.Clear();
        }

        public void createDir(string parentPath)
        {
            foreach(eModelType key in m_modelTypeDic.Keys)
            {
                m_modelTypeDic[key].createDir(parentPath);
            }
        }

        public void addXmlHeader()
        {
            foreach (eModelType key in m_modelTypeDic.Keys)
            {
                m_modelTypeDic[key].addXmlHeader();
            }
        }

        public void addXmlEnd()
        {
            foreach (eModelType key in m_modelTypeDic.Keys)
            {
                m_modelTypeDic[key].addXmlEnd();
            }
        }

        public void save2Files(string parentPath)
        {
            foreach (eModelType key in m_modelTypeDic.Keys)
            {
                m_modelTypeDic[key].save2Files(parentPath);
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
        protected string m_outFileName;     // 输出文件名字
        public string m_content;         // 输出内容

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

        public void createDir(string parentPath)
        {
            string path = "";
            //path = ExportUtil.getDataPath(string.Format("{0}/{1}", parentPath, m_subPath));
            path = ExportUtil.getDataPath(m_subPath);
            if(!UtilPath.ExistDirectory(path))
            {
                UtilPath.recurseCreateDirectory(path);
            }
        }

        public void addXmlHeader()
        {
            m_content = "<?xml version='1.0' encoding='utf-8' ?>\n<Root>\n";
            m_content += string.Format("    <Mesh name=\"{0}\" >\n", m_outFileName);
        }

        public void addXmlEnd()
        {
            m_content += "    </Mesh>\n";
            m_content += "</Root>";
        }

        public void save2Files(string parentPath)
        {
            string path = "";
            //path = ExportUtil.getDataPath(string.Format("{0}/{1}/{2}.xml", parentPath, m_subPath, m_outFileName));
            path = ExportUtil.getDataPath(string.Format("{0}/{1}.xml", parentPath, m_outFileName));

            UtilPath.deleteFile(path);
            FileStream fileStream = new FileStream(path, FileMode.CreateNew);
            byte[] data = new UTF8Encoding().GetBytes(m_content);
            //开始写入
            fileStream.Write(data, 0, data.Length);

            //清空缓冲区、关闭流
            fileStream.Flush();
            fileStream.Close();
            fileStream.Dispose();

            m_content = "";
        }

        public void parseXml(XmlElement elem)
        {
            m_id = (eModelType)ExportUtil.getXmlAttrInt(elem.Attributes["id"]);
            m_subPath = ExportUtil.getXmlAttrStr(elem.Attributes["subpath"]);
            m_outFileName = ExportUtil.getXmlAttrStr(elem.Attributes["outfilename"]);
        }
    }
}