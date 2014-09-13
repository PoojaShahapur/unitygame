using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace SDK.Lib
{
    public class SceneParse
    {
        protected SceneCfg m_sceneCfg;

        public SceneCfg sceneCfg
        {
            get
            {
                return m_sceneCfg;
            }
            set
            {
                m_sceneCfg = value;
            }
        }

        public void parse(Stream file)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(file);
            XmlNodeList xnl = xmlDoc.SelectSingleNode("config").ChildNodes;
            SceneNodeCfg node;
            foreach (XmlNode xn in xnl)
            {
                XmlElement xe = (XmlElement)xn;

                if (xe.Name == "Terrain")
                {
                    m_sceneCfg.terrainCfg.parse(xe);
                }
                else
                {
                    node = new SceneNodeCfg();
                    node.parse(xe);
                    m_sceneCfg.addSceneNode(node);
                }
            }
        }
    }
}
