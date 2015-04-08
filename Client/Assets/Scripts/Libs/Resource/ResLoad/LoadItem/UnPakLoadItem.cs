using SDK.Common;
using System.IO;
using UnityEngine;

namespace SDK.Lib
{
    public class UnPakLoadItem : LoadItem
    {
        public byte[] m_bytes;
        public string m_extName;    // 扩展名字

        override public void reset()
        {
            base.reset();
            m_bytes = null;
        }

        override public void load()
        {
            base.load();

            string curExt;

            // 如果是打包的资源
            if (Ctx.m_instance.m_cfg.m_pakExtNameList.IndexOf(m_extName) != -1)
            {
                curExt = FileResItem.UNITY3D_EXT;
            }
            else
            {
                curExt = m_extName;
            }

            string curPath;
            curPath = Path.Combine(Application.streamingAssetsPath, m_path);
            curPath = UtilApi.getPakPathAndExt(curPath, curExt);


            if (Ctx.m_instance.m_localFileSys.isFileExist(curPath))
            {
                m_bytes = Ctx.m_instance.m_localFileSys.LoadFileByte(curPath);
            }

            if (m_bytes != null)
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