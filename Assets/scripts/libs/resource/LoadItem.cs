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

        public LoadItem()
        {
            
        }

        public delegate void loaded(LoadItem item);
        public loaded onLoaded;
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

        public void load()
        {
            StartCoroutine(downloadAsset());
        }

        IEnumerator downloadAsset()
        {
            string path = Ctx.m_instance.m_cfg.m_webIP + m_path;
            //m_w3File = WWW.LoadFromCacheOrDownload(path, UnityEngine.Random.Range(int.MinValue, int.MaxValue));
            m_w3File = WWW.LoadFromCacheOrDownload(path, 150);
            yield return m_w3File;
            if(onLoaded != null)
            {
                onLoaded(this);
            }
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