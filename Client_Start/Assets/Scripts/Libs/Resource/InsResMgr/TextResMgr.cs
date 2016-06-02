using System;

namespace SDK.Lib
{
    public class TextResMgr : InsResMgrBase
    {
        public TextResMgr()
        {

        }

        public TextRes getAndSyncLoadRes(string path)
        {
            return getAndSyncLoad<TextRes>(path);
        }

        public TextRes getAndAsyncLoadRes(string path, MAction<IDispatchObject> handle)
        {
            return getAndAsyncLoad<TextRes>(path, handle);
        }
    }
}