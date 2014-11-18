using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    public class PartInfo
    {
        public IRes m_res;              // 加载的资源
        public string m_bundleName;     // 资源包 assetBundle 的名字
        public string m_partName;       // 资源的名字
        public GameObject m_partGo;     // 实例化的资源
    }
}