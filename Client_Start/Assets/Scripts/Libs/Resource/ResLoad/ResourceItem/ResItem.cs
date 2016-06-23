using UnityEngine;
using System.Collections;

namespace SDK.Lib
{
    public class ResItem : IDispatchObject
    {
        protected ResPackType m_resPackType;    // 资源打包类型
        protected ResLoadType m_resLoadType;    // 资源加载类型

        protected string m_loadPath;            // 完整的目录
        protected string m_origPath;            // 原始的资源目录
        protected string m_extName;             // 扩展名字
        protected string m_prefabName;          // 预制名字

        protected bool m_resNeedCoroutine;      // 资源是否需要协同程序
        protected RefCountResLoadResultNotify m_refCountResLoadResultNotify;
        protected bool mIsLoadAll;               // 是否加载所有的内容
        protected string mResUniqueId;          // 资源唯一 Id，查找资源的索引
        protected string mLogicPath;

        public ResItem()
        {
            mIsLoadAll = false;
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

        public string loadPath
        {
            get
            {
                return m_loadPath;
            }
            set
            {
                m_loadPath = value;
            }
        }

        public string origPath
        {
            get
            {
                return m_origPath;
            }
            set
            {
                m_origPath = value;
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

        public string getLoadPath()
        {
            return m_loadPath;
        }

        public void setLogicPath(string value)
        {
            mLogicPath = value;
        }

        public string getLogicPath()
        {
            return mLogicPath;
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

        // 释放 Asset-Object 资源，自己保存引用
        virtual public void unrefAssetObject()
        {

        }

        public void setLoadAll(bool value)
        {
            mIsLoadAll = value;
        }

        public bool getLoadAll()
        {
            return mIsLoadAll;
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
            
        }

        virtual public void failed(LoadItem item)
        {
            m_refCountResLoadResultNotify.resLoadState.setFailed();
            m_refCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }

        virtual public void reset()
        {
            m_loadPath = "";
            m_refCountResLoadResultNotify.resLoadState.reset();
            m_refCountResLoadResultNotify.refCount.refNum = 0;
        }

        // 卸载
        virtual public void unload(bool unloadAllLoadedObjects = true)
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
            m_loadPath = rhv.m_loadPath;
            m_origPath = rhv.m_origPath;
            m_extName = rhv.m_extName;
            m_resNeedCoroutine = rhv.m_resNeedCoroutine;
            m_refCountResLoadResultNotify.copyFrom(rhv.refCountResLoadResultNotify);
        }

        virtual public void setLoadParam(LoadParam param)
        {
            this.resNeedCoroutine = param.m_resNeedCoroutine;
            this.resPackType = param.m_resPackType;
            this.resLoadType = param.m_resLoadType;
            this.loadPath = param.mLoadPath;
            this.origPath = param.m_origPath;
            this.extName = param.extName;
            this.setLoadAll(param.mIsLoadAll);
            this.setResUniqueId(param.mResUniqueId);
            this.setLogicPath(param.mLogicPath);
        }

        public bool hasSuccessLoaded()
        {
            return m_refCountResLoadResultNotify.resLoadState.hasSuccessLoaded();
        }

        public bool hasFailed()
        {
            return m_refCountResLoadResultNotify.resLoadState.hasFailed();
        }
    }
}