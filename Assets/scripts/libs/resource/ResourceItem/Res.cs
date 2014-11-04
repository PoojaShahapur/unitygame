using System;
using UnityEngine;
using System.Collections;
using SDK.Common;

namespace SDK.Lib
{
    public class Res : IRes
    {
        protected ResPackType m_type;
        protected string m_path;
        protected bool m_resNeedCoroutine;     // 资源是否需要协同程序

        protected bool m_isLoaded;              // 资源是否加载完成
        protected bool m_isSucceed;             // 资源是否加载成功

        protected uint m_refNum;                // 引用计数
        protected ResLoadType m_resLoadType;   // 资源加载类型
        protected Action<SDK.Common.Event> onLoadedCB;        // 加载成功回调
        protected Action<SDK.Common.Event> onFailedCB;        // 加载失败回调

        public Res()
        {
            
        }

        public ResPackType GetResPackType()
        {
            return m_type;
        }

        public ResPackType type
        {
            get
            {
                return m_type;
            }
            set
            {
                m_type = value;
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

        public uint refNum
        {
            get
            {
                return m_refNum;
            }
            set
            {
                m_refNum = value;
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

        virtual public void init(LoadItem item)
        {

        }

        virtual public void reset()
        {
            //m_type = ResType.eNoneType;
            m_path = "";
            //m_resNeedCoroutine = false;
            m_isLoaded = false;
            m_isSucceed = false;
            m_refNum = 0;
        }

        virtual public void unload()
        {

        }

        public void addEventListener(EventID evtID, Action<SDK.Common.Event> cb)
        {
            if(EventID.LOADED_EVENT == evtID)       // 加载成功事件
            {
                onLoadedCB += cb;
            }
            else if (EventID.FAILED_EVENT == evtID)
            {
                onFailedCB += cb;
            }
        }

        public void removeEventListener(EventID evtID, Action<SDK.Common.Event> cb)
        {
            if (EventID.LOADED_EVENT == evtID)       // 加载成功事件
            {
                onLoadedCB -= cb;
            }
            else if (EventID.FAILED_EVENT == evtID)
            {
                onFailedCB -= cb;
            }
        }

        virtual public GameObject InstantiateObject(string resname)
        {
            return null;
        }

        virtual public UnityEngine.Object getObject(string resname)
        {
            return null;
        }
    }
}