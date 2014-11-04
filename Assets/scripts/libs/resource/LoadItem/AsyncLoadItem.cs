using SDK.Common;
using UnityEngine;

/**
 * @brief 线程异步加载，目前只支持 WWW 加载
 */
namespace SDK.Lib
{
    public class AsyncLoadItem : LoadItem
    {
        // 加载完成后，在线程中的初始化
        virtual public void AsyncInit()
        {

        }

        public void load()
        {
            m_ResLoadState = ResLoadState.eLoading;
            //if (m_resLoadType == ResLoadType.eLoadResource)     // 从默认资源 Bundle 中读取
            //{
            //    loadFromDefaultAssetBundle();
            //}
            //else if (m_resLoadType == ResLoadType.eLoadDisc && m_type != ResPackType.eLevelType)        // 从本地 Bundle 中读取
            //{
                // CreateFromFile(注意这种方法只能用于standalone程序）这是最快的加载方法
                // AssetBundle.CreateFromFile 这个函数仅支持未压缩的资源。这是加载资产包的最快方式。自己被这个函数坑了好几次，一定是非压缩的资源，如果压缩式不能加载的，加载后，内容也是空的
                //loadFromAssetBundle();
            //    downloadAsset();
            //}
            //else                                    // 从 web 服务器加载
            //{
            //    downloadAsset();
            //}
        }
    }
}