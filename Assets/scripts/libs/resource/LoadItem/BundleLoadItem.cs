using SDK.Common;
using System.Collections;
using UnityEngine;

namespace SDK.Lib
{
    public class BundleLoadItem : LoadItem
    {
        override public void load()
        {
            base.load();
            loadFromDefaultAssetBundle();
        }

        // Resources.Load就是从一个缺省打进程序包里的AssetBundle里加载资源，而一般AssetBundle文件需要你自己创建，运行时 动态加载，可以指定路径和来源的。
        override protected void loadFromDefaultAssetBundle()
        {
            if (onLoaded != null)
            {
                onLoaded(this);
            }
        }

        // CreateFromFile(注意这种方法只能用于standalone程序）这是最快的加载方法
        override protected void loadFromAssetBundle()
        {
            string path;
            path = Application.dataPath + "/" + m_path;
            m_assetBundle = AssetBundle.CreateFromFile(path);

            if (onLoaded != null)
            {
                onLoaded(this);
            }
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
            yield return m_w3File;
            m_assetBundle = m_w3File.assetBundle;

            if (onLoaded != null)
            {
                onLoaded(this);
            }
        }
    }
}
