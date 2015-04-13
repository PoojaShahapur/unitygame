using System.IO;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 打包的资源系统 base
     */
    public class ABUnPakFileResItemBase : FileResItem
    {
        public byte[] m_bytes;
        protected AssetBundle m_bundle;

        override public void unload()
        {
            m_bytes = null;
            m_bundle.Unload(false);
            m_bundle = null;
        }
    }
}