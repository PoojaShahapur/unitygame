using SDK.Common;
using System.IO;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 自定义打包文件加载
     */
    public class ABPakLoadItem : LoadItem
    {
        public FileStream m_fs = null;      // 文件句柄

        override public void reset()
        {
            base.reset();
        }

        override public void load()
        {
            base.load();

            m_fs = Ctx.m_instance.m_localFileSys.openFile(m_path);

            if (m_fs != null)
            {
                if (onLoaded != null)
                {
                    onLoaded(this);
                }
            }
            else
            {
                if (onFailed != null)
                {
                    onFailed(this);
                }
            }
        }
    }
}