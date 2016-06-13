using System.IO;

namespace SDK.Lib
{
    /**
    * @brief 从网络下载数据
    */
    public class DownloadItem : LoadItem, ITask
    {
        protected byte[] m_bytes;
        protected string m_version = "";
        protected bool m_isRunSuccess = true;
        protected string m_localPath;

        public DownloadItem()
        {

        }

        override public void reset()
        {
            base.reset();
            m_bytes = null;
        }

        override public void load()
        {
            base.load();
            m_localPath = Path.Combine(MFileSys.getLocalWriteDir(), UtilLogic.getRelPath(m_loadPath));
            if (!string.IsNullOrEmpty(m_version))
            {
                m_localPath = UtilLogic.combineVerPath(m_localPath, m_version);
            }

            Ctx.m_instance.m_logSys.log(string.Format("添加下载任务 {0}", m_loadPath));
        }

        public virtual void runTask()
        {

        }

        public virtual void handleResult()
        {

        }
    }
}