using System;

namespace SDK.Lib
{
    public class BytesResMgr : ResMgrBase
    {
        public BytesResMgr()
        {

        }

        public BytesRes getAndSyncLoadRes(string path)
        {
            return getAndSyncLoad<BytesRes>(path);
        }

        public BytesRes getAndAsyncLoadRes(string path, Action<IDispatchObject> handle)
        {
            return getAndAsyncLoad<BytesRes>(path, handle);
        }
    }
}