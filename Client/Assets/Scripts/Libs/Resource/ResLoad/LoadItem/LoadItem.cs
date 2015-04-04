﻿using System;
using UnityEngine;
using System.Collections;
using SDK.Common;

namespace SDK.Lib
{
    public class LoadItem
    {
        protected ResPackType m_resPackType;
        protected ResLoadType m_resLoadType;   // 资源加载类型

        protected string m_path;                // 这个是相对目录，但是不包括扩展名字
        protected WWW m_w3File;
        protected bool m_loadNeedCoroutine;     // 加载是否需要协同程序

        protected AssetBundle m_assetBundle;
        protected ResLoadState m_ResLoadState = ResLoadState.eNotLoad;  // 资源加载状态

        public Action<LoadItem> onLoaded;
        public Action<LoadItem> onFailed;

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

        virtual public void reset()
        {
            //m_type = ResType.eNoneType;
            m_path = "";
            //m_loadNeedCoroutine = false;
            m_w3File = null;
            m_loadNeedCoroutine = false;

            clearListener();
        }

        virtual protected IEnumerator downloadAsset()
        {
            string path = "";
            if (m_resLoadType == ResLoadType.eLoadDicWeb)
            {
                path = "file://" + Application.dataPath + "/" + m_path;
            }
            else if (m_resLoadType == ResLoadType.eLoadWeb)
            {
                path = Ctx.m_instance.m_cfg.m_webIP + m_path;
            }
            deleteFromCache(path);
            m_w3File = WWW.LoadFromCacheOrDownload(path, 1);
            //m_w3File = WWW.LoadFromCacheOrDownload(path, UnityEngine.Random.Range(0, int.MaxValue));
            yield return m_w3File;

            onWWWEnd();
        }

        // 检查加载成功
        protected bool isLoadedSuccess(WWW www)
        {
            if (www.error != null)
            {
                return false;
            }

            return true;
        }

        // 加载完成回调处理
        virtual protected void onWWWEnd()
        {
            if (isLoadedSuccess(m_w3File))
            {
                m_assetBundle = m_w3File.assetBundle;

                if (onLoaded != null)
                {
                    onLoaded(this);
                }
            }
            else
            {
                if (onFailed != null)
                {
                    onFailed(this);
                }
            }
        }

        protected void deleteFromCache(string path)
        {
            if(Caching.IsVersionCached(path, 1))
            {
                //Caching.DeleteFromCache(path);
                Caching.CleanCache();
            }
        }

        public void clearListener()
        {
            onLoaded = null;            // 清理事件监听器
            onFailed = null;            // 清理事件监听器
        }
    }
}