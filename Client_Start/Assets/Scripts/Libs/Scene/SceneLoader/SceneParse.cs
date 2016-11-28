using Mono.Xml;
using System.Collections;
using System.Security;

namespace SDK.Lib
{
    public class SceneParse
    {
        protected SceneCfg mSceneCfg;

        public SceneCfg sceneCfg
        {
            get
            {
                return mSceneCfg;
            }
            set
            {
                mSceneCfg = value;
            }
        }

        public void parse(string xmlstr)
        {
            SecurityParser xmlDoc = new SecurityParser();
            xmlDoc.LoadXml(xmlstr);
            SecurityElement SE = xmlDoc.ToXml();
            ArrayList xnl = SE.Children;
            SceneNodeCfg node;
            foreach (SecurityElement xn in xnl)
            {
                SecurityElement xe = (SecurityElement)xn;

                if (xe.Tag == "Terrain")
                {
                    mSceneCfg.terrainCfg.parse(xe);
                }
                else
                {
                    node = new SceneNodeCfg();
                    node.parse(xe);
                    mSceneCfg.addSceneNode(node);
                }
            }
        }
    }
}