using UnityEngine;
using System;
using System.Collections.Generic;

namespace San.Guo
{
    public class ResMgr : IResMgr
    {
        protected LoadParam m_loadParam;
        protected Dictionary<string, LoadItem> m_path2LDItem;
        protected IDictionary<string, Res> m_path2Res;

        public ResMgr()
        {
            m_path2LDItem = new Dictionary<string, LoadItem>();
            m_path2Res = new Dictionary<string, Res>();
            m_loadParam = new LoadParam();
        }

        public LoadParam loadParam
        {
            get
            {
                return m_loadParam;
            }
        }

        public Res load(LoadParam param)
        {
            if (m_path2Res.ContainsKey(param.m_path))
            {
                if (param.m_cb != null)
                {
                    param.m_cb(m_path2Res[param.m_path]);
                }
                return m_path2Res[param.m_path];
            }

            if(param.m_type == ResType.eLevelType)
            {
                //m_path2Res[param.m_path] = new LevelRes(param.m_path, param.m_lvlName);
                m_path2Res[param.m_path] = Ctx.m_instance.m_dataTrans.gameObject.AddComponent<LevelRes>() as LevelRes;
                m_path2Res[param.m_path].type = param.m_type;
                m_path2Res[param.m_path].path = param.m_path;
                (m_path2Res[param.m_path] as LevelRes).levelName = param.m_lvlName;
            }
            
            //m_path2LDItem[param.m_path] = new LoadItem(param.m_path);
            m_path2LDItem[param.m_path] = Ctx.m_instance.m_dataTrans.gameObject.AddComponent<LoadItem>() as LoadItem;
            m_path2LDItem[param.m_path].path = param.m_path;
            m_path2LDItem[param.m_path].onLoaded += onLoad;
            m_path2LDItem[param.m_path].load();

            return m_path2Res[param.m_path];
        }

        public void unload(string path)
        {
            if (m_path2Res.ContainsKey(path))
            {
                m_path2Res.Remove(path);
            }
        }

        public void onLoad(Component cmpt)
        {
            string path = (cmpt as LoadItem).path;
            m_path2LDItem[path].onLoaded -= onLoad;
            if(m_path2Res[path] != null)
            {
                m_path2Res[path].init(m_path2LDItem[path]);
            }

            GameObject.Destroy(m_path2Res[path]);
            GameObject.Destroy(cmpt);
            m_path2LDItem.Remove(path);
        }
    }
}