using System.IO;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 自定义打包文件加载
     */
    public class ABPakLoadItem : LoadItem
    {
        public FileStream m_fs = null;      // 文件句柄

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
                curPath = Path.Combine(Ctx.m_instance.m_localFileSys.getLocalReadDir(), m_path);
            }
            else if (ResLoadType.ePersistentData == m_resLoadType)
            {
                curPath = Path.Combine(Ctx.m_instance.m_localFileSys.getLocalWriteDir(), m_path);
            }
            m_fs = Ctx.m_instance.m_localFileSys.openFile(curPath);

            if (m_fs != null)
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