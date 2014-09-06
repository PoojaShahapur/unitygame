using System;
using UnityEngine;
using System.Collections;

namespace San.Guo
{
    public class LoadItem : MonoBehaviour
    {
        protected string m_path;
        protected WWW m_w3File;

        public LoadItem(string path)
        {
            m_path = path;
        }

        public delegate void loaded(Component path);
        public loaded onLoaded;

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

        public void load()
        {
            StartCoroutine(downloadAsset());
        }

        IEnumerator downloadAsset()
        {
            string path = Ctx.m_instance.m_cfg.m_webIP + m_path;
            m_w3File = WWW.LoadFromCacheOrDownload(path, 1);
            yield return m_w3File;
            if(onLoaded != null)
            {
                onLoaded(this);
            }
        }
    }
}