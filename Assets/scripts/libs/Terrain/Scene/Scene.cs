﻿using SDK.Common;

namespace SDK.Lib
{
    public class Scene : IScene
    {
        protected SceneCfg m_sceneCfg;
        protected string m_file;

        public Scene()
        {
            m_sceneCfg = new SceneCfg();
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