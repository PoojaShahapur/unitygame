using SDK.Common;
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
            if (Ctx.m_instance.m_cfg.m_pakExtNameList.IndexOf(m_extName) != -1)         // 打包成 unity3d 加载的
            {
                if (m_bundle != null)
                {
                    m_bundle.Unload(false);
                    m_bundle = null;
                }
                else
                {
                    Ctx.m_instance.m_logSys.log("unity3d 资源卸载的时候 AssetBundle 加载失败");
                }
            }
        }
    }
}