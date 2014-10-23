/**
 * @brief 执行线程加载资源
 */
namespace SDK.Lib
{
    public class AsyncLoadTask : ThreadWrap
    {

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

        }
    }
}
