using System;
using UnityEngine;
using System.Collections;

namespace San.Guo
{
    public class Res : MonoBehaviour
    {
        protected ResType m_type;
        protected string m_path;
        protected bool m_resNeedCoroutine;     // 资源是否需要协同程序

        protected bool m_isLoaded;              // 资源是否加载完成
        protected bool m_isSucceed;             // 资源是否加载成功

        protected uint m_refNum;                // 引用计数

        protected delegate void Init();
        protected Init onInited;

        //public Res(string path)
        public Res()
        {
            //m_path = path;
        }

        public ResType type
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

        virtual public void init(LoadItem item)
        {

        }

        virtual public IEnumerator initAssetByCoroutine()
        {
            return null;
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
    }
}