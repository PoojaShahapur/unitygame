using SDK.Common;
using System;

namespace SDK.Lib
{
    public class AsyncRes : IRes
    {
        protected ResPackType m_type;
        protected string m_path;

        protected bool m_isLoaded;              // 资源是否加载完成
        protected bool m_isSucceed;             // 资源是否加载成功

        protected uint m_refNum;                // 引用计数
        protected Action<IRes> onInited;
        protected ResLoadType m_resLoadType;    // 资源加载类型
        protected Action<Event> onLoadCB;        // 加载完成回调

        public AsyncRes()
        {
            
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

        public Action<IRes> onInitedCB
        {
            get
            {
                return onInited;
            }
            set
            {
                onInited = value;
            }
        }

        public bool HasLoaded()
        {
            return m_isLoaded;
        }

        virtual public void init(AsyncLoadItem item)
        {

        }

        virtual public void initAsset()
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
            onInited = null ;
        }

        virtual public void unload()
        {

        }

        public void addEventListener(EventID evtID, Action<Event> cb)
        {
            onLoadCB += cb;
        }

        public void removeEventListener(EventID evtID, Action<Event> cb)
        {
            onLoadCB -= cb;
        }
    }
}
