using System.Xml;
using UnityEditor.Animations;

namespace EditorTool
{
    public class XmlAnimatorController
    {
        protected string m_inPath;
        protected string m_outPath;
        protected string m_outName;
        protected string m_outExtName;

        protected XmlParams m_params = new XmlParams();
        protected XmlLayers m_layers = new XmlLayers();

        protected string m_controllerFullPath;      // 中间生成的控制器的目录
        protected string m_assetFullPath;
        protected AnimatorController m_animatorController;

        public XmlAnimatorController()
        {
            m_params.xmlAnimatorController = this;
            m_layers.xmlAnimatorController = this;
        }

        public string inPath
        {
            get
            {
                return m_inPath;
            }
            set
            {
                m_inPath = value;
            }
        }

        public string outPath
        {
            get
            {
                return m_outPath;
            }
            set
            {
                m_outPath = value;
            }
        }

        public string outName
        {
            get
            {
                return m_outName;
            }
            set
            {
                m_outName = value;
            }
        }

        public string outExtName
        {
            get
            {
                return m_outExtName;
            }
            set
            {
                m_outExtName = value;
            }
        }

        public XmlParams getParams
        {
            get
            {
                return m_params;
            }
            set
            {
                m_params = value;
            }
        }

        public XmlLayers layers
        {
            get
            {
                return m_layers;
            }
            set
            {
                m_layers = value;
            }
        }

        public string controllerFullPath
        {
            get
            {
                return m_controllerFullPath;
            }
            set
            {
                m_controllerFullPath = value;
            }
        }

        public string assetFullPath
        {
            get
            {
                return m_assetFullPath;
            }
            set
            {
                m_assetFullPath = value;
            }
        }

        public AnimatorController animatorController
        {
            get
            {
                return m_animatorController;
            }
            set
            {
                m_animatorController = value;
            }
        }

        public void parseXml(XmlElement elem)
        {
            m_inPath = ExportUtil.getXmlAttrStr(elem.Attributes["inpath"]);
            m_outPath = ExportUtil.getXmlAttrStr(elem.Attributes["outpath"]);
            m_outName = ExportUtil.getXmlAttrStr(elem.Attributes["outname"]);
            m_outExtName = ExportUtil.getXmlAttrStr(elem.Attributes["outextname"]);

            m_controllerFullPath = string.Format("{0}/{1}.controller", m_inPath, m_outName);
            m_assetFullPath = string.Format("{0}/{1}.{2}", m_outPath, m_outName, m_outExtName);

            XmlNode paramsNode = elem.SelectSingleNode("Params");
            m_params.parseXml(paramsNode as XmlElement);

            XmlNode layersNode = elem.SelectSingleNode("Layers");
            m_layers.parseXml(layersNode as XmlElement);
        }
    }
}