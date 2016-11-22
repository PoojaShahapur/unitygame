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
                Ctx.mInstance.mLogSys.log("prefab 为 null");
            }
            else
            {
                m_retGO = GameObject.Instantiate(m_go) as GameObject;
                if (null == m_retGO)
                {
                    Ctx.mInstance.mLogSys.log("不能实例化数据");
                }
            }

            return m_retGO;
        }

        public void InstantiateObject(string resName, ResInsEventDispatch evtHandle)
        {
            Ctx.mInstance.mCoroutineMgr.StartCoroutine(asyncInstantiateObject(resName, evtHandle));
        }

        public IEnumerator asyncInstantiateObject(string resName, ResInsEventDispatch evtHandle)
        {
            GameObject retGO = null;

            if (null == m_go)
            {
                Ctx.mInstance.mLogSys.log("prefab 为 null");
            }
            else
            {
                retGO = GameObject.Instantiate(m_go) as GameObject;
                if (null == retGO)
                {
                    Ctx.mInstance.mLogSys.log("不能实例化数据");
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
                // 一定要先设置 null，然后再调用 UnloadUnusedAssets ，否则删除不了 Resources 管理器中的 Asset-Object 资源
                m_go = null;

                UtilApi.UnloadUnusedAssets();

                //UtilApi.UnloadAsset(m_go);      // 强制卸载资源数据
                //UtilApi.DestroyImmediate(m_go, true); // 这个会删除磁盘上的资源
                //UtilApi.UnloadUnusedAssets();   // 卸载的时候， AssetBundles 完全卸载掉 AssetBundles 资源，因为现在使用 AssetBundles::Unload(true); 一次性卸载所有的资源，这次这里就不再调用 UnloadUnusedAssets 这个资源了 
            }

            m_retGO = null;

            base.unload();
        }
    }
}