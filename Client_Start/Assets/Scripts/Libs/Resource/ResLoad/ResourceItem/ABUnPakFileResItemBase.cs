using System.Collections;
using System.IO;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 打包的资源系统 base
     */
    public class ABUnPakFileResItemBase : FileResItem
    {
        public byte[] mBytes = null;
        protected AssetBundle mBundle = null;

        public void initByBytes(byte[] bytes, string prefixPath)
        {
            mBytes = bytes;
            m_bundlePath = Path.Combine(prefixPath, mLoadPath);

            // 检查是否资源打包成 unity3d 
            if (Ctx.mInstance.mCfg.mPakExtNameList.IndexOf(mExtName) != -1)
            {
                if (mResNeedCoroutine)
                {
                    Ctx.mInstance.mCoroutineMgr.StartCoroutine(initAssetByCoroutine());
                }
                else
                {
                    initAsset();
                }
            }
            else
            {
                m_refCountResLoadResultNotify.resLoadState.setSuccessLoaded();
                m_refCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
            }
        }

        virtual protected void initAsset()
        {
#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2
            mBundle = AssetBundle.CreateFromMemoryImmediate(mBytes);
#else
            mBundle = AssetBundle.LoadFromMemory(mBytes);
#endif
        }

        virtual protected IEnumerator initAssetByCoroutine()
        {
            return null;
        }

        override public void unload(bool unloadAllLoadedObjects = true)
        {
            mBytes = null;

            if (Ctx.mInstance.mCfg.mPakExtNameList.IndexOf(mExtName) != -1)         // 打包成 unity3d 加载的
            {
                if (mBundle != null)
                {
                    mBundle.Unload(false);
                    mBundle = null;
                }
                else
                {
                    Ctx.mInstance.mLogSys.log("When unity3d unload resource, AssetBundle load failed");
                }
            }
        }
    }
}