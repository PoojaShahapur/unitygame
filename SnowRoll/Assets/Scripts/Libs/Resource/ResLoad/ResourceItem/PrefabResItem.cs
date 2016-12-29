using System.Collections;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 预设资源，通常就一个资源，主要是 Resources 目录下的资源加载
     */
    public class PrefabResItem : ResItem
    {
        protected UnityEngine.Object mPrefabObj;       // 加载完成的 Prefab 对象
        protected UnityEngine.Object[] mAllPrefabObj;   // 所有的 Prefab 对象
        protected GameObject mRetGO;       // 方便调试的临时对象

        override public void init(LoadItem item)
        {
            base.init(item);

            if (!mIsLoadAll)
            {
                mPrefabObj = (item as ResourceLoadItem).prefabObj;
            }
            else
            {
                mAllPrefabObj = (item as ResourceLoadItem).getAllPrefabObject();
            }

            mRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            mRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }

        public UnityEngine.Object prefabObj()
        {
            return mPrefabObj;
        }

        public override string getPrefabName()         // 只有 Prefab 资源才实现这个函数
        {
            return mPrefabName;
        }

        public override void reset()
        {
            base.reset();

            mPrefabName = null;
            mAllPrefabObj = null;
            mRetGO = null;
        }

        override public void unrefAssetObject()
        {
            mPrefabObj = null;
            mAllPrefabObj = null;
            mRetGO = null;

            base.unrefAssetObject();
        }

        override public void unload(bool unloadAllLoadedObjects = true)
        {
            //if(mPrefabObj is GameObject)
            //{
            //    UtilApi.Destroy(mPrefabObj);
            //}
            //else
            //{
            //    UtilApi.UnloadAsset(mPrefabObj);
            //}
            // 如果你用个全局变量保存你 Load 的 Assets，又没有显式的设为 null，那 在这个变量失效前你无论如何 UnloadUnusedAssets 也释放不了那些Assets的。如果你这些Assets又不是从磁盘加载的，那除了 UnloadUnusedAssets 或者加载新场景以外没有其他方式可以卸载之。
            if (mPrefabObj != null)
            {
                // mPrefabObj = null;

                // Asset-Object 无法被Destroy销毁，Asset-Objec t由 Resources 系统管理，需要手工调用Resources.UnloadUnusedAssets()或者其他类似接口才能删除。
                if (mPrefabObj is GameObject)
                {
                    mPrefabObj = null;
                    UtilApi.UnloadUnusedAssets();   // UnloadUnusedAssets 可以卸载没有引用的资源，一定要先设置 null ，然后再调用 UnloadUnusedAssets
                }
                else
                {
                    UtilApi.UnloadAsset(mPrefabObj);    // 只能卸载组件类型的资源，比如 Textture、 Material、TextAsset 之类的资源，不能卸载容器之类的资源，例如 GameObject，因为组件是添加到 GameObject 上面去的
                    mPrefabObj = null;
                }
            }

            // 如果不想释放资源，只需要置空就行了
            if(mAllPrefabObj != null)
            {
                bool hasGameObject = false;
                int idx = 0;
                int len = mAllPrefabObj.Length;

                while(idx < len)
                {
                    if(mAllPrefabObj[idx] != null)
                    {
                        if (mAllPrefabObj[idx] is GameObject)
                        {
                            mAllPrefabObj[idx] = null;
                            hasGameObject = true;
                        }
                        else
                        {
                            UtilApi.UnloadAsset(mAllPrefabObj[idx]);
                            mAllPrefabObj[idx] = null;
                        }
                    }
                    ++idx;
                }
                mAllPrefabObj = null;

                if (hasGameObject)
                {
                    UtilApi.UnloadUnusedAssets();
                }
            }

            base.unload(unloadAllLoadedObjects);
        }

        override public void InstantiateObject(string resName, ResInsEventDispatch evtHandle)
        {
            Ctx.mInstance.mCoroutineMgr.StartCoroutine(asyncInstantiateObject(resName, evtHandle));
        }

        override public GameObject InstantiateObject(string resName)
        {
            mRetGO = null;

            if (null == mPrefabObj)
            {
                
            }
            else
            {
                mRetGO = GameObject.Instantiate(mPrefabObj) as GameObject;
                if (null == mRetGO)
                {
                    
                }
            }

            return mRetGO;
        }

        override public IEnumerator asyncInstantiateObject(string resName, ResInsEventDispatch evtHandle)
        {
            GameObject retGO = null;

            if (null == mPrefabObj)
            {
                
            }
            else
            {
                retGO = GameObject.Instantiate(mPrefabObj) as GameObject;
                if (null == retGO)
                {
                    
                }
            }

            yield return null;

            evtHandle.setInsGO(retGO);
            evtHandle.dispatchEvent(evtHandle);
        }

        override public UnityEngine.Object getObject(string resName)
        {
            return mPrefabObj;
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
            if(mPrefabObj != null && (mPrefabObj as TextAsset) != null)
            {
                return (mPrefabObj as TextAsset).bytes;
            }

            return null;
        }

        override public string getText(string resName)
        {
            if (mPrefabObj != null && (mPrefabObj as TextAsset) != null)
            {
                return (mPrefabObj as TextAsset).text;
            }

            return null;
        }
    }
}