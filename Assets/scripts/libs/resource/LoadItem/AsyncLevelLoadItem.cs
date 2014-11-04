using SDK.Common;
using System.Collections;
using UnityEngine;
namespace SDK.Lib
{
    class AsyncLevelLoadItem : AsyncLoadItem
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

        // 加载完成后，在线程中的初始化
        override public void AsyncInit()
        {
            m_assetBundle = m_w3File.assetBundle;
            Application.LoadLevel(m_levelName);
        }

        // Resources.Load就是从一个缺省打进程序包里的AssetBundle里加载资源，而一般AssetBundle文件需要你自己创建，运行时 动态加载，可以指定路径和来源的。
        protected void loadFromDefaultAssetBundle()
        {
            //string path = Application.dataPath + "/" + m_path;
            string path = Ctx.m_instance.m_cfg.m_dataPath + "/" + m_path;
            Application.LoadLevel(path);
        }

        // CreateFromFile(注意这种方法只能用于standalone程序）这是最快的加载方法
        protected void loadFromAssetBundle()
        {
            string path;
            //path = Application.dataPath + "/" + m_path;
            path = Ctx.m_instance.m_cfg.m_dataPath + "/" + m_path;
            m_assetBundle = AssetBundle.CreateFromFile(path);

            Application.LoadLevel(path);
        }

        protected IEnumerator downloadAsset()
        {
            string path;
            //m_w3File = WWW.LoadFromCacheOrDownload(path, UnityEngine.Random.Range(int.MinValue, int.MaxValue));
            if (m_resLoadType == ResLoadType.eLoadDisc)
            {
                //path = "file://" + Application.dataPath + "/" + m_path;
                path = "file://" + Ctx.m_instance.m_cfg.m_dataPath + "/" + m_path;
            }
            else
            {
                path = Ctx.m_instance.m_cfg.m_webIP + m_path;
            }
            m_w3File = WWW.LoadFromCacheOrDownload(path, 1);
            //yield return m_w3File;
            //m_assetBundle = m_w3File.assetBundle;

            //AsyncOperation async = Application.LoadLevelAsync(m_levelName);
            //yield return async;

            return null;
        }

        override public void CheckLoadState()
        {
            if (m_resLoadType == ResLoadType.eLoadResource)     // 从默认资源 Bundle 中读取
            {
                AsyncInit();
                ResLoadState = ResLoadState.eLoaded;
            }
            else if (m_resLoadType == ResLoadType.eLoadDisc && m_type != ResPackType.eLevelType)        // 从本地 Bundle 中读取
            {
                if (w3File.isDone)          // 加载完成
                {
                    AsyncInit();
                    ResLoadState = ResLoadState.eLoaded;
                }
                else if (w3File.error.Length > 0)          // 加载出现错误
                {
                    ResLoadState = ResLoadState.eFailed;
                }
            }
            else                                    // 从 web 服务器加载
            {
                if (w3File.isDone)          // 加载完成
                {
                    AsyncInit();
                    ResLoadState = ResLoadState.eLoaded;
                }
                else if (w3File.error.Length > 0)          // 加载出现错误
                {
                    ResLoadState = ResLoadState.eFailed;
                }
            }
        }
    }
}
