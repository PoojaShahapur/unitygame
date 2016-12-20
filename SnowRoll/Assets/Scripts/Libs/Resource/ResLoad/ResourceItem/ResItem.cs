using UnityEngine;
using System.Collections;

namespace SDK.Lib
{
    public class ResItem : IDispatchObject
    {
        protected ResPackType mResPackType;    // 资源打包类型
        protected ResLoadType mResLoadType;    // 资源加载类型

        protected string mLoadPath;            // 完整的目录
        protected string mOrigPath;            // 原始的资源目录
        protected string mExtName;             // 扩展名字
        protected string mPrefabName;          // 预制名字

        protected bool mResNeedCoroutine;      // 资源是否需要协同程序
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
            return mResPackType;
        }

        public ResPackType resPackType
        {
            get
            {
                return mResPackType;
            }
            set
            {
                mResPackType = value;
            }
        }

        public string loadPath
        {
            get
            {
                return mLoadPath;
            }
            set
            {
                mLoadPath = value;
            }
        }

        public string origPath
        {
            get
            {
                return mOrigPath;
            }
            set
            {
                mOrigPath = value;
            }
        }

        public string extName
        {
            get
            {
                return mExtName;
            }
            set
            {
                mExtName = value;
            }
        }

        public string prefabName
        {
            get
            {
                return mPrefabName;
            }
            set
            {
                mPrefabName = value;
            }
        }

        public string getLoadPath()
        {
            return mLoadPath;
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
                return mResNeedCoroutine;
            }
            set
            {
                mResNeedCoroutine = value;
            }
        }

        public ResLoadType resLoadType
        {
            get
            {
                return mResLoadType;
            }
            set
            {
                mResLoadType = value;
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
            return mPrefabName;
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
            mLoadPath = "";
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
            mResPackType = rhv.mResPackType;
            mResLoadType = rhv.mResLoadType;
            mLoadPath = rhv.mLoadPath;
            mOrigPath = rhv.mOrigPath;
            mExtName = rhv.mExtName;
            mResNeedCoroutine = rhv.mResNeedCoroutine;
            m_refCountResLoadResultNotify.copyFrom(rhv.refCountResLoadResultNotify);
        }

        virtual public void setLoadParam(LoadParam param)
        {
            this.resNeedCoroutine = param.mResNeedCoroutine;
            this.resPackType = param.mResPackType;
            this.resLoadType = param.mResLoadType;
            this.loadPath = param.mLoadPath;
            this.origPath = param.mOrigPath;
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