using System.Collections;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 预设资源，通常就一个资源，主要是 Resources 目录下的资源加载
     */
    public class PrefabResItem : ResItem
    {
        protected UnityEngine.Object m_prefabObj;       // 加载完成的 Prefab 对象
        protected UnityEngine.Object[] mAllPrefabObj;   // 所有的 Prefab 对象
        protected GameObject m_retGO;       // 方便调试的临时对象

        override public void init(LoadItem item)
        {
            base.init(item);
            if (!mIsLoadAll)
            {
                m_prefabObj = (item as ResourceLoadItem).prefabObj;
            }
            else
            {
                mAllPrefabObj = (item as ResourceLoadItem).getAllPrefabObject();
            }
            m_refCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            m_refCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }

        public UnityEngine.Object prefabObj()
        {
            return m_prefabObj;
        }

        public override string getPrefabName()         // 只有 Prefab 资源才实现这个函数
        {
            return m_prefabName;
        }

        public override void reset()
        {
            base.reset();
            m_prefabName = null;
            mAllPrefabObj = null;
            m_retGO = null;
        }

        override public void unload()
        {
            //if(m_prefabObj is GameObject)
            //{
            //    UtilApi.Destroy(m_prefabObj);
            //}
            //else
            //{
            //    UtilApi.UnloadAsset(m_prefabObj);
            //}
            // 如果你用个全局变量保存你 Load 的 Assets，又没有显式的设为 null，那 在这个变量失效前你无论如何 UnloadUnusedAssets 也释放不了那些Assets的。如果你这些Assets又不是从磁盘加载的，那除了 UnloadUnusedAssets 或者加载新场景以外没有其他方式可以卸载之。
            m_prefabObj = null;
            mAllPrefabObj = null;

            // Asset-Object 无法被Destroy销毁，Asset-Objec t由 Resources 系统管理，需要手工调用Resources.UnloadUnusedAssets()或者其他类似接口才能删除。
            UtilApi.UnloadUnusedAssets();
        }

        override public void InstantiateObject(string resName, ResInsEventDispatch evtHandle)
        {
            Ctx.m_instance.m_coroutineMgr.StartCoroutine(asyncInstantiateObject(resName, evtHandle));
        }

        override public GameObject InstantiateObject(string resName)
        {
            m_retGO = null;

            if (null == m_prefabObj)
            {
                Ctx.m_instance.m_logSys.log("prefab 为 null");
            }
            else
            {
                m_retGO = GameObject.Instantiate(m_prefabObj) as GameObject;
                if (null == m_retGO)
                {
                    Ctx.m_instance.m_logSys.log("不能实例化数据");
                }
            }
            return m_retGO;
        }

        override public IEnumerator asyncInstantiateObject(string resName, ResInsEventDispatch evtHandle)
        {
            GameObject retGO = null;

            if (null == m_prefabObj)
            {
                Ctx.m_instance.m_logSys.log("prefab 为 null");
            }
            else
            {
                retGO = GameObject.Instantiate(m_prefabObj) as GameObject;
                if (null == retGO)
                {
                    Ctx.m_instance.m_logSys.log("不能实例化数据");
                }
            }

            yield return null;
            evtHandle.setInsGO(retGO);
            evtHandle.dispatchEvent(evtHandle);
        }

        override public UnityEngine.Object getObject(string resName)
        {
            return m_prefabObj;
        }

        override public UnityEngine.Object[] getAllObject()
        {
            return mAllPrefabObj;
        }

        override public T[] loadAllAssets<T>()
        {
            /*
            // 使用 ArrayList，即使 ArrayList 中所有的类型都是 UnityEngine.Sprite ，也会导致不能使用 ToArray 转换成 UnityEngine.Sprite[] 数组形式
            ArrayList list = new ArrayList(mAllPrefabObj);
            int idx = mAllPrefabObj.Length - 1;
            while(idx >= 0)
            {
                if(mAllPrefabObj[idx] is T)
                {
                    list.Remove(mAllPrefabObj[idx]);
                }

                --idx;
            }
            return list.ToArray() as T[];
            */

            MList<T> list = new MList<T>();
            int idx = 0;
            int len = mAllPrefabObj.Length;
            while(idx < len)
            {
                if(mAllPrefabObj[idx] is T)
                {
                    list.Add(mAllPrefabObj[idx] as T);
                }

                ++idx;
            }

            return list.ToArray();
        }

        override public byte[] getBytes(string resName)            // 获取字节数据
        {
            if(m_prefabObj != null && (m_prefabObj as TextAsset) != null)
            {
                return (m_prefabObj as TextAsset).bytes;
            }

            return null;
        }

        override public string getText(string resName)
        {
            if (m_prefabObj != null && (m_prefabObj as TextAsset) != null)
            {
                return (m_prefabObj as TextAsset).text;
            }

            return null;
        }
    }
}