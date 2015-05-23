using System;
using UnityEngine;
using System.Collections;
using SDK.Common;

namespace SDK.Lib
{
    public class ResItem : IDispatchObject
    {
        protected ResPackType m_resPackType;    // 资源打包类型
        protected ResLoadType m_resLoadType;    // 资源加载类型

        protected string m_path;                // 完整的目录
        protected string m_pathNoExt;           // 不包括扩展名字的路径
        protected string m_extName;             // 扩展名字

        protected bool m_resNeedCoroutine;     // 资源是否需要协同程序
        protected ResLoadState m_resLoadState;  // 资源加载状态
        protected EventDispatch m_loadEventDispatch;
        protected RefCount m_refCount;

        public ResItem()
        {
            m_resLoadState = new ResLoadState();
            m_loadEventDispatch = new EventDispatch();
            m_refCount = new RefCount();
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

        public ResLoadState resLoadState
        {
            get
            {
                return m_resLoadState;
            }
            set
            {
                m_resLoadState = value;
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

        public EventDispatch loadEventDispatch
        {
            get
            {
                return m_loadEventDispatch;
            }
            set
            {
                m_loadEventDispatch = value;
            }
        }

        public RefCount refCount
        {
            get
            {
                return m_refCount;
            }
            set
            {
                m_refCount = value;
            }
        }

        public virtual string getPrefabName()         // 只有 Prefab 资源才实现这个函数
        {
            return "";
        }

        virtual public void init(LoadItem item)
        {
            m_resLoadState.setSuccessLoaded();
        }

        virtual public void failed(LoadItem item)
        {
            m_resLoadState.setFailed();
            m_loadEventDispatch.dispatchEvent(this);
            clearListener();
        }

        public void clearListener()
        {
            m_loadEventDispatch.clearEventHandle();
        }

        virtual public void reset()
        {
            m_path = "";
            m_resLoadState.reset();
            m_refCount.refNum = 0;
            clearListener();
        }

        // 卸载
        virtual public void unload()
        {

        }

        virtual public GameObject InstantiateObject(string resName)
        {
            return null;
        }

        virtual public UnityEngine.Object getObject(string resName)
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
            m_resLoadState = rhv.resLoadState;
            m_refCount.refNum = rhv.refCount.refNum;
            m_loadEventDispatch = rhv.loadEventDispatch;
        }
    }
}