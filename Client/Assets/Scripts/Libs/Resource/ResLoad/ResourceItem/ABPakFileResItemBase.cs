using System.IO;

namespace SDK.Lib
{
    /**
     * @brief 打包的资源系统 base
     */
    public class ABPakFileResItemBase : FileResItem
    {
        public FileStream m_fs = null;      // 文件句柄
    }
}