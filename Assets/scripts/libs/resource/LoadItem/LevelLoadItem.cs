using SDK.Common;
using System.Collections;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 支持从本地和 Web 服务器加载场景和场景 Bundle 资源
     */
    public class LevelLoadItem : LoadItem
    {
        protected string m_levelName;

        public string levelName
        {
            get
            {
                return m_levelName;
            }
            set
            {
                m_levelName = value;
            }
        }

        override public void load()
        {
            base.load();
            if (ResLoadType.eLoadDisc == m_resLoadType)
            {
                if (loadNeedCoroutine)
                {
                    Ctx.m_instance.m_CoroutineMgr.StartCoroutine(AsyncLoadFromDefaultAssetBundle());
                }
                else
                {
                    SyncLoadFromDefaultAssetBundle();
                }
            }
            else if (ResLoadType.eLoadDicWeb == m_resLoadType || ResLoadType.eLoadWeb == m_resLoadType)
            {
                Ctx.m_instance.m_CoroutineMgr.StartCoroutine(downloadAsset());
            }
        }

        // Resources.Load就是从一个缺省打进程序包里的AssetBundle里加载资源，而一般AssetBundle文件需要你自己创建，运行时 动态加载，可以指定路径和来源的。
        protected void SyncLoadFromDefaultAssetBundle()
        {
            //string path = Application.dataPath + "/" + m_path;
            string path = m_path;       // 注意这个是场景打包的时候场景的名字，不是目录，这个场景一定要 To add a level to the build settings use the menu File->Build Settings...
            Application.LoadLevel(path);

            if (onLoaded != null)
            {
                onLoaded(this);
            }
        }

        protected IEnumerator AsyncLoadFromDefaultAssetBundle()
        {
            //string path = Application.dataPath + "/" + m_path;
            string path = m_path;
            AsyncOperation asyncOpt = Application.LoadLevelAsync(path);

            yield return asyncOpt;

            if (onLoaded != null)
            {
                onLoaded(this);
            }
        }

        protected IEnumerator downloadAsset()
        {
            string path;
            //m_w3File = WWW.LoadFromCacheOrDownload(path, UnityEngine.Random.Range(int.MinValue, int.MaxValue));
            if (m_resLoadType == ResLoadType.eLoadDicWeb)
            {
                path = "file://" + Application.dataPath + "/" + m_path;
            }
            else
            {
                path = Ctx.m_instance.m_cfg.m_webIP + m_path;
            }
            m_w3File = WWW.LoadFromCacheOrDownload(path, 1);
            yield return m_w3File;
            m_assetBundle = m_w3File.assetBundle;

            AsyncOperation async = Application.LoadLevelAsync(m_levelName);
            yield return async;

            if (onLoaded != null)
            {
                onLoaded(this);
            }
        }
    }
}