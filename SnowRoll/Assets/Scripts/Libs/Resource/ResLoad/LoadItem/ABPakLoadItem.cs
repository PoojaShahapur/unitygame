using System.IO;

namespace SDK.Lib
{
    /**
     * @brief 自定义打包文件加载
     */
    public class ABPakLoadItem : LoadItem
    {
        public MDataStream mDataStream = null;      // 文件流

        override public void reset()
        {
            base.reset();
        }

        override public void load()
        {
            base.load();

            string curPath = "";
            if (ResLoadType.eLoadStreamingAssets == mResLoadType)
            {
                curPath = Path.Combine(MFileSys.getLocalReadDir(), mLoadPath);
            }
            else if (ResLoadType.eLoadLocalPersistentData == mResLoadType)
            {
                curPath = Path.Combine(MFileSys.getLocalWriteDir(), mLoadPath);
            }
            mDataStream = new MDataStream(curPath);

            if (mDataStream != null)
            {
                m_nonRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            }
            else
            {
                m_nonRefCountResLoadResultNotify.resLoadState.setFailed();
            }

            m_nonRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }
    }
}