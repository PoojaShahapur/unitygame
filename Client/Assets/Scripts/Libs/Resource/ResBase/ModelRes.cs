﻿using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    public class ModelRes : InsResBase
    {
        public GameObject m_go;
        public GameObject m_retGO;

        public ModelRes()
        {

        }

        override public void init(ResItem res)
        {
            m_go = res.getObject(res.getPrefabName()) as GameObject;
            base.init(res);
        }

        public GameObject InstantiateObject(string resName)
        {
            m_retGO = null;

            if (null == m_go)
            {
                Ctx.m_instance.m_logSys.log("prefab 为 null");
            }
            else
            {
                m_retGO = GameObject.Instantiate(m_go) as GameObject;
                if (null == m_retGO)
                {
                    Ctx.m_instance.m_logSys.log("不能实例化数据");
                }
            }
            return m_retGO;
        }

        public GameObject getObject()
        {
            return m_go;
        }

        public override void unload()
        {
            if (m_go != null)
            {
                //UtilApi.UnloadAsset(m_go);      // 强制卸载资源数据
                UtilApi.DestroyImmediate(m_go, true);
                m_go = null;
            }
            m_retGO = null;
        }
    }
}