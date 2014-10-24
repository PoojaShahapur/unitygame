using System;
using UnityEngine;
using System.Collections;
using SDK.Common;

namespace SDK.Lib
{
    public class LoadItem : MonoBehaviour
    {
        protected ResPackType m_type;
        protected string m_path;
        protected WWW m_w3File;
        protected bool m_loadNeedCoroutine;    // 加载是否需要协同程序
        protected ResLoadType m_resLoadType;   // 资源加载类型
        protected AssetBundle m_assetBundle;
        protected ResLoadState m_ResLoadState = ResLoadState.eNotLoad;  // 资源加载状态

        public delegate void loaded(LoadItem item);
        public loaded onLoaded;

        public LoadItem()
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

        public void load()
        {
            m_ResLoadState = ResLoadState.eLoading;
            if (m_resLoadType == ResLoadType.eLoadResource)     // 从默认资源 Bundle 中读取
            {
                loadFromDefaultAssetBundle();
            }
            else if (m_resLoadType == ResLoadType.eLoadDisc && m_type != ResPackType.eLevelType)        // 从本地 Bundle 中读取
            {
                loadFromAssetBundle();
            }
            else                                    // 从 web 服务器加载
            {
                StartCoroutine(downloadAsset());
            }
        }

        // Resources.Load就是从一个缺省打进程序包里的AssetBundle里加载资源，而一般AssetBundle文件需要你自己创建，运行时 动态加载，可以指定路径和来源的。
        virtual protected void loadFromDefaultAssetBundle()
        {
            //string path = Application.dataPath + "/" + m_path;
            //if (m_type == ResPackType.eBundleType)
            //{
            //    m_prefabObj = Resources.Load(path);
            //}
            //else if (m_type == ResPackType.eLevelType)
            //{
            //    Application.LoadLevel(path);
            //}
            //if (onLoaded != null)
            //{
            //    onLoaded(this);
            //}
        }

        // CreateFromFile(注意这种方法只能用于standalone程序）这是最快的加载方法
        virtual protected void loadFromAssetBundle()
        {
            //string path;
            //if (m_type == ResPackType.eBundleType)
            //{
            //    path = Application.dataPath + "/" + m_path;
            //    m_assetBundle = AssetBundle.CreateFromFile(path);
            //}
            //if (onLoaded != null)
            //{
            //    onLoaded(this);
            //}
        }

        virtual protected IEnumerator downloadAsset()
        {
            //string path;
            //m_w3File = WWW.LoadFromCacheOrDownload(path, UnityEngine.Random.Range(int.MinValue, int.MaxValue));
            //if (m_resLoadType == ResLoadType.eLoadDisc)
            //{
            //    path = "file://" + Application.dataPath + "/" + m_path;
            //}
            //else
            //{
            //    path = Ctx.m_instance.m_cfg.m_webIP + m_path;
            //}
            //m_w3File = WWW.LoadFromCacheOrDownload(path, 1);
            //yield return m_w3File;
            //m_assetBundle = m_w3File.assetBundle;

            //AsyncOperation async = Application.LoadLevelAsync(m_levelName);
            //yield return async;

            //if(onLoaded != null)
            //{
            //    onLoaded(this);
            //}

            yield return null;
        }

        public void reset()
        {
            //m_type = ResType.eNoneType;
            m_path = "";
            //m_loadNeedCoroutine = false;
            m_w3File = null;
            m_loadNeedCoroutine = false;
        }
    }
}