using System.Collections;
using UnityEngine;

namespace SDK.Lib
{
    public class PrefabResBase : InsResBase
    {
        public GameObject m_go;
        public GameObject m_retGO;

        public PrefabResBase()
        {

        }

        override protected void initImpl(ResItem res)
        {
            m_go = res.getObject(res.getPrefabName()) as GameObject;
            base.initImpl(res);
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

        public void InstantiateObject(string resName, ResInsEventDispatch evtHandle)
        {
            Ctx.m_instance.m_coroutineMgr.StartCoroutine(asyncInstantiateObject(resName, evtHandle));
        }

        public IEnumerator asyncInstantiateObject(string resName, ResInsEventDispatch evtHandle)
        {
            GameObject retGO = null;

            if (null == m_go)
            {
                Ctx.m_instance.m_logSys.log("prefab 为 null");
            }
            else
            {
                retGO = GameObject.Instantiate(m_go) as GameObject;
                if (null == retGO)
                {
                    Ctx.m_instance.m_logSys.log("不能实例化数据");
                }
            }

            yield return null;
            evtHandle.setInsGO(retGO);
            evtHandle.dispatchEvent(evtHandle);
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
                //UtilApi.DestroyImmediate(m_go, true);
                UtilApi.UnloadUnusedAssets();
                m_go = null;
            }
            m_retGO = null;
            base.unload();
        }
    }
}