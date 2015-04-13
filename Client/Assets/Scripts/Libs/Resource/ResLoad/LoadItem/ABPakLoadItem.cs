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

            string curExt;

            // 如果是打包的资源
            if (Ctx.m_instance.m_cfg.m_pakExtNameList.IndexOf(m_extName) != -1)
            {
                curExt = FileResItem.UNITY3D_EXT;
            }
            else if(string.IsNullOrEmpty(m_extName))        // 材质 mat 源文件是没有扩展名字的，因为 3dmax 材质扩展名字也是 mat ，可能 unity 怕识别错误了吧
            {
                curExt = FileResItem.UNITY3D_EXT;
            }
            else
            {
                curExt = m_extName;
            }

            string curPath;
            curPath = Path.Combine(Application.streamingAssetsPath, m_pathNoExt);
            curPath = UtilApi.getPakPathAndExt(curPath, curExt);

            if (Ctx.m_instance.m_localFileSys.isFileExist(curPath))
            {
                m_bytes = Ctx.m_instance.m_localFileSys.LoadFileByte(curPath);
            }
            else
            {
                throw new System.Exception("error");
            }

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