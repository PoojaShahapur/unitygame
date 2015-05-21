using System;
using UnityEngine;
using System.Collections;
using SDK.Common;

namespace SDK.Lib
{
    public class ResItem : RefCount, IResItem
    {
        protected ResPackType m_resPackType;    // 资源打包类型
        protected ResLoadType m_resLoadType;    // 资源加载类型

        protected string m_path;                // 完整的目录
        protected string m_pathNoExt;           // 不包括扩展名字的路径
        protected string m_extName;             // 扩展名字

        protected bool m_resNeedCoroutine;     // 资源是否需要协同程序

        protected bool m_isLoaded;              // 资源是否加载完成
        protected bool m_isSucceed;             // 资源是否加载成功

        protected Action<IDispatchObject> onLoaded;        // 加载成功回调
        protected Action<IDispatchObject> onFailed;        // 加载失败回调

        public ResItem()
        {
            
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

        public bool isLoaded
        {
            get
            {
                return m_isLoaded;
            }
            set
            {
                m_isLoaded = value;
            }
        }

        public bool isSucceed
        {
            get
            {
                return m_isSucceed;
            }
            set
            {
                m_isSucceed = value;
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

        public bool HasLoaded()
        {
            return m_isLoaded;
        }

        public virtual string getPrefabName()         // 只有 Prefab 资源才实现这个函数
        {
            return "";
        }

        virtual public void init(LoadItem item)
        {
            m_isLoaded = true;
            m_isSucceed = true;
        }

        virtual public void failed(LoadItem item)
        {
            m_isLoaded = true;
            m_isSucceed = false;

            if(onFailed != null)
            {
                onFailed(this);
            }

            clearListener();
        }

        public void clearListener()
        {
            onLoaded = null;            // 清理事件监听器
            onFailed = null;            // 清理事件监听器
        }

        virtual public void reset()
        {
            //m_type = ResType.eNoneType;
            m_path = "";
            //m_resNeedCoroutine = false;
            m_isLoaded = false;
            m_isSucceed = false;
            m_refNum = 0;
            clearListener();
        }

        // 卸载
        virtual public void unload()
        {

        }

        public void addEventListener(EventID evtID, Action<IDispatchObject> cb)
        {
            if(EventID.LOADED_EVENT == evtID)       // 加载成功事件
            {
                onLoaded += cb;
            }
            else if (EventID.FAILED_EVENT == evtID)
            {
                onFailed += cb;
            }
        }

        public void removeEventListener(EventID evtID, Action<IDispatchObject> cb)
        {
            if (EventID.LOADED_EVENT == evtID)       // 加载成功事件
            {
                onLoaded -= cb;
            }
            else if (EventID.FAILED_EVENT == evtID)
            {
                onFailed -= cb;
            }
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
            m_isLoaded = rhv.m_isLoaded;
            m_isSucceed = rhv.m_isSucceed;
            m_refNum = rhv.m_refNum;
            onLoaded = rhv.onLoaded;
            onFailed = rhv.onFailed;
        }
    }
}