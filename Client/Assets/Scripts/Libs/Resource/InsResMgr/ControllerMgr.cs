using System;

namespace SDK.Lib
{
    /**
     * @brief 主要是 Animator 中 Controller 管理器
     */
    public class ControllerMgr : InsResMgrBase
    {
        public ControllerRes getAndSyncLoadRes(string path)
        {
            return getAndSyncLoad<ControllerRes>(path);
        }

        public ControllerRes getAndAsyncLoadRes(string path, Action<IDispatchObject> handle)
        {
            return getAndAsyncLoad<ControllerRes>(path, handle);
        }
    }
}