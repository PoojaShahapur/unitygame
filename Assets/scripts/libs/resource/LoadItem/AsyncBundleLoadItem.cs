using SDK.Common;
using System.Collections;
using UnityEngine;

namespace SDK.Lib
{
    class AsyncBundleLoadItem : AsyncLoadItem
    {
        override public void AsyncInit()
        {
            if (m_resLoadType == ResLoadType.eLoadResource)     // 从默认资源 Bundle 中读取
            {
                
            }
            else if (m_resLoadType == ResLoadType.eLoadDisc && m_type != ResPackType.eLevelType)        // 从本地 Bundle 中读取
            {
                m_assetBundle = m_w3File.assetBundle;
            }
            else
            {
                m_assetBundle = m_w3File.assetBundle;
            }
        }

        // Resources.Load就是从一个缺省打进程序包里的AssetBundle里加载资源，而一般AssetBundle文件需要你自己创建，运行时 动态加载，可以指定路径和来源的。
        override protected void loadFromDefaultAssetBundle()
        {
            
        }

        // CreateFromFile(注意这种方法只能用于standalone程序）这是最快的加载方法
        // AssetBundle.CreateFromFile 这个函数仅支持未压缩的资源。这是加载资产包的最快方式。自己被这个函数坑了好几次，一定是非压缩的资源，如果压缩式不能加载的，加载后，内容也是空的
        override protected void loadFromAssetBundle()
        {
            string path;
            path = Application.dataPath + "/" + m_path;
            m_assetBundle = AssetBundle.CreateFromFile(path);
        }

        override protected IEnumerator downloadAsset()
        {
            string path;
            //m_w3File = WWW.LoadFromCacheOrDownload(path, UnityEngine.Random.Range(int.MinValue, int.MaxValue));
            if (m_resLoadType == ResLoadType.eLoadDisc)
            {
                path = "file://" + Application.dataPath + "/" + m_path;
            }
            else
            {
                path = Ctx.m_instance.m_cfg.m_webIP + m_path;
            }
            m_w3File = WWW.LoadFromCacheOrDownload(path, 1);

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
