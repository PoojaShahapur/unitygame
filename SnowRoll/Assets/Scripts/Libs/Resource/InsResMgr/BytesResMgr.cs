using System;

namespace SDK.Lib
{
    public class BytesResMgr : InsResMgrBase
    {
        public BytesResMgr()
        {

        }

        public BytesRes getAndSyncLoadRes(string path, MAction<IDispatchObject> handle)
        {
            return getAndSyncLoad<BytesRes>(path, handle);
        }

        public BytesRes getAndAsyncLoadRes(string path, MAction<IDispatchObject> handle)
        {
            return getAndAsyncLoad<BytesRes>(path, handle);
        }
    }
}