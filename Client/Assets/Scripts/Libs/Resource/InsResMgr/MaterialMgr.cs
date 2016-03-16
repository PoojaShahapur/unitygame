using System;

namespace SDK.Lib
{
    public class MaterialMgr : ResMgrBase
    {
        //public Dictionary<MaterialID, Material> m_ID2MatDic = new Dictionary<MaterialID, Material>();

        public MaterialMgr()
        {

        }

        public MatRes getAndSyncLoad(string path)
        {
            return getAndSyncLoad<MatRes>(path);
        }

        public MatRes getAndAsyncLoad(string path, Action<IDispatchObject> handle)
        {
            return getAndAsyncLoad<MatRes>(path, handle);
        }
    }
}