using System.IO;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief Asset Bundles 打包的普通文件资源
     */
    public class ABPakComFileResItem : ABPakFileResItemBase
    {
        public FileStream m_fs = null;      // 文件句柄

        override public void init(LoadItem item)
        {
            base.init(item);
        }

        override public GameObject InstantiateObject(string resname)
        {
            byte[] bytes = getBytes(resname);
            return null;
        }
    }
}