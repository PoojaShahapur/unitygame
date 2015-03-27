using QuadTree;
using SDK.Common;

namespace SDK.Lib
{
    public class Scene
    {
        protected SceneCfg m_sceneCfg;
        protected string m_file;
        protected QuadTree<Area> m_quadTree;        // 地形四叉树
        protected ZoneSys m_zoneSys;

        public Scene()
        {
            m_sceneCfg = new SceneCfg();
            m_zoneSys = new ZoneSys();
        }

        public ZoneSys zoneSys
        {
            get
            {
                return m_zoneSys;
            }
            set
            {
                m_zoneSys = value;
            }
        }

        public SceneCfg sceneCfg
        {
            get
            {
                return m_sceneCfg;
            }
        }

        public string file
        {
            get
            {
                return m_file;
            }
            set
            {
                m_file = value;
            }
        }
    }
}