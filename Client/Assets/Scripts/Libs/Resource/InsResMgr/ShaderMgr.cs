using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
    public class ShaderMgr : InsResMgrBase
    {
        public Dictionary<string, Shader> m_ID2ShaderDic = new Dictionary<string, Shader>();

        public ShaderMgr()
        {

        }

        public ShaderRes getAndSyncLoadRes(string path)
        {
            return getAndSyncLoad<ShaderRes>(path);
        }

        public ShaderRes getAndAsyncLoadRes(string path, MAction<IDispatchObject> handle)
        {
            return getAndAsyncLoad<ShaderRes>(path, handle);
        }
    }
}