using System;

namespace SDK.Lib
{
    public class SkelAniMgr : InsResMgrBase
    {
        public SkelAniMgr()
        {

        }

        public SkelAnimRes getAndSyncLoadRes(string path)
        {
            return getAndSyncLoad<SkelAnimRes>(path);
        }

        public SkelAnimRes getAndAsyncLoadRes(string path, MAction<IDispatchObject> handle)
        {
            return getAndAsyncLoad<SkelAnimRes>(path, handle);
        }
    }
}