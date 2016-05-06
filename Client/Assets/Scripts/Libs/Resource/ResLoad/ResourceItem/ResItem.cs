using System;
using UnityEngine;
using System.Collections;

namespace SDK.Lib
{
    public class ResItem : IDispatchObject
    {
        protected ResPackType m_resPackType;    // 资源打包类型
        protected ResLoadType m_resLoadType;    // 资源加载类型

        protected string m_path;                // 完整的目录
        protected string m_pathNoExt;           // 不包括扩展名字的路径
        protected string m_extName;             // 扩展名字
        protected string m_prefabName;          // 预制名字

        protected bool m_resNeedCoroutine;      // 资源是否需要协同程序
        protected RefCountResLoadResultNotify m_refCountResLoadResultNotify;
        protected bool mIsLoaded;               // 是否加载所有的内容
        protected string mResUniqueId;             // 资源唯一 Id，查找资源的索引

        public ResItem()
        {
            mIsLoaded = false;
            m_refCountResLoadResultNotify = new RefCountResLoadResultNotify();
        }

        public ResPackType GetResPackType()
        {
            return m_resPackType;
        }

        public ResPackType resPackType
        {
            get
            {
                return m_resPackType;
            }
            set
            {
                m_resPackType = value;
            }
        }

        public string path
        {
            get
            {
                return m_path;
            }
            set
            {
                m_path = value;
            }
        }

        public string pathNoExt
        {
            get
            {
                return m_pathNoExt;
            }
            set
            {
                m_pathNoExt = value;
            }
        }

        public string extName
        {
            get
            {
                return m_extName;
            }
            set
            {
                m_extName = value;
            }
        }

        public string prefabName
        {
            get
            {
                return m_prefabName;
            }
            set
            {
                m_prefabName = value;
            }
        }

        public string GetPath()
        {
            return m_path;
        }

        public bool resNeedCoroutine
        {
            get
            {
                return m_resNeedCoroutine;
            }
            set
            {
                m_resNeedCoroutine = value;
            }
        }

        public ResLoadType resLoadType
        {
            get
            {
                return m_resLoadType;
            }
            set
            {
                m_resLoadType = value;
            }
        }

        public RefCountResLoadResultNotify refCountResLoadResultNotify
        {
            get
            {
                return m_refCountResLoadResultNotify;
            }
            set
            {
                m_refCountResLoadResultNotify = value;
            }
        }

        public void setLoadAll(bool value)
        {
            mIsLoaded = value;
        }

        public bool getLoadAll()
        {
            return mIsLoaded;
        }

        public void setResUniqueId(string value)
        {
            mResUniqueId = value;
        }

        public string getResUniqueId()
        {
            return mResUniqueId;
        }

        public virtual string getPrefabName()
        {
            return m_prefabName;
        }

        virtual public void init(LoadItem item)
        {
            // 这个状态一定要在事件分发之前设置这个状态，如果在使用协程之前就设置，几个协程的执行完成的顺序可能是不同的
            //m_refCountResLoadResultNotify.resLoadState.setSuccessLoaded();
        }

        virtual public void failed(LoadItem item)
        {
            m_refCountResLoadResultNotify.resLoadState.setFailed();
            m_refCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }

        virtual public void reset()
        {
            m_path = "";
            m_refCountResLoadResultNotify.resLoadState.reset();
            m_refCountResLoadResultNotify.refCount.refNum = 0;
        }

        // 卸载
        virtual public void unload()
        {

        }

        virtual public void InstantiateObject(string resName, ResInsEventDispatch evtHandle)
        {

        }

        virtual public GameObject InstantiateObject(string resName)
        {
            return null;
        }

        virtual public IEnumerator asyncInstantiateObject(string resName, ResInsEventDispatch evtHandle)
        {
            return null;
        }

        virtual public UnityEngine.Object getObject(string resName)
        {
            return null;
        }

        virtual public UnityEngine.Object[] getAllObject()
        {
            return null;
        }

        virtual public T[] loadAllAssets<T>() where T : UnityEngine.Object
        {
            return null;
        }

        virtual public byte[] getBytes(string resName)            // 获取字节数据
        {
            return null;
        }

        virtual public string getText(string resName)
        {
            return null;
        }

        public void copyFrom(ResItem rhv)
        {
            m_resPackType = rhv.m_resPackType;
            m_resLoadType = rhv.m_resLoadType;
            m_path = rhv.m_path;
            m_pathNoExt = rhv.m_pathNoExt;
            m_extName = rhv.m_extName;
            m_resNeedCoroutine = rhv.m_resNeedCoroutine;
            m_refCountResLoadResultNotify.copyFrom(rhv.refCountResLoadResultNotify);
        }
    }
}