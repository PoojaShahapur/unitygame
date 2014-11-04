using SDK.Common;

/**
 * @brief 执行线程加载资源
 */
namespace SDK.Lib
{
    public class AsyncLoadTask : ThreadWrap
    {
        protected AsyncResLoadData m_AsyncResLoadData;

        public AsyncResLoadData AsyncResLoadData
        {
            set
            {
                m_AsyncResLoadData = value;
            }
        }

        public AsyncLoadTask()
            : base(null, null)
        {
            base.cb = this.ThreadEntry;
            base.param = this;
        }

        // 线程入口
        public void ThreadEntry(object param)
        {
            // 执行加载任务
            while (!m_ExitFlag)
            {
                if(m_AsyncResLoadData.m_path2LDItem.Values.Count > 0)
                {
                    foreach(AsyncLoadItem loadItem in m_AsyncResLoadData.m_path2LDItem.Values)
                    {
                        if(loadItem.ResLoadState == ResLoadState.eNotLoad)  // 没有加载
                        {
                            loadItem.load();
                        }
                        else if(loadItem.ResLoadState == ResLoadState.eLoading) // 正在加载
                        {
                            //if(loadItem.w3File.isDone)          // 加载完成
                            //{
                            //    loadItem.ResLoadState = ResLoadState.eLoaded;
                            //}
                            //if(loadItem.w3File.error.Length > 0)          // 加载出现错误
                            //{
                            //    loadItem.ResLoadState = ResLoadState.eFailed;
                            //}
                            loadItem.CheckLoadState();
                        }
                    }
                }
            }
        }
    }
}