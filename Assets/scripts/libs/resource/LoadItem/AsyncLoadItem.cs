using SDK.Common;
using UnityEngine;

/**
 * @brief 线程异步加载，目前只支持 WWW 加载
 */
namespace SDK.Lib
{
    public class AsyncLoadItem
    {
        protected ResPackType m_type;
        protected string m_path;
        protected WWW m_w3File;
        protected ResLoadType m_resLoadType;   // 资源加载类型
        protected AssetBundle m_assetBundle;
        protected ResLoadState m_ResLoadState = ResLoadState.eNotLoad;  // 资源加载状态

        public delegate void loaded(AsyncLoadItem item);
        public loaded onLoaded;

        public WWW w3File
        {
            get
            {
                return m_w3File;
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
            string path;
            if (m_resLoadType == ResLoadType.eLoadDisc)
            {
                path = "file://" + Application.dataPath + "/" + m_path;
            }
            else
            {
                path = Ctx.m_instance.m_cfg.m_webIP + m_path;
            }
            m_w3File = WWW.LoadFromCacheOrDownload(path, 1);
        }

        // 加载完成后，在线程中的初始化
        virtual public void AsyncInit()
        {

        }

        public void reset()
        {
            m_path = "";
            m_w3File = null;
        }
    }
}
