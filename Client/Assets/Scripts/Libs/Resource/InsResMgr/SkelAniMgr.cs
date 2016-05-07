using System;

namespace SDK.Lib
{
    public class SkelAniMgr : ResMgrBase
    {
        public SkelAniMgr()
        {

        }

        public SkelAnimRes getAndSyncLoadRes(string path)
        {
            return getAndSyncLoad<SkelAnimRes>(path);
        }

        public SkelAnimRes getAndAsyncLoadRes(string path, Action<IDispatchObject> handle)
        {
            return getAndAsyncLoad<SkelAnimRes>(path, handle);
        }
    }
}