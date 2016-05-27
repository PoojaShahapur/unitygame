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
            if (ResLoadType.eStreamingAssets == m_resLoadType)
            {
                curPath = Path.Combine(MFileSys.getLocalReadDir(), m_loadPath);
            }
            else if (ResLoadType.ePersistentData == m_resLoadType)
            {
                curPath = Path.Combine(MFileSys.getLocalWriteDir(), m_loadPath);
            }
            mDataStream = new MDataStream(curPath);

            if (mDataStream != null)
            {
                nonRefCountResLoadResultNotify.resLoadState.setSuccessLoaded();
            }
            else
            {
                nonRefCountResLoadResultNotify.resLoadState.setFailed();
            }

            nonRefCountResLoadResultNotify.loadResEventDispatch.dispatchEvent(this);
        }
    }
}