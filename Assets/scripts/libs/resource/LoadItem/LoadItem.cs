using System;
using UnityEngine;
using System.Collections;
using SDK.Common;

namespace SDK.Lib
{
    public class LoadItem
    {
        protected ResPackType m_resPackType;
        protected ResLoadType m_resLoadType;   // 资源加载类型

        protected string m_path;
        protected WWW m_w3File;
        protected bool m_loadNeedCoroutine;    // 加载是否需要协同程序

        protected AssetBundle m_assetBundle;
        protected ResLoadState m_ResLoadState = ResLoadState.eNotLoad;  // 资源加载状态

        public delegate void loaded(LoadItem item);
        public loaded onLoaded;

        public LoadItem()
        {
            
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

        public WWW w3File
        {
            get
            {
                return m_w3File;
            }
        }

        public bool loadNeedCoroutine
        {
            get
            {
                return m_loadNeedCoroutine;
            }
            set
            {
                m_loadNeedCoroutine = value;
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

        public AssetBundle assetBundle
        {
            get
            {
                return m_assetBundle;
            }
            set
            {
                m_assetBundle = value;
            }
        }

        public ResLoadState ResLoadState
        {
            get
            {
                return m_ResLoadState;
            }
            set
            {
                m_ResLoadState = value;
            }
        }

        virtual public void load()
        {
            m_ResLoadState = ResLoadState.eLoading;
        }

        public void reset()
        {
            //m_type = ResType.eNoneType;
            m_path = "";
            //m_loadNeedCoroutine = false;
            m_w3File = null;
            m_loadNeedCoroutine = false;
        }

        // 检查加载状态
        virtual public void CheckLoadState()
        {

        }

        protected void deleteFromCache(string path)
        {
            if(Caching.IsVersionCached(path, 1))
            {
                //Caching.DeleteFromCache(path);
                Caching.CleanCache();
            }
        }
    }
}