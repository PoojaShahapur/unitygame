using System;

namespace SDK.Lib
{
    public class MaterialMgr : InsResMgrBase
    {
        //public Dictionary<MaterialID, Material> m_ID2MatDic = new Dictionary<MaterialID, Material>();

        public MaterialMgr()
        {

        }

        public MatRes getAndSyncLoadRes(string path)
        {
            return getAndSyncLoad<MatRes>(path);
        }

        public MatRes getAndAsyncLoadRes(string path, MAction<IDispatchObject> handle)
        {
            return getAndAsyncLoad<MatRes>(path, handle);
        }
    }
}