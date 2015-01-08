using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
    public class ShaderMgr : ResMgrBase
    {
        public Dictionary<string, Shader> m_ID2ShaderDic = new Dictionary<string, Shader>();

        public ShaderMgr()
        {

        }
    }
}