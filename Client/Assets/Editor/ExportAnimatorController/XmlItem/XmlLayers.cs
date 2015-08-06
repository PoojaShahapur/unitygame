using System.Collections.Generic;
using System.Xml;

namespace EditorTool
{
    public class XmlLayers
    {
        protected List<XmlLayer> m_layerList = new List<XmlLayer>();
        protected XmlAnimatorController m_xmlAnimatorController;

        public List<XmlLayer> layerList
        {
            get
            {
                return m_layerList;
            }
            set
            {
                m_layerList = value;
            }
        }

        public XmlAnimatorController xmlAnimatorController
        {
            get
            {
                return m_xmlAnimatorController;
            }
            set
            {
                m_xmlAnimatorController = value;
            }
        }

        public void adjustFileName(string modelName)
        {
            foreach(var lay in m_layerList)
            {
                lay.adjustFileName(modelName);
            }
        }

        public void parseXml(XmlElement elem)
        {
            m_layerList.Clear();

            XmlNodeList layersNodeList = elem.SelectNodes("Layer");
            XmlElement layerElem = null;
            XmlLayer layer;
            foreach (XmlNode layerNode in layersNodeList)
            {
                layerElem = (XmlElement)layerNode;
                layer = new XmlLayer();
                m_layerList.Add(layer);
                layer.xmlLayers = this;
                layer.parseXml(layerElem);
            }
        }
    }
}