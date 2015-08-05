using System.Collections.Generic;
using System.Xml;

namespace EditorTool
{
    public class XmlLayers
    {
        protected List<XmlLayer> m_layerList = new List<XmlLayer>();

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

        public void parseXml(XmlElement elem)
        {
            XmlNodeList layersNodeList = elem.SelectNodes("Layer");
            XmlElement layerElem = null;
            XmlLayer layer;
            foreach (XmlNode layerNode in layersNodeList)
            {
                layerElem = (XmlElement)layerNode;
                layer = new XmlLayer();
                m_layerList.Add(layer);
                layer.parseXml(layerElem);
            }
        }
    }
}