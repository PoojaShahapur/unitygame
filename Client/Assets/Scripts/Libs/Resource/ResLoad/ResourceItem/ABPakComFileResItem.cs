using System.IO;

namespace SDK.Lib
{
    /**
     * @brief Asset Bundles 打包的普通文件资源
     */
    public class ABPakComFileResItem : FileResItem
    {
        public FileStream m_fs = null;      // 文件句柄
    }
}